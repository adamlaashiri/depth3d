using Depth3d.Entities;
using OpenTK.Mathematics;

namespace Depth3d.Maths
{
    public class Math
    {

        public static Matrix4 createViewMatrix(Camera camera)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix *= Matrix4.CreateTranslation(new Vector3(-camera.Position.X, -camera.Position.Y, -camera.Position.Z));
            matrix *= Matrix4.CreateRotationX(ToRadians(camera.Rotation.X));
            matrix *= Matrix4.CreateRotationY(ToRadians(camera.Rotation.Y));
            matrix *= Matrix4.CreateRotationZ(ToRadians(camera.Rotation.Z));
            return matrix;
        }

        public static Matrix4 createProjectionMatrix(int width, int height, float fov, float nearPlane, float farPlane)
        {
            float aspectRatio = (float)width / (float)height;
            float yScale = (float)((1f / System.Math.Tan(ToRadians(fov / 2f))) * aspectRatio);
            float xScale = yScale / aspectRatio;
            float frustumLength = farPlane - nearPlane;

            Matrix4 matrix = new Matrix4();
            matrix.M11 = xScale;
            matrix.M22 = yScale;
            matrix.M33 = -((farPlane + nearPlane) / frustumLength);
            matrix.M34 = -1;
            matrix.M43 = -((2 * nearPlane * farPlane) / frustumLength);
            matrix.M44 = 0;

            return matrix;
        }
        public static Matrix4 createTransformationMatrix(Vector3 translation, Vector3 rotation, float scale)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix *= Matrix4.CreateScale(scale);
            matrix *= Matrix4.CreateRotationX(ToRadians(rotation.X));
            matrix *= Matrix4.CreateRotationY(ToRadians(rotation.Y));
            matrix *= Matrix4.CreateRotationZ(ToRadians(rotation.Z));
            matrix *= Matrix4.CreateTranslation(translation);
            return matrix;
        }

        // by automaticaddison, source - https://automaticaddison.com/how-to-convert-a-quaternion-into-euler-angles-in-python/
        public static Vector3 QuaternionToEuler(float x, float y, float z, float w)
        {
            float t0 = 2.0f * (w * x + y * z);
            float t1 = 1.0f - 2.0f * (x * x + y * y);
            float eulerX = (MathF.Atan2(t0, t1) * 180) / (float) System.Math.PI;

            float t2 = 2.0f * (w * y - z * x);
            if (t2 > 1.0f)
            {
                t2 = 1.0f;
            }
            else if (t2 < -1.0f)
            {
                t2 = -1.0f;
            }
            float eulerY = (MathF.Asin(t2) * 180) / (float) System.Math.PI;

            float t3 = +2.0f * (w * z + x * y);
            float t4 = +1.0f - 2.0f * (y * y + z * z);
            float eulerZ = (MathF.Atan2(t3, t4) * 180) / (float) System.Math.PI;


            return new Vector3(eulerX, eulerY, eulerZ);
        }

        // Get forward vector3 from rotation
        public static Vector3 ForwardVector(Camera camera)
        {
            Matrix4 matrix = createViewMatrix(camera);
            return new Vector3(matrix.M31, matrix.M32, -matrix.M33);
        }

        public static Vector3 ForwardVector(Entity entity)
        {
            Matrix4 matrix = createTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            return new Vector3(matrix.M31, matrix.M32, matrix.M33);
        }

        public static Vector3 RightVector(Camera camera)
        {
            Matrix4 matrix = createViewMatrix(camera);
            return new Vector3(matrix.M11, matrix.M12, -matrix.M13);
        }

        public static Vector3 RightVector(Entity entity)
        {
            Matrix4 matrix = createTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            return new Vector3(matrix.M11, matrix.M12, matrix.M13);
        }

        private static float ToRadians(float angle)
        {
            return (float)(System.Math.PI / 180) * angle;
        }
    }
}
