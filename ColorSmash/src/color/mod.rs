//! This implements k-means traits for colors and other support functions.

use std::marker::PhantomData;

use image_lib;
use num::{FromPrimitive, Zero};

use k_means::{Grouped, Input, Output, SimpleInput};

pub mod combination;
mod rgb5a3;
mod rgba8;
pub use self::rgb5a3::Rgb5a3;
pub use self::rgba8::Rgba8;

#[cfg(test)]
mod tests;

pub type Pixel = image_lib::Rgba<u8>;

pub trait Color: Output {
    fn new(components: (f64, f64, f64, f64)) -> Self;

    fn as_pixel(&self) -> Pixel;

    fn components(&self) -> (f64, f64, f64, f64);
    fn simple_distance_to<T: Color>(&self, other: &T) -> Self::Distance {
        let (r1, g1, b1, a1) = self.components();
        let (r2, g2, b2, a2) = other.components();

        let opaque_distance = (r1 - r2).powi(2) + (g1 - g2).powi(2) + (b1 - b2).powi(2);
        let alpha_distance = (a1 - a2).powi(2) * 3.0;

        let distance = (opaque_distance * a1 * a2) + alpha_distance;
        Self::Distance::from_f64(distance).unwrap()
    }
}

impl<T: Color> Output for T {
    type Distance = f64;
    fn distance_to(&self, other: &T) -> Self::Distance {
        self.simple_distance_to(other)
    }
}

#[derive(PartialEq, Eq, Hash, Copy, Clone, Debug)]
pub struct ConvertibleColor<I: Color, O: Color> {
    pub color: I,
    output_type: PhantomData<O>,
}

impl<O: Color> From<Pixel> for ConvertibleColor<Rgba8, O> {
    fn from(pixel: Pixel) -> Self {
        ConvertibleColor {
            color: pixel.into(),
            output_type: PhantomData,
        }
    }
}

impl<I: Color, O: Color> SimpleInput for ConvertibleColor<I, O> {
    type Output = O;
    type Distance = I::Distance;

    fn distance_to(&self, other: &Self::Output) -> Self::Distance {
        self.color.simple_distance_to(other)
    }

    fn normalized_distance(&self, other: &Self::Output) -> Self::Distance {
        let output = self.as_output();
        let closest_possible_distance = self.distance_to(&output);
        let distance = self.distance_to(other);

        if distance < closest_possible_distance {
            println!(
                "Distance from {:?} to {:?} is closer than to output version {:?}",
                self, other, output
            );
            return Self::Distance::zero();
        }

        distance - closest_possible_distance
    }

    fn as_output(&self) -> Self::Output {
        Self::Output::new(self.color.components())
    }
}

impl<I: Color, O: Color> Input for Grouped<ConvertibleColor<I, O>> {
    fn mean_of(grouped_colors: &[&Grouped<ConvertibleColor<I, O>>]) -> Self::Output {
        mean_of_colors(
            grouped_colors
                .iter()
                .map(|&group| (&group.data, group.count)),
        )
    }
}

fn mean_of_colors<'a, I, C, O>(colors_with_counts: I) -> O
where
    I: Iterator<Item = (&'a ConvertibleColor<C, O>, u32)>,
    C: 'a + Color,
    O: 'a + Color,
{
    let mut r_sum = 0.0;
    let mut g_sum = 0.0;
    let mut b_sum = 0.0;
    let mut a_sum = 0.0;
    let mut total_count = 0;

    for (data, count) in colors_with_counts {
        let (r, g, b, a) = data.color.components();
        let weighted_a = a * f64::from(count);

        r_sum += r * weighted_a;
        g_sum += g * weighted_a;
        b_sum += b * weighted_a;
        a_sum += weighted_a;
        total_count += count;
    }

    if a_sum > 0.0 {
        let r = r_sum / a_sum;
        let g = g_sum / a_sum;
        let b = b_sum / a_sum;
        let a = a_sum / f64::from(total_count);

        O::new((r, g, b, a))
    } else {
        O::new((0.0, 0.0, 0.0, 0.0))
    }
}
