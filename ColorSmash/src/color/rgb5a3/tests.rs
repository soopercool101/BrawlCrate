use super::super::*;
use super::convert_5_bits_to_8;

use k_means::SimpleInput;

#[test]
fn rgba8_to_rgb5a3_test() {
    let test_data = [
        (
            [0xFF, 0x00, 0x08, 0xFF],
            (1 << 15) | (0x1F << 10) | (0 << 5) | 1,
        ),
        (
            [0xED, 0x04, 0x05, 0xED],
            (1 << 15) | (0x1D << 10) | (0 << 5) | 1,
        ),
        (
            [0xEC, 0x08, 0x09, 0xEC],
            (6 << 12) | (0x0E << 8) | (0 << 4) | 1,
        ),
    ];
    for &(test_data, expected_data) in &test_data {
        let test_color = ConvertibleColor::<Rgba8, Rgb5a3>::from(Pixel { data: test_data });
        let expected = Rgb5a3 {
            data: expected_data,
        };
        let result = test_color.as_output();
        assert_eq!(expected, result);
    }
}

#[test]
fn rgb5a3_as_pixel_test() {
    let test_data = [
        (
            (1 << 15) | (0x1F << 10) | (0 << 5) | 1,
            [0xFF, 0x00, 0x08, 0xFF],
        ),
        (
            (1 << 15) | (0x1D << 10) | (0 << 5) | 1,
            [0xEF, 0x00, 0x08, 0xFF],
        ),
        (
            (0x6 << 12) | (0x0E << 8) | (0 << 4) | 1,
            [0xEE, 0x00, 0x11, 0xDB],
        ),
    ];
    for &(test_data, expected_data) in &test_data {
        let test_color = Rgb5a3 { data: test_data };
        let expected = Pixel {
            data: expected_data,
        };
        let result = test_color.as_pixel();
        assert_eq!(expected, result);
    }
}

#[test]
fn rgb5a3_rgb5_accessors_test() {
    let test_data = [
        ((1 << 15) | (0x1F << 10) | (0 << 5) | 1, (0x1F, 0, 1)),
        ((1 << 15) | (0x1D << 10) | (0 << 5) | 1, (0x1D, 0, 1)),
    ];
    for &(test_data, (expected_r, expected_g, expected_b)) in &test_data {
        let test_color = Rgb5a3 { data: test_data };
        let r = test_color.r5();
        let g = test_color.g5();
        let b = test_color.b5();
        assert_eq!(expected_r, r);
        assert_eq!(expected_g, g);
        assert_eq!(expected_b, b);
    }
}

#[test]
fn convert_5_bits_to_8_test() {
    let test_data = [(0, 0), (1, 8), (0x1F, 0xFF)];
    for &(test_data, expected_result) in &test_data {
        let actual_result = convert_5_bits_to_8(test_data);
        assert_eq!(expected_result, actual_result);
    }
}
