using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.CompilerServices;

namespace Depth3d
{
    public class Window
    {
        private const int RENDERFREQUENCY = 120;
        private const int UPDATEFREQUENCY = 60;

        public static GameWindow CreateWindow(string title, int width, int height, int samples)
        {
            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = new NativeWindowSettings();

            // Settings
            gws.RenderFrequency = RENDERFREQUENCY;
            gws.UpdateFrequency = UPDATEFREQUENCY;

            nws.APIVersion = Version.Parse("4.1.0");
            nws.Size = new Vector2i(width, height);
            nws.Title = title;
            nws.NumberOfSamples = samples; // Anti aliasing

            GameWindow window = new GameWindow(gws, nws);

            return window;

        }
    }
}
