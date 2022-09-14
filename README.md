# Depth3d
Game engine with rendering and physics [WIP]. Using OpenGL and JitterPhysics.

## About
This is a hobby project that I have been working on. This engine is far from perfect or finished, but provides the perfect grounds for learning programming (OOP), opengl, linear math, physics and 
most importantly, a good architectural pattern for a game engine. I am developing this engine with abstraction, inheritance, hierarchies and composition in mind. The goal is to implement an Entity Component System (ECS).

## Demo
https://user-images.githubusercontent.com/56280397/190067085-ed1bf780-f25d-410a-9682-8f1eea1704be.mp4



*Vehicle physics still needs work... Wheel spinning is not accurate enough, but more on that once I implement a transform hierarchy*

## TODO :
+ [x] Store entities rotations as quaternions.
+ [ ] Add support for multiple light sources.
+ [ ] Implement opengl frame buffers.
+ [ ] Restructure namespaces & classes.
+ [ ] Implement Scene graph / transform hierarchy
+ [ ] Implement Entity Component System (ECS).
+ [ ] Refactor the engine (separate between engine code & game code).
