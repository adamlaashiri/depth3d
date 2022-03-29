using OpenTK.Mathematics;
using Depth3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Utils
{
    public  class Wavefront
    {
        public static Model LoadObj(string filename, Loader loader)
        {

            string file;
            List<Vector3> vertices = new();
            List<Vector2> texCoords = new();
            List<Vector3> normals = new();
            List<int> indices = new();

            float[] verticesArray = null;
            float[] texCoordsArray = null;
            float[] normalsArray = null;
            int[] indicesArray = null;

            try
            {
                file = File.ReadAllText("../../../Res/" + filename);
            }
            catch (Exception)
            {

                throw new Exception(".obj File not found");
            }

            string[] lines = file.Split('\n');
            List<string> faceLines = new List<string>();

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace(".", ",");
                    string[] line = lines[i].Split(" ");
                    switch (line[0])
                    {
                        case "v": // Vertex
                            vertices.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                            break;

                        case "vt": // Texture coordinate
                            texCoords.Add(new Vector2(float.Parse(line[1]), float.Parse(line[2])));
                            break;

                        case "vn": // Normal vertex
                            normals.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                            break;

                        case "f": // Face
                            faceLines.Add(lines[i]);
                            break;

                        default:
                            break;
                    }
                }
            } catch (Exception e)
            {
                throw new Exception("Invalid .obj file format");
            }

            texCoordsArray = new float[vertices.Count * 2];
            normalsArray = new float[vertices.Count * 3];

            // Map all faces
            for (int i = 0; i < faceLines.Count; i++)
            {
                string[] currentLine = faceLines[i].Split(" ");
                string[] vertex1 = currentLine[1].Split("/");
                string[] vertex2 = currentLine[2].Split("/");
                string[] vertex3 = currentLine[3].Split("/");

                MapVertex(vertex1, indices, texCoords, normals, texCoordsArray, normalsArray);
                MapVertex(vertex2, indices, texCoords, normals, texCoordsArray, normalsArray);
                MapVertex(vertex3, indices, texCoords, normals, texCoordsArray, normalsArray);
            }

            verticesArray = new float[vertices.Count * 3];
            indicesArray = indices.ToArray();

            int vertexPointer = 0;
            foreach (var vertex in vertices)
            {
                verticesArray[vertexPointer++] = vertex.X;
                verticesArray[vertexPointer++] = vertex.Y;
                verticesArray[vertexPointer++] = vertex.Z;
            }

            // Return parsed .obj into Model
            return loader.LoadModel(verticesArray, indicesArray, texCoordsArray, normalsArray);
        }

        private static void MapVertex(string[] vertexData, List<int> indices, List<Vector2> texCoords, List<Vector3> normals, float[] texCoordsArray, float[] normalsArray)
        {
            int currentVertexPointer = int.Parse(vertexData[0]) - 1;
            indices.Add(currentVertexPointer);

            Vector2 currentTexCoord = texCoords[int.Parse(vertexData[1]) - 1];
            texCoordsArray[currentVertexPointer * 2] = currentTexCoord.X;
            texCoordsArray[currentVertexPointer * 2 + 1] = currentTexCoord.Y;

            Vector3 currentNormal = normals[int.Parse(vertexData[2]) - 1];
            normalsArray[currentVertexPointer * 3] = currentNormal.X;
            normalsArray[currentVertexPointer * 3 + 1] = currentNormal.Y;
            normalsArray[currentVertexPointer * 3 + 2] = currentNormal.Z;
        }
    }
}
