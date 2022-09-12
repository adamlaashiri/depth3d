# Depth3d
Game engine with rendering and physics [WIP]. Using OpenGL and JitterPhysics.

## About
This a hobby project that I have been working on. This engine is far from perfect or finished, but provides the perfect grounds for learning programming (OOP), opengl, linear math, physics and 
most importantly, a good architectural pattern for a game engine. I am developing this engine with inheritance, hierarchies and composition in mind. The goal is to implement an
Entity Component System (ECS).

## Demo
https://user-images.githubusercontent.com/56280397/189562772-94200c50-4bdf-4a66-bfa9-ec8a47dbd8ba.mp4

*Vehicle physics still needs work. Sideways friction and wheels rotations will be fixed once I use quaternions to store rotations.*

## TODO :
+ Store entities rotations as quaternions.
+ Add support for multiple light sources.
+ Implement opengl frame buffers.
+ Restructure namespaces & classes.
+ Implement Entity Component System (ECS).
+ Refactor the engine (separate between engine code & game code).
