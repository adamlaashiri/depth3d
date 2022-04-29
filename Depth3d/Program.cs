using BepuPhysics;
using BepuPhysics.Collidables;
using Depth3d;
using Depth3d.Entities;
using Depth3d.Physics;
using Depth3d.shaders;
using Depth3d.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

GameWindow window = Window.CreateWindow("DEPTH", 1200, 1000);

// Loader, render engine and physics engine
Loader loader = new Loader();
MasterRenderEngine renderEngine = new MasterRenderEngine(window.Size.X, window.Size.Y);
PhysicsEngine physicsEngine = new PhysicsEngine(window.UpdateFrequency);

// Models and textures

Depth3d.Mesh platformModel = Wavefront.LoadObj("plane.obj", loader);
Texture platformTexture = loader.LoadTexture("concrete.jpg");

Depth3d.Mesh model = Wavefront.LoadObj("untitled.obj", loader);
Texture texture = loader.LoadTexture("box_textures/crate.png");
texture.ShineDamper = 128f;
texture.Reflectivity = 0.45f;

Depth3d.Mesh dragonModel = Wavefront.LoadObj("dragon.obj", loader);
Texture dragonTexture = loader.LoadTexture("dragon.png");

TexturedModel platformTexturedModel = new TexturedModel(platformModel, platformTexture);
TexturedModel crate = new TexturedModel(model, texture);
TexturedModel dragon = new TexturedModel(dragonModel, dragonTexture);

// Entities
Camera camera = new Camera(new Vector3(40, 25, 100), new Vector3(0, 0, 0));
Light directionalLight = new Light(new Vector3(1, -1, -1), new Vector3(1, 1, 1));

Entity platform = new Entity(platformTexturedModel, new Vector3(0, -10, 0), new Vector3(0f, 0f, 0f), 1f, true);
Entity dragonObject = new Entity(dragon, new Vector3(0, 0, 0), new Vector3(0f, 0f, 0f), 4f, true);

List<Entity> entities = new List<Entity>();
entities.Add(platform);

// Physics stuff
physicsEngine.Init();
physicsEngine.AddStatic(new Vector3(platform.Position.X, platform.Position.Y, platform.Position.Z), new Vector3(512, 0.1f, 512));

var boxShape = new Box(4, 4, 4);
var boxInertia = boxShape.ComputeInertia(1);
var boxIndex = physicsEngine.AddShape(boxShape);

for (int i = 0; i < 200; i++)
{
    Random rand = new Random();
    var x = (float)rand.NextDouble() * 50;
    var y = (float)rand.NextDouble() * 50 + 50;
    var z = (float)rand.NextDouble() * 50;
    var xRot = 0;
    var yRot = 0;
    var zRot = 0;

    Entity tmpObject = new Entity(crate, new Vector3(x, y, z), new Vector3(xRot, yRot, zRot), 2f, false);
    tmpObject.BodyHandle = physicsEngine.AddBody(BodyDescription.CreateDynamic(
        new System.Numerics.Vector3(x, y, z),
        boxInertia,
        boxIndex,
        0.01f
    ));

    entities.Add(tmpObject);
}

double fps = 0.0;

// Events
//window.Load += WindowLoad;
window.UpdateFrame += UpdateFrame;
window.RenderFrame += RenderFrame;
window.Closing += WindowClosing;
window.Resize += WindowResize;

// Run window
window.Run();
window.CenterWindow();

void UpdateFrame(FrameEventArgs obj)
{
    if (window.IsExiting)
        return;

    Input.KeyboardState = window.KeyboardState;

    // Physics
    physicsEngine.Update();
    physicsEngine.UpdateEntities(entities);
    
    // Game logic
    camera.UpdateSpeed(1f);
}

void RenderFrame(FrameEventArgs obj)
{
    if (window.IsExiting)
        return;

    foreach (var entity in entities)
    {
        renderEngine.processEntity(entity);
    }
    renderEngine.Render(directionalLight, camera);
    window.SwapBuffers();

    //meta
    fps = 1.0/window.RenderTime;
    //Console.WriteLine("FPS " + fps);
}

void WindowClosing(System.ComponentModel.CancelEventArgs obj)
{
    loader.CleanUp();
    renderEngine.CleanUp();
    physicsEngine.Dispose();
}

void WindowResize(ResizeEventArgs obj)
{
    GL.Viewport(0, 0, window.Size.X, window.Size.Y);
}