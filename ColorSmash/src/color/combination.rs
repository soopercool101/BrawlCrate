//! This covers vectors of colors, for quantizing sets of images.
//!
//! Each color combination represents the list of colors that appear in a
//! given pixel location, with one color per input image.

use color::{Color, ConvertibleColor, Pixel};
use k_means::{Grouped, Input, Output, SimpleInput};

#[derive(PartialEq, Eq, Hash, Clone, Debug)]
pub struct ColorCombination<T: Color> {
    colors: Vec<T>,
}

impl<T: Color> ColorCombination<T> {
    pub fn new(colors: Vec<T>) -> ColorCombination<T> {
        ColorCombination { colors }
    }
    pub fn as_pixels(&self) -> Vec<Pixel> {
        self.colors.iter().map(Color::as_pixel).collect()
    }
}

impl<T: Color> Output for ColorCombination<T> {
    type Distance = T::Distance;
    fn distance_to(&self, other: &ColorCombination<T>) -> Self::Distance {
        self.colors
            .iter()
            .zip(other.colors.iter())
            .map(|(c1, c2)| c1.distance_to(c2))
            .sum()
    }
}

#[derive(PartialEq, Eq, Hash, Clone, Debug)]
pub struct ConvertibleColorCombination<I: Color, O: Color> {
    colors: Vec<ConvertibleColor<I, O>>,
}

impl<I: Color, O: Color> ConvertibleColorCombination<I, O> {
    pub fn new(colors: Vec<ConvertibleColor<I, O>>) -> ConvertibleColorCombination<I, O> {
        ConvertibleColorCombination { colors }
    }
    pub fn as_pixels(&self) -> Vec<Pixel> {
        self.colors
            .iter()
            .map(|input_color| input_color.color.as_pixel())
            .collect()
    }
}

impl<I: Color, O: Color> SimpleInput for ConvertibleColorCombination<I, O> {
    type Output = ColorCombination<O>;
    type Distance = I::Distance;

    fn distance_to(&self, other: &Self::Output) -> Self::Distance {
        self.colors
            .iter()
            .zip(other.colors.iter())
            .map(|(c1, c2)| c1.distance_to(c2))
            .sum()
    }

    fn normalized_distance(&self, other: &Self::Output) -> Self::Distance {
        self.colors
            .iter()
            .zip(other.colors.iter())
            .map(|(c1, c2)| c1.normalized_distance(c2))
            .sum()
    }

    fn as_output(&self) -> Self::Output {
        ColorCombination::new(self.colors.iter().map(SimpleInput::as_output).collect())
    }
}

impl<I: Color, O: Color> Input for Grouped<ConvertibleColorCombination<I, O>> {
    fn mean_of(grouped_colorsets: &[&Grouped<ConvertibleColorCombination<I, O>>]) -> Self::Output {
        mean_of(grouped_colorsets)
    }
}

fn mean_of<I: Color, O: Color>(
    grouped_colorsets: &[&Grouped<ConvertibleColorCombination<I, O>>],
) -> ColorCombination<O> {
    let color_count = grouped_colorsets[0].data.colors.len();
    let mean_colors = (0..color_count)
        .map(|i| {
            let color_iter = grouped_colorsets
                .iter()
                .map(|&group| (&group.data.colors[i], group.count));
            ::color::mean_of_colors(color_iter)
        })
        .collect();
    ColorCombination::new(mean_colors)
}
