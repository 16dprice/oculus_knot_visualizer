# File Conventions

## File Format

The files contained in this folder are meant to represent lists of 3D
points corresponding to a representation of the knot or link contained
in that file. The format of the file is such that there is a list of
points followed a newline character. This newline character denotes the
end of a list of points. Thus, it is necessary to have this newline at
the end of each knot file and after every component in a link file.

## File Naming

### Knot Files

knot_a_b.txt represents the knot which has

* **a** crossings

and is the

* **b<sup>th</sup>** knot in the knot table

### Link Files

link_a_b_c.txt represents the link which has

* **a** crossings
* **b** components
  
and is the

* **c<sup>th</sup>** link in link table