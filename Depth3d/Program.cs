using BepuPhysics;
using BepuPhysics.Collidables;
using Depth3d;
using Depth3d.Entities;
using Depth3d.Models;
using Depth3d.Physics;
using Depth3d.shaders;
using Depth3d.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

GameWindow window = Window.CreateWindow("DEPTH", 1000, 800);

// Loader, render engine and physics engine
Loader loader = new Loader();
MasterRenderEngine renderEngine = new MasterRenderEngine(window.Size.X, window.Size.Y);
PhysicsEngine physicsEngine = new PhysicsEngine(window.UpdateFrequency);

// Init physics engine
physicsEngine.Init();

// Models and textures
Model platformModel = Wavefront.LoadObj("plane.obj", loader);
Texture platformTexture = loader.LoadTexture("standard.png");

Model model = Wavefront.LoadObj("untitled.obj", loader);
Texture texture = loader.LoadTexture("crate.png");

Model gokartModel = Wavefront.LoadObj("gokart_blender.obj", loader);
Texture gokartTexture = loader.LoadTexture("body_msh_albedo.jpg");

TexturedModel texturedModel = new TexturedModel(model, texture);
TexturedModel platformTexturedModel = new TexturedModel(platformModel, platformTexture);
TexturedModel gokartTexturedModel = new TexturedModel(gokartModel, gokartTexture);

// Entities
Camera camera = new Camera(new Vector3(40, 25, 100), new Vector3(0, 0, 0));
Light ambientLight = new Light(new Vector3(-50, 50, 50), new Vector3(1, 1, 1));

Entity platform = new Entity(platformTexturedModel, new Vector3(0, -10, 0), new Vector3(0f, 0f, 0f), 1f, true);
Entity gokart = new Entity(gokartTexturedModel, new Vector3(0, 0, 0), new Vector3(0f, 0f, 0f), 1f, true);

List<Entity> entities = new List<Entity>();
entities.Add(platform);
entities.Add(gokart);

// Physics
physicsEngine.AddStatic(new Vector3(platform.Position.X, platform.Position.Y, platform.Position.Z), new Vector3(512, 0.1f, 512));

var boxShape = new Box(2, 2, 2);
var boxInertia = boxShape.ComputeInertia(1);
var boxIndex = physicsEngine.AddShape(boxShape);

for (int i = 0; i < 5000; i++)
{
    Random rand = new Random();
    var x = (float)rand.NextDouble() * 50;
    var y = (float)rand.NextDouble() * 50 + 50;
    var z = (float)rand.NextDouble() * 50;
    var xRot = 0;
    var yRot = 0;
    var zRot = 0;

    Entity tmpObject = new Entity(texturedModel, new Vector3(x, y, z), new Vector3(xRot, yRot, zRot), 1f, false);
    tmpObject.BodyHandle = physicsEngine.AddBody(BodyDescription.CreateDynamic(
        new System.Numerics.Vector3(x, y, z),
        boxInertia,
        boxIndex,
        0.01f
    ));

    entities.Add(tmpObject);
}

// Events
//window.Load += WindowLoad;
window.UpdateFrame += UpdateFrame;
window.RenderFrame += RenderFrame;
window.Closing += WindowClosing;
window.Resize += WindowResize;

// Run window
window.Run();

void UpdateFrame(FrameEventArgs obj)
{
    Input.KeyboardState = window.KeyboardState;

    // Physics
    physicsEngine.Update();
    physicsEngine.UpdateEntities(entities);

    
    foreach (var item in entities)
    {
        //physicsEngine.AddForce(item.BodyHandle, new System.Numerics.Vector3(item.Forward.X, item.Forward.Y, item.Forward.Z));
    }
    
    // Game logic
    camera.UpdateSpeed(1.5f);
}

void RenderFrame(FrameEventArgs obj)
{
    foreach (var entity in entities)
    {
        renderEngine.processEntity(entity);
    }
    renderEngine.Render(ambientLight, camera);
    window.SwapBuffers();
}

void WindowClosing(System.ComponentModel.CancelEventArgs obj)
{
    loader.CleanUp();
    renderEngine.CleanUp();
    physicsEngine.Dispose();
}

void WindowResize(ResizeEventArgs obj)
{
    //GL.Viewport(0, 0, window.Size.X, window.Size.Y);
}