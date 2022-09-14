using Depth3d;
using Depth3d.Entities;
using Depth3d.Physics;
using Depth3d.Utils;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Mesh = Depth3d.Mesh;
using Window = Depth3d.Window;

GameWindow window = Window.CreateWindow("DEPTH3D", 1200, 1000);

// Loader, render engine and physics engine
Loader loader = new Loader();
MasterRenderEngine renderEngine = new MasterRenderEngine(window.Size.X, window.Size.Y);
PhysicsEngine jitter = new(window.UpdateFrequency);

// Physics stuff
Shape shape = new BoxShape(1, 1, 1);
Shape planeShape = new BoxShape(1000, 0.001f, 1000);
Shape vehicleShape = new BoxShape(2f, 1.45f, 5.5f);
List<RigidBody> bodies = new();

#region resources
// Load resources

Mesh vehicleModel = PolygonFileFormat.LoadPly("CoupeBlue/CoupeBlue.ply", loader);
Texture vehicleTexture = loader.LoadTexture("CoupeBlue/CoupeBlue.png");
vehicleTexture.ShineDamper = 128f;
vehicleTexture.Reflectivity = 0.45f;

Mesh wheelModel = PolygonFileFormat.LoadPly("CoupeBlue/wheel.ply", loader);
Mesh wheelModelFlipped = PolygonFileFormat.LoadPly("CoupeBlue/wheelFlipped.ply", loader);
Texture wheelTexture = loader.LoadTexture("CoupeBlue/wheel_1_Diffuse.png");


Mesh platformModel = Wavefront.LoadObj("plane.obj", loader);
Texture platformTexture = loader.LoadTexture("Textures/asphalt-seamless.jpg");
platformTexture.Reflectivity = 0.1f;

Mesh model = Wavefront.LoadObj("untitled.obj", loader);
Texture texture = loader.LoadTexture("box_textures/crate.png");
texture.ShineDamper = 128f;
texture.Reflectivity = 0.45f;

TexturedModel crate = new TexturedModel(model, texture);
TexturedModel vehicleTexturedModel = new TexturedModel(vehicleModel, vehicleTexture);
TexturedModel wheelTexturedModel = new TexturedModel(wheelModel, wheelTexture);
TexturedModel wheelFlippedTexturedModel = new TexturedModel(wheelModelFlipped, wheelTexture);
TexturedModel platformTexturedModel = new TexturedModel(platformModel, platformTexture);
#endregion

// Entities
#region Camera & lighting
Camera camera = new Camera(new Vector3(25.75f, 2f, 12.25f), new Vector3(0, 0, 0));
Light directionalLight = new Light(new Vector3(1, -1, -1), new Vector3(1, 1, 1));
#endregion

#region Vehicle

// Wheels
Entity wheelFrontLeft   = new Entity(wheelFlippedTexturedModel, new Vector3(), new Vector3(), 1f);
Entity wheelFrontRight  = new Entity(wheelTexturedModel, new Vector3(), new Vector3(), 1f);
Entity wheelRearLeft    = new Entity(wheelFlippedTexturedModel, new Vector3(), new Vector3(), 1f);
Entity wheelRearRight   = new Entity(wheelTexturedModel, new Vector3(), new Vector3(), 1f);

Vehicle vehicle = new Vehicle(vehicleTexturedModel, new Vector3(-10f, -8, -10f), new Vector3(0f, 90f, 0f), 1f) { WheelEntites = new Entity[4] {wheelFrontLeft, wheelFrontRight, wheelRearLeft, wheelRearRight }, WheelRadius = 0.4f, RestLength = 0.45f, SpringTravel = 0.4f, SpringStiffness = 30000, DamperStiffness = 900f, WheelBase = 2.62f, RearTrack = 1.225f, TurnRadius = 4.8f}; //1.525 10.8

RigidBody vehicleBody = new RigidBody(vehicleShape);
vehicleBody.Mass = 1400;
vehicle.RigidBody = vehicleBody;

#endregion

#region Other entities
Entity platform = new Entity(platformTexturedModel, new Vector3(0, -10, 0), new Vector3(0f, 0f, 0f), 1f);
RigidBody platformBody = new RigidBody(planeShape);
platformBody.IsStatic = true;
platform.RigidBody = platformBody;
#endregion

List<Entity> entities = new List<Entity>();

for (int i = 0; i < 250; i++)
{
    Random rand = new Random();
    var x = (float)rand.NextDouble() * 12.5f;
    var y = (float)rand.NextDouble() * 12.5f + 12.5f;
    var z = (float)rand.NextDouble() * 12.5f;
    var xRot = 0;
    var yRot = 0;
    var zRot = 0;

    Material physMat = new Material();
    RigidBody tmpBody = new RigidBody(shape);
    Entity tmpObject = new Entity(crate, new Vector3(x, y, z), new Vector3(xRot, yRot, zRot), 0.5f);
    tmpObject.RigidBody = tmpBody;
    physMat.KineticFriction = 5;
    tmpBody.Material = physMat;
    tmpBody.Mass = 20f;

    bodies.Add(tmpBody);
    entities.Add(tmpObject);
}

entities.Add(platform);
entities.Add(vehicle);
entities.Add(wheelFrontLeft);
entities.Add(wheelFrontRight);
entities.Add(wheelRearLeft);
entities.Add(wheelRearRight);

bodies.Add(platformBody);
bodies.Add(vehicleBody);
jitter.Init(bodies);
double fps = 0.0;

// Events
window.Load += WindowLoad;

window.UpdateFrame += UpdateFrame;
window.RenderFrame += RenderFrame;
window.Closing += WindowClosing;
window.Resize += WindowResize;

// Run window
window.Run();
window.CenterWindow();

void WindowLoad()
{
    vehicle.Start();

}

void UpdateFrame(FrameEventArgs obj)
{
    if (window.IsExiting)
        return;

    Input.KeyboardState = window.KeyboardState;

    // Physics
    jitter.Update();

    for (int i = 0; i < entities.Count; i++)
        entities[i].Update();

    // Game logic
    camera.LookAt(vehicle);
    vehicle.Update(jitter);
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
    fps = 1.0 / window.RenderTime;
    //Console.WriteLine("FPS " + fps);
    //Console.WriteLine(camera.ToString());
}

void WindowClosing(System.ComponentModel.CancelEventArgs obj)
{
    loader.CleanUp();
    renderEngine.CleanUp();
    jitter.CleanUp();
}

void WindowResize(ResizeEventArgs obj)
{
    GL.Viewport(0, 0, window.Size.X, window.Size.Y);
}