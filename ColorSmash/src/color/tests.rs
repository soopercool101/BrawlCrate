use super::*;
use k_means::{Grouped, Input};

#[test]
fn color_distance_test() {
    let test_data = [
        ([0xFF, 0xFF, 0xFF, 0xFF], [0x00, 0x00, 0x00, 0xFF], 3.0),
        ([0xFF, 0xFF, 0xFF, 0xFF], [0x00, 0x00, 0x00, 0x00], 3.0),
    ];
    for &(first_color, second_color, expected_distance) in &test_data {
        let first: Rgba8 = Pixel { data: first_color }.into();
        let second: Rgba8 = Pixel { data: second_color }.into();
        let result = first.simple_distance_to(&second);
        assert_eq!(expected_distance, result);
    }
}

#[test]
fn color_mean_test() {
    let test_data = [
        (
            [([0xFF, 0x80, 0x00, 0xFF], 1), ([0x00, 0x00, 0x00, 0xFF], 1)],
            [0x80, 0x40, 0x00, 0xFF],
        ),
        (
            [([0xFF, 0xFF, 0xFF, 0x00], 1), ([0x80, 0x80, 0x80, 0x00], 1)],
            [0x00, 0x00, 0x00, 0x00],
        ),
        (
            [([0xFF, 0x80, 0x00, 0x80], 2), ([0x00, 0x00, 0x00, 0xFF], 1)],
            [0x80, 0x40, 0x00, 0xAA],
        ),
    ];
    for &(colors, expected_data) in &test_data {
        let nodes: Vec<_> = colors
            .iter()
            .map(|&(color_data, count)| Grouped {
                data: ConvertibleColor::<Rgba8, Rgba8>::from(Pixel { data: color_data }),
                count: count,
            })
            .collect();
        let vector: Vec<_> = nodes.iter().collect();
        let expected_mean: Rgba8 = Pixel {
            data: expected_data,
        }
        .into();
        let result = Grouped::mean_of(&vector);
        assert_eq!(expected_mean, result);
    }
}
