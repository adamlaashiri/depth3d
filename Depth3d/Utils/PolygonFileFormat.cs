using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Utils
{
    public class PolygonFileFormat
    {
        public static Mesh LoadPly(string filename, Loader loader)
        {
            Mesh m;
            string file;
            List<float> vertices = new();
            List<float> normals = new();
            List<float> uvs = new();
            List<float> colors = new();
            
            List<int> indices = new();

            try
            {
                file = File.ReadAllText("../../../Res/" + filename);
            }
            catch (Exception)
            {
                throw new Exception(".ply File not found");
            }

            string[] lines = file.Remove(0, file.IndexOf("end_header")).Split('\n');
            
            try
            {
            
                // Skipp first line, it contains "end_header"
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] elements = lines[i].Replace('.', ',').Split(" ");
                    int elementCount = elements.Length;
                    switch (elementCount)
                    {
                        case 8:
                        vertices.Add(float.Parse(elements[0]));
                        vertices.Add(float.Parse(elements[1]));
                        vertices.Add(float.Parse(elements[2]));

                        normals.Add(float.Parse(elements[3]));
                        normals.Add(float.Parse(elements[4]));
                        normals.Add(float.Parse(elements[5]));

                        uvs.Add(float.Parse(elements[6]));
                        uvs.Add(float.Parse(elements[7]));
                        break;

                        case 4:
                        indices.Add(int.Parse(elements[1]));
                        indices.Add(int.Parse(elements[2]));
                        indices.Add(int.Parse(elements[3]));
                        break;

                        default:
                        break;
                    }
                }

                m = loader.LoadModel(vertices.ToArray(), indices.ToArray(), uvs.ToArray(), normals.ToArray());
            
            }
            catch(Exception e)
            {
                throw new Exception("Invalid .ply file format");
            }
            

            return m;
        }
    }
}
