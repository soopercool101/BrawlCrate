use image_lib::Pixel as PixelTrait;

use super::{Color, Pixel};

#[derive(PartialEq, Eq, Hash, Copy, Clone, Debug)]
pub struct Rgba8 {
    data: Pixel,
}

impl Color for Rgba8 {
    fn new(components: (f64, f64, f64, f64)) -> Rgba8 {
        let (r_float, g_float, b_float, a_float) = components;

        let a = (a_float * 255.0).round() as u8;
        if a == 0 {
            return Rgba8 {
                data: Pixel { data: [0, 0, 0, 0] },
            };
        }

        let r = (r_float * 255.0).round() as u8;
        let g = (g_float * 255.0).round() as u8;
        let b = (b_float * 255.0).round() as u8;
        Rgba8 {
            data: Pixel { data: [r, g, b, a] },
        }
    }

    fn as_pixel(&self) -> Pixel {
        self.data
    }

    fn components(&self) -> (f64, f64, f64, f64) {
        let (r, g, b, a) = self.data.channels4();
        (
            f64::from(r) / 255.0,
            f64::from(g) / 255.0,
            f64::from(b) / 255.0,
            f64::from(a) / 255.0,
        )
    }
}

impl From<Pixel> for Rgba8 {
    fn from(pixel: Pixel) -> Self {
        Rgba8 { data: pixel }
    }
}
