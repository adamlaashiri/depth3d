using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Models
{
    public class TexturedModel
    {
        private Model _model;
        private Texture _texture;

        public Model Model { get => _model; }
        public Texture Texture { get => _texture; }

        public TexturedModel(Model model, Texture texture)
        {
            _model = model;
            _texture = texture;
        }
    }
}
