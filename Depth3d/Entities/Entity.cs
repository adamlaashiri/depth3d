using BepuPhysics;
using Jitter.Dynamics;
using Jitter.LinearMath;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace Depth3d.Entities
{
    // Composite design pattern could work for a parent/children structure
    public class Entity
    {
        protected readonly TexturedModel _model;
        protected Vector3 _position;
        protected Vector3 _rotation;
        protected float _scale;
        
        protected RigidBody _rigidBody;
        public RigidBody RigidBody { 
            get => _rigidBody; 
            set 
            {
                // Set the rigidbody position to that of the entity
                value.Position = new JVector(_position.X, _position.Y, _position.Z);
                _rigidBody = value; 
            } 
        }

        public TexturedModel Model { get => _model; }
        public Vector3 Position { get => _position; set { _position = value; } }
        public Vector3 Rotation { get => _rotation; set { _rotation = value; } }
        public float Scale { get => _scale; }

        // Relative directional vectors
        public Vector3 Right { get => Maths.Math.RightVector(this); }
        public Vector3 Forward { get => Maths.Math.ForwardVector(this); }
        public Vector3 Up { get => Vector3.Cross(Forward, Right); }

        protected bool _debug = false;

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
        {
            _model = model;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public void Update()
        {
            if (_rigidBody != null)
            {
                _rigidBody.Update();
                _position = new(_rigidBody.Position.X, _rigidBody.Position.Y, _rigidBody.Position.Z);
                var q = Jitter.LinearMath.JQuaternion.CreateFromMatrix(_rigidBody.Orientation);
                _rotation = Maths.Math.QuaternionToEuler(q.X, q.Y, q.Z, q.W);

                if (_debug)
                {
                    Vector3 debugVector = new Quaternion(q.X, q.Y, q.Z, q.W).ToEulerAngles();
                    debugVector.X = Maths.Math.Rad2Deg(debugVector.X);
                    debugVector.Y = Maths.Math.Rad2Deg(debugVector.Y);
                    debugVector.Z = Maths.Math.Rad2Deg(debugVector.Z);
                    
                    Console.WriteLine($"{debugVector.Y}");

                }
            }
        }

        public override string ToString() => $"X:{_position.X}, Y:{_position.Y}, Z:{_position.Z}";
    }
}
