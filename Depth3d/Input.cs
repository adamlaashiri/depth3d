using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d
{
    public class Input
    {
        private static KeyboardState _KeyboardState;

        public static KeyboardState KeyboardState { get => _KeyboardState; set => _KeyboardState = value; }
    }
}
