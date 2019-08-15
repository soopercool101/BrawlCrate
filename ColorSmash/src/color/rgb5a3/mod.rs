use std::fmt;

use super::{Color, Pixel};

#[cfg(test)]
mod tests;

#[derive(PartialEq, Eq, Hash, Copy, Clone)]
pub struct Rgb5a3 {
    data: u16,
}

enum Rgb5a3Type {
    Rgb5,
    Rgb4a3,
}

impl Rgb5a3 {
    fn storage_type(self) -> Rgb5a3Type {
        if (self.data >> 15) & 1 == 1 {
            Rgb5a3Type::Rgb5
        } else {
            Rgb5a3Type::Rgb4a3
        }
    }

    fn r5(self) -> u16 {
        (self.data >> 10) & 0x1F
    }
    fn g5(self) -> u16 {
        (self.data >> 5) & 0x1F
    }
    fn b5(self) -> u16 {
        self.data & 0x1F
    }

    fn a3(self) -> u16 {
        (self.data >> 12) & 0x07
    }
    fn r4(self) -> u16 {
        (self.data >> 8) & 0x0F
    }
    fn g4(self) -> u16 {
        (self.data >> 4) & 0x0F
    }
    fn b4(self) -> u16 {
        self.data & 0x0F
    }
}

fn convert_5_bits_to_8(byte: u16) -> u8 {
    ((byte * 255 + 15) / 31) as u8
}
fn convert_4_bits_to_8(byte: u16) -> u8 {
    (byte * 17) as u8
}
fn convert_3_bits_to_8(byte: u16) -> u8 {
    ((byte * 255 + 3) / 7) as u8
}

impl Color for Rgb5a3 {
    fn new(components: (f64, f64, f64, f64)) -> Rgb5a3 {
        let (r_float, g_float, b_float, a_float) = components;
        let a = (a_float * 7.0).round() as u16;

        let data = match a {
            0 => 0,
            7 => {
                let r = (r_float * 31.0).round() as u16;
                let g = (g_float * 31.0).round() as u16;
                let b = (b_float * 31.0).round() as u16;
                (1 << 15) | (r << 10) | (g << 5) | b
            }
            1..=6 => {
                let r = (r_float * 15.0).round() as u16;
                let g = (g_float * 15.0).round() as u16;
                let b = (b_float * 15.0).round() as u16;
                (a << 12) | (r << 8) | (g << 4) | b
            }
            _ => panic!(
                "Invalid alpha parameter to Rgb5a3::new: {:?} (as 3 bit integer {:?})",
                a_float, a
            ),
        };
        Rgb5a3 { data }
    }

    fn as_pixel(&self) -> Pixel {
        match self.storage_type() {
            Rgb5a3Type::Rgb5 => {
                let r = convert_5_bits_to_8(self.r5());
                let g = convert_5_bits_to_8(self.g5());
                let b = convert_5_bits_to_8(self.b5());
                Pixel {
                    data: [r, g, b, 0xFF],
                }
            }
            Rgb5a3Type::Rgb4a3 => {
                let a = convert_3_bits_to_8(self.a3());
                let r = convert_4_bits_to_8(self.r4());
                let g = convert_4_bits_to_8(self.g4());
                let b = convert_4_bits_to_8(self.b4());
                Pixel { data: [r, g, b, a] }
            }
        }
    }

    fn components(&self) -> (f64, f64, f64, f64) {
        match self.storage_type() {
            Rgb5a3Type::Rgb5 => {
                let r = f64::from(self.r5()) / 31.0;
                let g = f64::from(self.g5()) / 31.0;
                let b = f64::from(self.b5()) / 31.0;
                (r, g, b, 1.0)
            }
            Rgb5a3Type::Rgb4a3 => {
                let a = f64::from(self.a3()) / 7.0;
                let r = f64::from(self.r4()) / 15.0;
                let g = f64::from(self.g4()) / 15.0;
                let b = f64::from(self.b4()) / 15.0;
                (r, g, b, a)
            }
        }
    }
}

impl fmt::Debug for Rgb5a3 {
    fn fmt(&self, fmt: &mut fmt::Formatter) -> fmt::Result {
        match self.storage_type() {
            Rgb5a3Type::Rgb5 => fmt
                .debug_struct("Rgb5a3 (Rgb5)")
                .field("r", &self.r5())
                .field("g", &self.g5())
                .field("b", &self.b5())
                .finish(),
            Rgb5a3Type::Rgb4a3 => fmt
                .debug_struct("Rgb5a3 (Rgb4a3)")
                .field("r", &self.r4())
                .field("g", &self.g4())
                .field("b", &self.b4())
                .field("a", &self.a3())
                .finish(),
        }
    }
}
