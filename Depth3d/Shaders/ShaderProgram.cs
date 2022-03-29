using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Depth3d.shaders
{
    public abstract class ShaderProgram
    {
        private int _programId;
        private int _vertexShaderId;
        private int _fragmentShaderId;

        public ShaderProgram(string vertexShaderLocation, string fragmentShaderLocation)
        {
            _vertexShaderId = LoadShader(vertexShaderLocation, ShaderType.VertexShader);
            _fragmentShaderId = LoadShader(fragmentShaderLocation, ShaderType.FragmentShader);
            _programId = GL.CreateProgram();

            // Attach the shaders to the program
            GL.AttachShader(_programId, _vertexShaderId);
            GL.AttachShader(_programId, _fragmentShaderId);
            bindAttributes();
            GL.LinkProgram(_programId);
            GL.ValidateProgram(_programId);
            GetAllUniformLocations();
        }
        protected abstract void GetAllUniformLocations();

        protected int getUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(_programId, uniformName);
        }

        public void Start()
        {
            GL.UseProgram(_programId);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        // Detach sahders and delete program
        public void CleanUp()
        {
            Stop();
            GL.DetachShader(_programId, _vertexShaderId);
            GL.DetachShader(_programId, _fragmentShaderId);
            GL.DeleteShader(_vertexShaderId);
            GL.DeleteShader(_fragmentShaderId);
            GL.DeleteProgram(_programId);
            Console.WriteLine("Cleaned up shaders!");
        }

        protected abstract void bindAttributes();

        protected void bindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(_programId, attribute, variableName);
        }

        // Load uniforms
        protected void LoadDouble(int location, double value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadVector3(int location, Vector3 vector)
        {
            GL.Uniform3(location, vector.X, vector.Y, vector.Z);
        }

        protected void LoadBool(int location, bool value)
        {
            GL.Uniform1(location, (value) ? 1.0 : 0);
        }

        protected void LoadMatrix(int location, Matrix4 matrix4)
        {
            GL.UniformMatrix4(location, 1, false, ref matrix4.Row0.X);
        }

        private static int LoadShader(string location, ShaderType type)
        {
            // Create shader object of specified type (vertex|fragment)
            int shaderId = GL.CreateShader(type);

            // Put the shader source in specified shader object
            GL.ShaderSource(shaderId, File.ReadAllText(location));
            GL.CompileShader(shaderId);

            string log = GL.GetShaderInfoLog(shaderId);

            if (!string.IsNullOrEmpty(log))
                throw new Exception(log);

            return shaderId;
        }
    }
}
