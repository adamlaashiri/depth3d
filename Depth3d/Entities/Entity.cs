using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace Depth3d.Entities
{
    public class Entity
    {
        private TexturedModel _model;
        private Vector3 _position;
        private Vector3 _rotation;
        private float _scale;

        // Physics
        private BodyHandle _bodyHandle;
        private bool _static;

        public TexturedModel Model { get => _model; }
        public Vector3 Position { get => _position; set { _position = value; } }
        public Vector3 Rotation { get => _rotation; set { _rotation = value; } }
        public float Scale { get => _scale; }

        public Vector3 Forward { get => Maths.Math.ForwardVector(this); }
        public Vector3 Right { get => Maths.Math.RightVector(this); }

        public BodyHandle BodyHandle { get => _bodyHandle; set { _bodyHandle = value; } }
        public bool Static { get => _static; set { _static = value; } }

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale, bool staticObject)
        {
            _model = model;
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _static = staticObject;
        }

        public void AddRotation(Vector3 rotation)
        {
            _rotation.X += rotation.X;
            _rotation.Y += rotation.Y;
            _rotation.Z += rotation.Z;
        }

        public void AddPosition(Vector3 position)
        {
            _position.X += position.X;
            _position.Y += position.Y;
            _position.Z += position.Z;
        }
    }
}
