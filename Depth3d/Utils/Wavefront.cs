using System.IO;
using ObjLoader.Loader.Loaders;
using OpenTK.Mathematics;

namespace Depth3d.Utils
{
    public class Wavefront
    {
        public static Mesh LoadObj(string filename, Loader loader)
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
            }
            catch (Exception e)
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

            // Return parsed .obj into Mesh
            return loader.LoadModel(verticesArray, indicesArray, texCoordsArray, normalsArray);
        }

        public static Mesh LoadObjAdvanced(string filename, Loader loader)
        {
            Mesh oobject = null;
            List<int> indices = new();

            List<float> _vertices = new();
            List<float> _uvs = new();
            List<float> _normals = new();

            #region
            var objLoaderFactory = new ObjLoaderFactory();
            //FileStream materialFileName = File.Open(("../../../Res/" + filename).Replace(".obj", ".mtl"), FileMode.Open);
            var objLoader = objLoaderFactory.Create();
            #endregion
            try
            {
                #region
                var fileStream = new FileStream("../../../Res/" + filename, FileMode.Open);
                var result = objLoader.Load(fileStream);
                #endregion

                for (int i = 0; i < result.Groups.Count; i++)
                {
                    #region
                    Console.WriteLine("Vertices " + result.Vertices.Count);
                    Console.WriteLine("Group " + (i+1) + $":{result.Groups[i].Name}");
                    Console.WriteLine("Faces " + result.Groups[i].Faces.Count);
                    Console.WriteLine();
                    #endregion

                    int faceCount = result.Groups[i].Faces.Count;

                    Vector3[] vertices = new Vector3[faceCount * 3];
                    Vector2[] uvs = new Vector2[faceCount * 3];
                    Vector3[] normals = new Vector3[faceCount * 3];

                    int index = 0;

                    for (int k = 0; k < faceCount; k++)
                    {
                        // Vertex 1
                        int vertexIndex1 = result.Groups[i].Faces[k][0].VertexIndex - 1;
                        int uvIndex1 = result.Groups[i].Faces[k][0].TextureIndex - 1;
                        int normalIndex1 = result.Groups[i].Faces[k][0].NormalIndex - 1;

                        vertices[index] = new Vector3(result.Vertices[vertexIndex1].X, result.Vertices[vertexIndex1].Y, result.Vertices[vertexIndex1].Z);
                        uvs[index] = new Vector2(result.Textures[uvIndex1].X, result.Textures[uvIndex1].Y);
                        normals[index] = new Vector3(result.Normals[normalIndex1].X, result.Normals[normalIndex1].Y, result.Normals[normalIndex1].Z);

                        indices.Add(index);


                        // Vertex 2
                        int vertexIndex2 = result.Groups[i].Faces[k][1].VertexIndex - 1;
                        int uvIndex2 = result.Groups[i].Faces[k][1].TextureIndex - 1;
                        int normalIndex2 = result.Groups[i].Faces[k][1].NormalIndex - 1;

                        vertices[index + 1] = new Vector3(result.Vertices[vertexIndex2].X, result.Vertices[vertexIndex2].Y, result.Vertices[vertexIndex2].Z);
                        uvs[index + 1] = new Vector2(result.Textures[uvIndex2].X, result.Textures[uvIndex2].Y);
                        normals[index + 1] = new Vector3(result.Normals[normalIndex2].X, result.Normals[normalIndex2].Y, result.Normals[normalIndex2].Z);

                        indices.Add(index + 1);


                        // Vertex 3
                        int vertexIndex3 = result.Groups[i].Faces[k][2].VertexIndex - 1;
                        int uvIndex3 = result.Groups[i].Faces[k][2].TextureIndex - 1;
                        int normalIndex3 = result.Groups[i].Faces[k][2].NormalIndex - 1;

                        vertices[index + 2] = new Vector3(result.Vertices[vertexIndex3].X, result.Vertices[vertexIndex3].Y, result.Vertices[vertexIndex3].Z);
                        uvs[index + 2] = new Vector2(result.Textures[uvIndex3].X, result.Textures[uvIndex3].Y);
                        normals[index + 2] = new Vector3(result.Normals[normalIndex3].X, result.Normals[normalIndex3].Y, result.Normals[normalIndex3].Z);

                        indices.Add(index + 2);
                        index += 3;
                    }

                    // flatten vertices, uvs and normals to float series
                    for (int k = 0; k < index; k++)
                    {
                        _vertices.Add(vertices[k].X);
                        _vertices.Add(vertices[k].Y);
                        _vertices.Add(vertices[k].Z);
                        _uvs.Add(uvs[k].X);
                        _uvs.Add(uvs[k].Y);
                        _normals.Add(normals[k].X);
                        _normals.Add(normals[i].Y);
                        _normals.Add(normals[i].Z);
                    }
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            //System.Environment.Exit(0);

            if (_vertices.Count > 0 && _uvs.Count > 0 && _normals.Count > 0 && indices.Count > 0)
                oobject = loader.LoadModel(_vertices.ToArray(), indices.ToArray(), _uvs.ToArray(), _normals.ToArray());

            return oobject;
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
