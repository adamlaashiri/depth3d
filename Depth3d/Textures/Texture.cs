using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d
{
    public class Texture
    {
        private int _textureId;
        private float _shineDamper = 1;
        private float _reflectivity = 0;

        public int TextureId { get => _textureId; }
        public float ShineDamper { get => _shineDamper; set { _shineDamper = value; } }
        public float Reflectivity { get => _reflectivity; set { _reflectivity = value; } }

        public Texture(int id)
        {
            _textureId = id;
        }
    }
}
