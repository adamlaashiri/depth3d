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
        private Vector3 _direction;
        private Vector3 _color;

        public Vector3 Direction { get => _direction; set { _direction = value; } }
        public Vector3 Color { get => _color; set { _color = value; } }
    
        public Light(Vector3 direction, Vector3 color)
        {
            _direction = direction;
            _color = color;
        }
    }
}
