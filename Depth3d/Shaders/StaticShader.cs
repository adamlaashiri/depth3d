using Depth3d.Entities;
using OpenTK.Mathematics;

namespace Depth3d.shaders
{
    public class StaticShader : ShaderProgram
    {
        private const string VERTEXSHADERLOCATION = "../../../Res/Shaders/vertex_shader.glsl";
        private const string FRAGMENTSHADERLOCATION = "../../../Res/Shaders/fragment_shader.glsl";

        private int _transformationMatrixLocation;
        private int _projectionMatrixLocation;
        private int _viewMatrixLocation;
        private int _lightDirectionLocation;
        private int _lightColorLocation;
        private int _shineDamperLocation;
        private int _reflectivityLocation;
        public StaticShader() : base(VERTEXSHADERLOCATION, FRAGMENTSHADERLOCATION)
        {
        }

        protected override void bindAttributes()
        {
            base.bindAttribute(0, "in_position");
            base.bindAttribute(1, "in_texcoords");
            base.bindAttribute(2, "in_normal");
        }

        protected override void GetAllUniformLocations()
        {
            _transformationMatrixLocation = base.getUniformLocation("transformationMatrix");
            _projectionMatrixLocation = base.getUniformLocation("projectionMatrix");
            _viewMatrixLocation = base.getUniformLocation("viewMatrix");

            _lightDirectionLocation = base.getUniformLocation("lightDirection");
            _lightColorLocation = base.getUniformLocation("lightColor");
            _shineDamperLocation = base.getUniformLocation("shineDamper");
            _reflectivityLocation = base.getUniformLocation("reflectivity");
        }

        // Matrices
        public void LoadTransformationMatrix(Matrix4 matrix) => base.LoadMatrix(_transformationMatrixLocation, matrix);

        public void LoadProjectionMatrix(Matrix4 matrix) => base.LoadMatrix(_projectionMatrixLocation, matrix);

        public void LoadViewMatrix(Camera camera) => base.LoadMatrix(_viewMatrixLocation, Maths.Math.createViewMatrix(camera));
        
        // Lighting
        public void LoadDirectionalLight(Light light)
        {
            base.LoadVector3(_lightDirectionLocation, light.Direction);
            base.LoadVector3(_lightColorLocation, light.Color);
        }

        public void LoadShineVariables(float damper, float reflectivity)
        {
            base.LoadDouble(_shineDamperLocation, damper);
            base.LoadDouble(_reflectivityLocation, reflectivity);
        }
    }
}
