//! The main module, which handles the command-line interface.
//!
//! Calls images::quantize to do the actual work of quantization.

#![cfg_attr(test, feature(test))]

use std::env;
use std::path::{Path, PathBuf};
use std::fs;

extern crate image as image_lib;
extern crate num;
extern crate ordered_float;
extern crate png;

extern crate getopts;
use getopts::{Options};

mod color;
mod images;
mod k_means;
mod options;

#[cfg(test)]
extern crate test;

fn main() {
    let mut args = env::args();
    let program = &args.next().unwrap();

    let options = initialize_options();

    let matches = match options.parse(args) {
        Ok(matches) => matches,
        Err(error) => exit_with_bad_args(&error.to_string(), program, options),
    };

    if matches.opt_present("help") {
        print_usage(program, options);
        return;
    }

    if matches.opt_present("version") {
        print!("color_smash {}", env!("CARGO_PKG_VERSION"));
        return;
    }
    
    let app_path = options::get_path().unwrap_or_else(|error| {
        println!("{}", error);
        std::process::exit(1);
    });

    let colortype = options::color_type(matches.opt_str("colortype")).unwrap_or_else(|error| {
        println!("{}", error);
        std::process::exit(1);
    });
    
    let num_colors: u32 = matches
        .opt_get_default("colors", 256)
        .unwrap_or_else(|error| {
            println!("{}", error);
            std::process::exit(1);
        });

    let verbose = matches.opt_present("verbose");
    
    let mut input_files: Vec<String> = Vec::new();
    let in_path = app_path.to_owned() + "/cs";
    
    // Ensure input/output directories are properly created
    assert!(Path::new(&(in_path.to_owned())).is_dir() || std::fs::create_dir(in_path.to_owned()).is_ok());
    assert!(Path::new(&(in_path.to_owned() + "/out")).is_dir() || std::fs::create_dir(in_path.to_owned() + "/out").is_ok());
    
	let paths = fs::read_dir(in_path).unwrap();
    for path in paths {
		let temp = String::from(path.unwrap().path().display().to_string());
		if temp.ends_with(".png") {
			input_files.push(temp);
		}
    }
    
    if num_colors > 256 {
        println!("More than 256 colors in the palette is not supported.");
        std::process::exit(1);
    }
    let input_paths: Vec<&Path> = input_files.iter()
                                                   .map(|input_string| Path::new(input_string))
                                                   .collect();
    
    let output_pathbufs: Vec<PathBuf> = input_paths.iter()
                                                   .map(|input_path| {
                                                       get_output_path(input_path)
                                                   })
                                                   .collect();
    let result = images::quantize(input_paths.into_iter(),
                                  output_pathbufs.iter().map(|o| o.as_path()),
                                  colortype,
                                  num_colors,
                                  verbose);
    
    if let Err(error) = result {
        println!("{}", error);
        std::process::exit(1);
    }
}

fn initialize_options() -> Options {
    let mut options = Options::new();

    options.optflag("h", "help", "print this help message.");
    options.optflag("V", "version", "print version info and exit.");
    options.optflag("v", "verbose", "print detailed output.");
    options.optopt(
        "s",
        "suffix",
        "set custom suffix for output filenames.",
        "SUFFIX",
    );
    options.optopt(
        "c",
        "colortype",
        "set output to RGBA8 (default) or RGB5A3.",
        "TYPE",
    );
    options.optopt(
        "n",
        "colors",
        "set number of colors in output files.",
        "NUMBER",
    );

    options
}

fn print_usage(program: &str, options: Options) {
    let brief = format!("Usage: {} [options] FILE", program);
    print!("{}", options.usage(&brief));
}

fn exit_with_bad_args(error: &str, program: &str, options: Options) -> ! {
    print!("{}\n\n", error);
    print_usage(program, options);
    std::process::exit(1);
}

fn get_output_path(input_file: &Path) -> PathBuf {
    //let stem = input_file.file_stem().unwrap();
	let filename = input_file.file_name().unwrap();
    //let output_suffix = match matches.opt_str("suffix") {
    //    Some(suffix) => suffix,
    //    None => " (smashed)".to_string(),
    //};
    //let output_extension = ".png";
    //let output_name = stem.to_string_lossy().into_owned() + &output_suffix + output_extension;
    let output_name = "./out/".to_owned() + &filename.to_string_lossy().into_owned();
	//println!("{}", output_name);
    input_file.with_file_name(output_name)
}
