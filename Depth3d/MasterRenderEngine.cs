using Depth3d.Entities;
using Depth3d.shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d
{
    // This class will handle all of the rendering code
    public class MasterRenderEngine
    {
        private const float FOV = 70;
        private const float NearPlane = 0.1f;
        private const float FarPlane = 1000f;
        private Matrix4 _projectionMatrix;

        // Sub render engines
        private StaticShader _shader;
        private EntityRenderEngine _entityRenderEngine;

        // Entities to be rendered
        private Dictionary<TexturedModel, List<Entity>> _entities = new();

        public MasterRenderEngine(int width, int height)
        {
            SetProjectionMatrix(Maths.Math.createProjectionMatrix(width, height, FOV, NearPlane, FarPlane));
            _shader = new StaticShader();
            _entityRenderEngine = new EntityRenderEngine(_shader, _projectionMatrix);
        }

        public void SetProjectionMatrix(Matrix4 projectionMatrix)
        {
            _projectionMatrix = projectionMatrix;
        }

        public void Render(Light directionalLight, Camera camera)
        {
            _entityRenderEngine.Prepare();

            _shader.Start();
            _shader.LoadViewMatrix(camera);
            _shader.LoadDirectionalLight(directionalLight);

            _entityRenderEngine.Render(_entities);

            _shader.Stop();
            _entities.Clear();
        }

        public void processEntity(Entity entity)
        {
            // Add entites grouped by distinct TexturedModel
            TexturedModel entityModel = entity.Model;
            List<Entity> batch = _entities.GetValueOrDefault(entityModel);
            if (batch != null)
            {
                batch.Add(entity);
            }
            else
            {
                List<Entity> newBatch = new();
                newBatch.Add(entity);
                _entities.Add(entityModel, newBatch);
            }
        }

        public void CleanUp()
        {
            _shader.CleanUp();
        }
    }
}
