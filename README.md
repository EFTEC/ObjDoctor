# ObjDoctor
Object Doctor is a small tool for open and edit Obj Wavefront 3d format files

It allows to open, edit and save it back 3d model stored in Obj Wavefront format by retaining (in the possible) the format and information of the original file.   This program doesn't rebuild the OBJ file but it works based in the same file but only rebuilding the VERTEX and NORMALS.

So, if a obj file contains 5000 lines, the result also contains 5000 files, even if the result was scaled or translated.

![In action](https://raw.githubusercontent.com/EFTEC/ObjDoctor/master/docs/ObjDoctor.gif "Object Doctor In Action")
In this example, we have a obj file that it's correct but we want to move the model over the Y axis.  So, we open the file, and then we used the option of RESCALE to modify altitude (Y) of the mesh. Since we want to move over the Y axis, then the selected the option MinY to zero and the option ANCHOR Y, to anchor (freeze) this value.

This program solves the next problem:
- It allows to scale the obj file without touching any other information of the OBJ file.
- It allows to translate the obj file.   The translation could be done via ANCHORING a margin or via centering the object.

## Features:   
- Compatible with 3dsmax, zbrush and Modo Obj Wavefront format, and may be another 3d program.  
- It allows to rescale and translate a 3d model.
- It shows statistics of the 3d object such as size, minimum, maximum, center and groups contained inside it.
- It works using 64-bit float precision (15-16 digits precision).

## How to use

![image 1](https://raw.githubusercontent.com/EFTEC/ObjDoctor/master/docs/image1.jpg)

In the first screen, you can see the OBJ file loaded (if any), you could load a new one or you could save a new OBJ file. 

It also shows statistics of the model such as GROUPS (if the model has one), the dimensions, minimum, maximum and center of the mesh.

![image 2](https://raw.githubusercontent.com/EFTEC/ObjDoctor/master/docs/image2.jpg)

In the rescale screen, it's possible to rescale and translate the object.

## Example of Usage

### Move the figure over the Y axis.   

- Check the option **ANCHOR MIN Y**
- Select the **MIN Y** to 0.

### Move the figure under the Y axis.   

- Check the option **ANCHOR MAX Y**
- Select the **MAX Y** to 0.

### Center the figure in the X axis

- Push the **Button CENTER X**

### The figure is 500 wide (X) and I need to scale to 1300.

- Change **Target Dimension X** to 1300.

### The figure must be scaled 300%

- Change the **Percentage** (at the right of Target Dimension) to 300.

### The figure must be scaled until the max height is 1000.

- Uncheck (if it's checked) **ANCHOR MAX Y** and select **MAX Y** to 1000. The figure will be rescaled according this value.






