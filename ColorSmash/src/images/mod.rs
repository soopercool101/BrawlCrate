//! Handles quantization of images.

use std::collections::{HashMap, HashSet};
use std::fs::File;
use std::io;
use std::path::Path;

use image_lib;
use image_lib::{ImageError, Pixel as PixelTrait, RgbaImage};

use png;
use png::HasParameters;

use color::combination::ConvertibleColorCombination;
use color::{Color, Pixel, Rgb5a3, Rgba8};
use k_means::Grouped;
use options::ColorType;

#[cfg(test)]
mod tests;

/// Quantize a set of input images, and writes the output.
pub fn quantize<'a, 'b, I, O>(
    input_paths: I,
    output_paths: O,
    colortype: ColorType,
    num_colors: u32,
    verbose: bool,
) -> Result<(), ImageError>
where
    I: Iterator<Item = &'a Path>,
    O: Iterator<Item = &'b Path>,
{
    let images = open_images(input_paths)?;

    let quantization_map =
        quantization_map_from_images_and_color_type(&images, colortype, num_colors, verbose);

    let mut color_combinations = ::std::collections::HashSet::new();
    for color_combination in quantization_map.values() {
        color_combinations.insert(color_combination);
    }

    if verbose {
        println!(
            "{} color combinations in output images",
            color_combinations.len()
        );
    }

    let ordered_color_combinations = order_color_combinations(color_combinations);

    let indexed_quantization_map =
        index_quantization_map(&quantization_map, &ordered_color_combinations);

    let width = images[0].width();
    let height = images[0].height();

    let indexed_image_data = calculate_indexes(images, indexed_quantization_map);
    let (rgb_palettes, alpha_palettes) = calculate_palettes(ordered_color_combinations);

    write_pngs(
        output_paths,
        indexed_image_data,
        rgb_palettes,
        alpha_palettes,
        width,
        height
    )?;

    Ok(())
}

fn open_images<'a, I: Iterator<Item = &'a Path>>(
    input_paths: I,
) -> Result<Vec<RgbaImage>, ImageError> {
    let mut images = Vec::new();
    for input_path in input_paths {
        let image = image_lib::open(input_path)?.to_rgba();
        images.push(image);
    }
    Ok(images)
}

fn quantization_map_from_images_and_color_type(
    images: &[RgbaImage],
    colortype: ColorType,
    num_colors: u32,
    verbose: bool,
) -> HashMap<Vec<Pixel>, Vec<Pixel>> {
    match colortype {
        ColorType::Rgba8 => quantization_map_from_images::<Rgba8>(images, num_colors, verbose),
        ColorType::Rgb5a3 => quantization_map_from_images::<Rgb5a3>(images, num_colors, verbose),
    }
}

fn quantization_map_from_images<O: Color>(
    images: &[RgbaImage],
    num_colors: u32,
    verbose: bool,
) -> HashMap<Vec<Pixel>, Vec<Pixel>> {
    let color_combinations = get_color_combinations::<O>(images);
    let grouped_color_combinations = group_color_combinations(color_combinations);

    if verbose {
        println!(
            "{} color combinations in input images",
            grouped_color_combinations.len()
        );
    }

    quantization_map_from_items(grouped_color_combinations, num_colors, verbose)
}

fn get_color_combinations<O: Color>(
    images: &[RgbaImage],
) -> Vec<ConvertibleColorCombination<Rgba8, O>> {
    let width = images[0].width();
    let height = images[0].height();

    let mut color_combinations = Vec::with_capacity((width as usize) * (height as usize));
    for y in 0..height {
        for x in 0..width {
            let color_combination = ConvertibleColorCombination::<Rgba8, O>::new(
                images
                    .iter()
                    .map(|image| (*image.get_pixel(x, y)).into())
                    .collect(),
            );
            color_combinations.push(color_combination);
        }
    }

    color_combinations
}

fn group_color_combinations<O: Color>(
    color_combinations: Vec<ConvertibleColorCombination<Rgba8, O>>,
) -> Vec<Grouped<ConvertibleColorCombination<Rgba8, O>>> {
    ::k_means::collect_groups(color_combinations.into_iter())
}

fn quantization_map_from_items<O: Color>(
    grouped_color_combinations: Vec<Grouped<ConvertibleColorCombination<Rgba8, O>>>,
    num_colors: u32,
    verbose: bool,
) -> HashMap<Vec<Pixel>, Vec<Pixel>> {
    let (centers, grouped_color_combinations_per_cluster) =
        ::k_means::run(&grouped_color_combinations, num_colors, verbose);

    let mut quantization_map = HashMap::new();
    for (center, grouped_color_combinations) in centers
        .into_iter()
        .zip(grouped_color_combinations_per_cluster.into_iter())
    {
        for grouped_color_combination in grouped_color_combinations {
            quantization_map.insert(
                grouped_color_combination.data.as_pixels(),
                center.as_pixels(),
            );
        }
    }

    quantization_map
}

fn order_color_combinations(color_combinations: HashSet<&Vec<Pixel>>) -> Vec<&Vec<Pixel>> {
    let mut ordered_color_combinations: Vec<&Vec<Pixel>> = color_combinations.into_iter().collect();
    ordered_color_combinations.sort_by_key(|color_combination| {
        let mut distinct_colors = color_combination.len();
        for i in 1..color_combination.len() {
            let color1 = color_combination[i];
            if color_combination[0..i].contains(&color1) {
                distinct_colors -= 1;
            }
        }

        let total_alpha: u32 = color_combination
            .iter()
            .map(|color| u32::from(color[3]))
            .sum();
        let total_red: u32 = color_combination
            .iter()
            .map(|color| u32::from(color[0]))
            .sum();
        let total_green: u32 = color_combination
            .iter()
            .map(|color| u32::from(color[1]))
            .sum();
        let total_blue: u32 = color_combination
            .iter()
            .map(|color| u32::from(color[2]))
            .sum();

        (
            distinct_colors,
            total_alpha,
            total_red + total_green + total_blue,
            total_red,
            total_green,
            total_blue,
        )
    });

    ordered_color_combinations
}

fn index_quantization_map<'a, 'b>(
    quantization_map: &'a HashMap<Vec<Pixel>, Vec<Pixel>>,
    ordered_color_combinations: &'b [&'a Vec<Pixel>],
) -> HashMap<&'a Vec<Pixel>, usize> {
    let mut colors_to_index = HashMap::with_capacity(ordered_color_combinations.len());
    for (index, color_combination) in ordered_color_combinations.iter().enumerate() {
        colors_to_index.insert(color_combination, index);
    }

    quantization_map
        .iter()
        .map(|(key, color_combination)| (key, colors_to_index[&color_combination]))
        .collect()
}

fn calculate_indexes(
    images: Vec<RgbaImage>,
    quantization_map: HashMap<&Vec<Pixel>, usize>,
) -> Vec<u8> {
    let width = images[0].width();
    let height = images[0].height();

    let mut indexes = Vec::with_capacity((width * height) as usize);

    for y in 0..height {
        for x in 0..width {
            let initial_pixels: Vec<_> =
                images.iter().map(|image| *image.get_pixel(x, y)).collect();
            let index = &quantization_map[&initial_pixels];
            indexes.push(*index as u8);
        }
    }

    indexes
}

fn calculate_palettes(color_combinations: Vec<&Vec<Pixel>>) -> (Vec<Vec<u8>>, Vec<Vec<u8>>) {
    let num_palette_entries = color_combinations.len();
    let num_images = color_combinations[0].len();

    let mut rgb_palettes = Vec::with_capacity(num_images);
    let mut alpha_palettes = Vec::with_capacity(num_images);

    for _ in 0..num_images {
        rgb_palettes.push(Vec::with_capacity(num_palette_entries * 3));
        alpha_palettes.push(Vec::with_capacity(num_palette_entries));
    }

    for color_combination in color_combinations {
        for (image_index, pixel) in color_combination.iter().enumerate() {
            let (r, g, b, a) = pixel.channels4();
            rgb_palettes[image_index].push(r);
            rgb_palettes[image_index].push(g);
            rgb_palettes[image_index].push(b);
            alpha_palettes[image_index].push(a);
        }
    }

    (rgb_palettes, alpha_palettes)
}

fn write_pngs<'a, O>(
    output_paths: O,
    indexed_image_data: Vec<u8>,
    rgb_palettes: Vec<Vec<u8>>,
    alpha_palettes: Vec<Vec<u8>>,
    width: u32,
    height: u32,
) -> io::Result<()>
where
    O: Iterator<Item = &'a Path>,
{
    for ((output_path, rgb_palette), alpha_palette) in output_paths
        .zip(rgb_palettes.into_iter())
        .zip(alpha_palettes.into_iter())
    {
        let output = &File::create(output_path)?;
        let mut encoder = png::Encoder::new(output, width, height);
        encoder
            .set(png::ColorType::Indexed)
            .set(png::BitDepth::Eight);

        let mut writer = encoder.write_header()?;
        writer.write_chunk(png::chunk::PLTE, &rgb_palette)?;
        writer.write_chunk(png::chunk::tRNS, &alpha_palette)?;
        writer.write_image_data(&indexed_image_data)?;
    }

    Ok(())
}
