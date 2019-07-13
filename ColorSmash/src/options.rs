//! Defines options that are passed to images::quantize.
//!
//! Option enums belong here, and helper functions that deal with them, but
//! command-line option handling is still in main. This module should be useful
//! for alternate interfaces as well.

use std::ops::Deref;
use std::env;
use std::io;

pub enum ColorType {
    Rgba8,
    Rgb5a3,
}

pub fn color_type(input: Option<String>) -> Result<ColorType, String> {
    match input {
        Some(string) => {
            let colortype = string.to_uppercase();
            match colortype.deref() {
                "RGBA8" => Ok(ColorType::Rgba8),
                "RGB5A3" => Ok(ColorType::Rgb5a3),
                _ => Err(format!("Unknown color type {}", string)),
            }
        }
        None => Ok(ColorType::Rgba8),
    }
}

pub fn get_path() -> io::Result<String> {
    let exe = env::current_exe()?;
    let dir = exe.parent().expect("Executable must be in some directory");
    Ok(dir.to_string_lossy().to_string())
}