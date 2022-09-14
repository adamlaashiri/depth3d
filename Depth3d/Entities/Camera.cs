using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Depth3d.Entities
{
    public class Camera
    {
        private Vector3 _position = new Vector3(0, 0, 5);
        private Vector3 _rotation = new Vector3(0, 0, 0);

        public Vector3 Position { get => _position; set { _position = value; } }
        public Vector3 Rotation { get => _rotation; set { _rotation = value; } }

        public Vector3 Right { get => Maths.Math.RightVector(this); }
        public Vector3 Forward { get => Maths.Math.ForwardVector(this); }
        public Vector3 Up { get => Vector3.Cross(Forward, Right); }

        public Camera(Vector3 position, Vector3 rotation)
        {
            _position = position;
            _rotation = rotation;
        }

        public void UpdateSpeed(float speed)
        {
            var keyboardState = Input.KeyboardState;

            if (keyboardState.IsKeyDown(Keys.A))
                _position += -Right * speed;

            if (keyboardState.IsKeyDown(Keys.D))
                _position += Right * speed;

            if (keyboardState.IsKeyDown(Keys.S))
                _position += -Forward * speed;

            if (keyboardState.IsKeyDown(Keys.W))
                _position += Forward * speed;

            if (keyboardState.IsKeyDown(Keys.Up))
                _position += new Vector3(0, speed, 0);

            if (keyboardState.IsKeyDown(Keys.Down))
                _position += new Vector3(0, -speed, 0);

            if (keyboardState.IsKeyDown(Keys.Left))
                _rotation += new Vector3(0, -1, 0);

            if (keyboardState.IsKeyDown(Keys.Right))
                _rotation += new Vector3(0, 1, 0);
        }

        public void LookAt(Entity target)
        {
            Matrix4 mat = Matrix4.LookAt(Position, target.Position, Vector3.UnitY);
            Rotation = Maths.Math.Quaternion2Euler(mat.ExtractRotation());
        }

        public override string ToString() => $"X:{_position.X}, Y:{_position.Y}, Z:{_position.Z}";
    }
}
