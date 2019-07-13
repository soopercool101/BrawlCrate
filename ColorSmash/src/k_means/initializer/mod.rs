use super::Input;
use num::{Float, FromPrimitive, Zero};
use ordered_float::NotNan;

pub fn initialize_centers<I: Input>(k: u32, points: &[I]) -> (Vec<I::Output>, Vec<Vec<&I>>) {
    let mut centers = Vec::with_capacity(k as usize);
    let first_center = points
        .iter()
        .max_by_key(|point| point.count())
        .unwrap()
        .as_output();
    centers.push(first_center);

    let mut distance_per_point: Vec<_> = points
        .iter()
        .map(|point| point.normalized_distance(&centers[0]))
        .collect();
    let mut cluster_per_point: Vec<_> = vec![0; points.len()];

    let mut distance_per_cluster: Vec<_> = Vec::with_capacity(k as usize);
    let distance_to_first_center = points
        .iter()
        .zip(distance_per_point.iter())
        .map(|(point, distance)| *distance * I::Distance::from_u32(point.count()).unwrap())
        .sum();
    distance_per_cluster.push(distance_to_first_center);

    while centers.len() < (k as usize) {
        let cluster_to_split = worst_cluster(&distance_per_cluster);
        let farthest_point_index =
            farthest_point_of(cluster_to_split, &cluster_per_point, &distance_per_point);
        let new_center = points[farthest_point_index].as_output();

        if centers.iter().any(|center| *center == new_center) {
            println!("Created duplicate center: {:?}", new_center);
        }

        let new_cluster = centers.len();
        distance_per_cluster.push(I::Distance::zero());

        for ((point, distance), cluster) in points
            .iter()
            .zip(distance_per_point.iter_mut())
            .zip(cluster_per_point.iter_mut())
        {
            let new_distance = point.normalized_distance(&new_center);
            if new_distance < *distance {
                // FIXME: -= doesn't work for num::Float
                // distance_per_cluster[*cluster] -= *distance * I::Distance::from_u32(point.count()).unwrap();
                distance_per_cluster[*cluster] = distance_per_cluster[*cluster]
                    - *distance * I::Distance::from_u32(point.count()).unwrap();
                *cluster = new_cluster;
                *distance = new_distance;
                // FIXME: += doesn't work for num::Float
                // distance_per_cluster[new_cluster] += new_distance * I::Distance::from_u32(point.count()).unwrap();
                distance_per_cluster[new_cluster] = distance_per_cluster[new_cluster]
                    + new_distance * I::Distance::from_u32(point.count()).unwrap();
            }
        }
        centers.push(new_center);
    }

    let points_per_cluster = points_per_cluster(points, cluster_per_point, k);

    (centers, points_per_cluster)
}

fn points_per_cluster<I: Input>(
    points: &[I],
    cluster_per_point: Vec<usize>,
    k: u32,
) -> Vec<Vec<&I>> {
    let mut points_per_cluster = vec![Vec::new(); k as usize];

    for (point, cluster) in points.iter().zip(cluster_per_point.into_iter()) {
        points_per_cluster[cluster as usize].push(point);
    }

    points_per_cluster
}

fn worst_cluster<T: Float>(distance_per_cluster: &[T]) -> usize {
    let distances_with_indexes = distance_per_cluster.iter().zip(0..);
    let (_distance, cluster) = distances_with_indexes
        .max_by_key(|&(&distance, _cluster)| NotNan::new(distance).unwrap())
        .unwrap();
    cluster
}

fn farthest_point_of<T: Float>(
    target_cluster: usize,
    cluster_per_point: &[usize],
    distance_per_point: &[T],
) -> usize {
    let point_indexes = cluster_per_point
        .iter()
        .zip(0..)
        .filter_map(|(&cluster, point_index)| {
            if cluster == target_cluster {
                Some(point_index)
            } else {
                None
            }
        });
    let distances_and_indexes = point_indexes.map(|i| (distance_per_point[i], i));
    let (_distance, index) = distances_and_indexes
        .max_by_key(|&(distance, _index)| NotNan::new(distance).unwrap())
        .unwrap();
    index
}
