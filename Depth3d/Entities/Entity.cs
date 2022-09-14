using BepuPhysics;
using Jitter.Dynamics;
using Jitter.LinearMath;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace Depth3d.Entities
{
    // Composite design pattern will be implemented later on
    public class Entity
    {
        protected readonly TexturedModel _model;
        public TexturedModel Model { get => _model; }

        protected Vector3 _position;
        protected Quaternion _rotation;
        protected float _scale;
        
        protected RigidBody _rigidBody;
        public RigidBody RigidBody { 
            get => _rigidBody; 
            set 
            {
                // Set the rigidbody position to that of the entity
                value.Position = new JVector(_position.X, _position.Y, _position.Z);
                value.Orientation = JMatrix.CreateFromQuaternion(new JQuaternion(_rotation.X, _rotation.Y, _rotation.Z, _rotation.W));
                _rigidBody = value; 
            } 
        }

        public Vector3 Position { get => _position; set { _position = value; } }
        
        // Rotation is stored as a quaternion, but manipulated through euler angles
        public Quaternion Orientation { get => _rotation; set { _rotation = value; } }
        public Vector3 EulerRotation { 
            set 
            {
                Vector3 rad = Maths.Math.ToRadians(value);
                _rotation = Quaternion.FromEulerAngles(rad.X, rad.Y, rad.Z); 
            } 
        }
        
        public float Scale { get => _scale; }

        // Relative directional vectors
        public Vector3 Right { get => Maths.Math.RightVector(this); }
        public Vector3 Forward { get => Maths.Math.ForwardVector(this); }
        public Vector3 Up { get => Vector3.Cross(Forward, Right); }

        protected bool _debug = false;

        public Entity(TexturedModel model, Vector3 position, Vector3 eulerOrientation, float scale)
        {
            _model = model;
            _position = position;
            EulerRotation = eulerOrientation;
            _scale = scale;
        }
        
        public void Update()
        {
            if (_rigidBody != null)
            {
                _rigidBody.Update();
                _position = new(_rigidBody.Position.X, _rigidBody.Position.Y, _rigidBody.Position.Z);
                JQuaternion o = JQuaternion.CreateFromMatrix(_rigidBody.Orientation);
                _rotation = new Quaternion(o.X, o.Y, o.Z, o.W);
            }
        }
        
        public void Rotate(Vector3 angles)
        {
            Vector3 rad = Maths.Math.ToRadians(angles);
            _rotation = _rotation * Quaternion.FromEulerAngles(new Vector3(rad.X, rad.Y, rad.Z));
        }

        public override string ToString() => $"X:{_position.X}, Y:{_position.Y}, Z:{_position.Z}";
    }
}
