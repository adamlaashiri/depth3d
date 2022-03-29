using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Entities
{
    public class Light
    {
        private Vector3 _position;
        private Vector3 _color;

        public Vector3 Position { get => _position; set { _position = value; } }
        public Vector3 Color { get => _color; set { _color = value; } }
    
        public Light(Vector3 position, Vector3 color)
        {
            _position = position;
            _color = color;
        }
    }
}
