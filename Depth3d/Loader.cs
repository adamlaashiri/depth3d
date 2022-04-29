using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace Depth3d
{
    public class Loader
    {
        private List<int> _vaos = new List<int>();
        private List<int> _vbos = new List<int>();
        private List<int> _textures = new List<int>();

        // Load model data into a vao and return information about the vao in the form of a model
        public Mesh LoadModel(float[] positions, int[] indices, float[] textureCoords, float[] normals)
        {
            int vaoId = CreateVAO();
            bindIndicesBuffer(indices);
            StoreDataInAttributeList(0, 3, positions);
            StoreDataInAttributeList(1, 2, textureCoords);
            StoreDataInAttributeList(2, 3, normals);
            UnbindVAO();
            return new Mesh(vaoId, indices.Length);
        }
        

        public Texture LoadTexture(string textureName)
        {
            // Generate and bind texture
            int textureId = GL.GenTexture();
            
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            Bitmap texture = new Bitmap($"../../../Res/{textureName}");
            texture.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData textureData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Opengl stuff
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            // anisotropic filtering, TODO check if anisotropic filtering capability is enabled
            float anisotropicAmount = MathF.Min(4, GL.GetFloat((GetPName)All.MaxTextureMaxAnisotropy));
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)All.TextureMaxAnisotropy, anisotropicAmount);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Unbind texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            texture.UnlockBits(textureData);
            texture.Dispose();
            _textures.Add(textureId);

            return new Texture(textureId);
        }

        // Create a vao and return its id
        private int CreateVAO()
        {
            int vaoId = GL.GenVertexArray();
            _vaos.Add(vaoId);
            GL.BindVertexArray(vaoId);
            return vaoId;
        }

        // Store data into one of the attribute list of the vao
        private void StoreDataInAttributeList(int attributeNumber, int coordSize, float[] data)
        {
            int vboId = GL.GenBuffer();
            _vbos.Add(vboId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, coordSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        // Bind a new buffer of the type ElementArrayBuffer
        private void bindIndicesBuffer(int[] data)
        {
            int vboId = GL.GenBuffer();
            _vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, BufferUsageHint.StaticDraw);
        }

        // Unbind the vao when finnished using the vao
        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        // Delete all vaos and vbos
        public void CleanUp()
        {
            foreach (var item in _vaos)
                GL.DeleteVertexArray(item);

            foreach (var item in _vbos)
                GL.DeleteBuffer(item);

            foreach (var item in _textures)
            {
                GL.DeleteTexture(item);
            }

            Console.WriteLine("Cleaned up vertexarrays, buffers and textures!");
        }

    }
}
