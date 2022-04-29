using Depth3d.Entities;
using Depth3d.shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Depth3d
{
    public class EntityRenderEngine
    {
        private StaticShader _shader;

        public EntityRenderEngine(StaticShader shader, Matrix4 projectionMatrix)
        {
            _shader = shader;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            // Projection matrix is loaded once
            _shader.Start();
            _shader.LoadProjectionMatrix(projectionMatrix);
            _shader.Stop();
        }

        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0, 0, 0, 1);
        }

        // This render method will bind the model data once per distinct model and draw with different transformations based on given entity
        public void Render(Dictionary<TexturedModel, List<Entity>> entities)
        {
            foreach (var model in entities.Keys)
            {
                PrepareModel(model);
                List<Entity> batch = entities[model];

                foreach (var entity in batch)
                {
                    prepareInstance(entity);

                    // Draw via elements (indices) as opposed to DrawArray (vertices)
                    GL.DrawElements(BeginMode.Triangles, model.Mesh.VertexCount, DrawElementsType.UnsignedInt, 0);
                }
                UnbindModel();
            }
        }

        private void PrepareModel(TexturedModel model)
        {
            // Bind model data
            GL.BindVertexArray(model.Mesh.VaoId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            
            // Load shine variables to shader
            _shader.LoadShineVariables(model.Texture.ShineDamper, model.Texture.Reflectivity);

            // Bind model texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.Texture.TextureId);
        }

        private void prepareInstance(Entity entity)
        {
            Matrix4 tranformationMatrix = Maths.Math.createTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            _shader.LoadTransformationMatrix(tranformationMatrix);
        }

        private void UnbindModel()
        {
            // Unbind model texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // Unbind model data
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindVertexArray(0);
        }
        
    }
}
