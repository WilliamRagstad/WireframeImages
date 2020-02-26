# Wireframe Images
 A file format for rendering wireframe images.

## Examples

### 1. 3D Cube

![Arwing](assets\cube.png)

```bat
@sym xy
l 0 1, 1 1,     red
l 1 1, 1 0,     red
l 0 .5, .5 .5,  red
l .5 .5, .5 0,  red
l 1 1, .5 .5,   #1f5
```



### 2. Star Fox Arwing

![Arwing](assets\ship.png)

```bat
@sym y

# Cockpit
t 0 -187, 100 -187, 140 -112
t 0 -117, 140 -112, 170 13, blue
l 170 13, -1 60

# Engine
l 0 13, 170 13, red
l 0 -105, 155 13, red

l 0 -67, 155 13, red
l 0 -28, 155 13, red

# Top wing
l 165 -20, 427 -627, blue
l 427 -627, 363 0, blue
l 363 0, 170 -3, blue

# Bottom wing
l 363 0, 1580 287, gray
l 1580 287, 170 0, gray
l 168 13, 266 118, gray
l 266 118, 1580 287, gray
```



## Usage

```
WireframeImages.exe [file] [sizeW] [sizeH] [output] (scale) (offsetX) (offsetY) (brushW)

  - sizeW & sizeH is the dimensions of the output image.
  - scale of object.
  - brushW is the width of each wireframe line.
```



## Instruction Syntax

`#` - Comments.

`@`- Directive

| Directive   | Action                                                 |
| ----------- | ------------------------------------------------------ |
| @sym [axis] | Mirror the object in the axis: `X`, `Y` or `XY` (both) |

| Instructions                                             | Action    |
| -------------------------------------------------------- | --------- |
| L [x1] [y1], [x2] [y2] (, [color])                       | Line      |
| T [x1] [y1], [x2] [y2], [x3] [y3] (, [color])            | Triangle  |
| R [x1] [y1], [x2] [y2], [x3] [y3], [x4] [y4] (, [color]) | Rectangle |

All extra spaces are ignored. Everything is case-insensitive.