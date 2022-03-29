using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d
{
    public class Model
    {
        private int _vaoId;
        private int _vertexCount;

        public Model(int vaoId, int vertexCount)
        {
            _vaoId = vaoId;
            _vertexCount = vertexCount;
        }

        public int VaoId { get => _vaoId;}
        public int VertexCount { get => _vertexCount;}


    }
}
