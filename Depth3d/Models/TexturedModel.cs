using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d
{
    public class TexturedModel
    {
        private Mesh _mesh;
        private Texture _texture;

        Dictionary<string, (Mesh, Texture)> _subModels;

        public Mesh Mesh { get => _mesh; }
        public Texture Texture { get => _texture; }

        public TexturedModel(Mesh mesh, Texture texture)
        {
            _mesh = mesh;
            _texture = texture;
            _subModels = new Dictionary<string, (Mesh, Texture)>();
        }

        public void AddSubMesh(string name, Mesh mesh, Texture texture)
        {
            _subModels.Add(name, (Mesh, texture));
        }
    }
}
