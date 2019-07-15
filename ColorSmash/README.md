Color Smash
===========

Color Smash reduces the number of colors in an image to a given amount up to 256, so it could be stored as indexes into a palette.  It can also convert a group of images such that they could be stored as a single, shared set of indexes, with a different palette for each image.

This allows efficient storage of images that are basically the same pattern as each other with different colors.  For example, if you have a game with character costumes that differ only by color, renders of the character with each of the different costumes would work well with this technique.  (Smash Bros. is one example of a game that could have used this, for the images on the character selection screen for picking your outfit.)

Color Smash was initially developed by [Peter Hatch](https://github.com/PeterHatch). This fork is maintained by soopercool101.

Algorithm
---------

Currently Color Smash uses the k-means algorithm, with the distance between two colors calculated as described [here](http://www.imagemagick.org/Usage/bugs/fuzz_distance/).

The initial points are chosen by finding the cluster with the greatest total distance to all nodes, and then placing a new centroid at the node furthest from it, and doing so repeatedly.  In my testing this worked better than random initialization or k-means++.  (Note that I'm optimizing for output quality, not speed.)
