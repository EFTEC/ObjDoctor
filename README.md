# ObjDoctor
Object Doctor is a small tool for open and edit Obj Wavefront 3d format files

It allows to open, edit and save it back 3d model stored in Obj Wavefront format by retaining (in the possible) the original format and style of the file.

![In action](https://raw.githubusercontent.com/EFTEC/ObjDoctor/master/docs/ObjDoctor.gif "Object Doctor In Action")
In this example, we have a obj file that it's correct but we want to move the model over the Y axis.  So, we open the file, and then we used the option of RESCALE to modify altitude (Y) of the mesh. Since we want to move over the Y axis, then the selected the option MinY to zero and the option ANCHOR Y, to anchor (freeze) this value.

This program solves the next problem:
- It allows to scale the obj file without touching any other information of the OBJ file.
- It allows to rescale the obj file.

## Features:   
- Compatible with 3dsmax, zbrush and Modo Obj Wavefront format, and may be another 3d program.  
- It allows to rescale and translate a 3d model.
- It shows statistics of the 3d object such as size, minimum, maximum, center and groups contained inside it.
- It works using 64-bit float precision (15-16 digits precision).




