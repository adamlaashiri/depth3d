using Depth3d.Physics;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using ObjLoader.Loader.Data.Elements;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Entities
{
    public class Vehicle : Entity
    {
        // Because of a lack of a transform hierarchy, we have to store positions/velocities of respective wheel and manipulating wheel rotation will prove diffuvult without "hacks",

        public new RigidBody RigidBody
        {
            get => _rigidBody;
            set
            {
                // Set the rigidbody position to that of the entity and disallow deactivation (specific to vehicle class)
                value.AllowDeactivation = false;
                base.RigidBody = value;
            }
        }
        public Entity[] WheelEntites { get; init; }

        private Vector3[] _wheels = new Vector3[4];
        private Vector3[] _oldWheelPos = new Vector3[4];
        public Vector3 WheelDistance = new Vector3(1f, 1.7f, -0.15f); // Relative position offsets to vehicle world space position

        // Suspention system

        public float RestLength { get; set; }
        public float SpringTravel { get; set; }
        public float WheelRadius { get; set; }
        public float SpringStiffness { get; set; }
        public float DamperStiffness { get; set; }

        private float _minLength;
        private float _maxLength = 3f;
        private float _springLength;
        private float _springForce;
        private float _springVelocity;
        private float _damperForce;

        private float[] _oldDist = new float[4];
        private Vector3 _suspensionForce;

        // Steering

        public float WheelBase { get; set; }
        public float RearTrack { get; set; }
        public float TurnRadius { get; set; }

        private float _ackermanAngleLeft;
        private float _ackermanAngleRight;

        // Driving
        private float _fx;
        private float _fy;
        private float _slideFactor = 1f;

        private float[] _rotated = new float[4];

        public Vehicle(TexturedModel model, Vector3 position, Vector3 rotation, float scale) : base(model, position, rotation, scale) {}

        public void Start()
        {
            // Start velocity & suspension distance for respective wheel
            for (int i = 0; i < 4; i++)
            {
                _oldWheelPos[i] = new Vector3(0, 0, 0);
                _oldDist[i] = RestLength;
            }

            _minLength = RestLength - SpringTravel;
            _maxLength = RestLength + SpringTravel;

        }

        public void Update(PhysicsEngine engine)
        {
            base.Update();

            if (_rigidBody == null)
                return;

            var input = Input.KeyboardState;

            // Vehicle physics
            UpdateWheelPositions();
            float driveInput = input.IsKeyDown(Keys.W) ? 1f : input.IsKeyDown(Keys.S) ? -1f : 0f;
            float steerInput = input.IsKeyDown(Keys.A) ? -1f : input.IsKeyDown(Keys.D) ? 1f : 0f;

            for (int i = 0; i < 4; i++)
            {
                Vector3 curr = _wheels[i];

                // Rotate front wheels according to steering angles
                if (i == 0)
                {
                    WheelEntites[i].Orientation = Orientation;
                    WheelEntites[i].Rotate(new Vector3(0, -_ackermanAngleLeft, 0));
                }
                else if (i == 1)
                {
                    WheelEntites[i].Orientation = Orientation;
                    WheelEntites[i].Rotate(new Vector3(0, -_ackermanAngleRight, 0));
                }
                else
                {
                    WheelEntites[i].Orientation = Orientation;
                }

                // Get linear velocity for current wheel in local space (by multiplying orientation with velocity vector)
                Quaternion q = WheelEntites[i].Orientation;
                q.Invert();
                Vector3 wheelLinearVelocity = q * ((curr - _oldWheelPos[i]) / 0.008f);
                

                RaycastHit hit = engine.Raycast(new JVector(curr.X, curr.Y, curr.Z), new JVector(Up.X, Up.Y, Up.Z) * -1, RigidBody);
                if (hit.RigidBody != null && hit.Distance <= _maxLength + WheelRadius)
                {

                    _springLength = hit.Distance - WheelRadius;
                    _springLength = Maths.Math.Clamp(_springLength, _minLength, _maxLength);
                    _springVelocity = (_oldDist[i] - _springLength) / 0.008f;
                    _springForce = SpringStiffness * (RestLength - _springLength);
                    _damperForce = DamperStiffness * _springVelocity;

                    _suspensionForce = (_springForce + _damperForce) * Up;

                    _fx = driveInput * _springForce;
                    _fy = wheelLinearVelocity.X * _springForce * _slideFactor;

                    JVector traction = _fx * new JVector(-WheelEntites[i].Forward.X, -WheelEntites[i].Forward.Y, -WheelEntites[i].Forward.Z);
                    JVector sideTraction = _fy * new JVector(-WheelEntites[i].Right.X, -WheelEntites[i].Right.Y, -WheelEntites[i].Right.Z); // Used forward vectors of wheel before, but wheel rotation messes with that

                    RigidBody.AddForce(new JVector(_suspensionForce.X, _suspensionForce.Y, _suspensionForce.Z) + traction + sideTraction, hit.Point);

                    Vector3 point = new Vector3(hit.Point.X, hit.Point.Y, hit.Point.Z);
                    WheelEntites[i].Position = curr - Up * (_springLength);
                    _oldDist[i] = _springLength;
                }

                // Rotate wheel according to z component of linear velocity
                float rotateFactor = -wheelLinearVelocity.Z / (2 * MathF.PI * WheelRadius) * 360f;
                _rotated[i] += rotateFactor * 0.008f;
                WheelEntites[i].Rotate(new Vector3(-_rotated[i], 0, 0));

            }

            UpdateWheelVelocities();

            if (steerInput < 0) // Turn left
            {
                _ackermanAngleLeft = Maths.Math.Rad2Deg(MathF.Atan(WheelBase / (TurnRadius - (RearTrack / 2))) * steerInput);
                _ackermanAngleRight = Maths.Math.Rad2Deg(MathF.Atan(WheelBase / (TurnRadius + (RearTrack / 2))) * steerInput);
            }
            else if (steerInput > 0) // Turn right
            {
                _ackermanAngleLeft = Maths.Math.Rad2Deg(MathF.Atan(WheelBase / (TurnRadius + (RearTrack / 2))) * steerInput);
                _ackermanAngleRight = Maths.Math.Rad2Deg(MathF.Atan(WheelBase / (TurnRadius - (RearTrack / 2))) * steerInput);
            }
            else
            {
                _ackermanAngleLeft = 0f;
                _ackermanAngleRight = 0f;
            }

            // Drifting is a must
            if (input.IsKeyDown(Keys.Space))
                _slideFactor = MathF.Max(_slideFactor * 0.95f, 0.025f);
            else
                _slideFactor = MathF.Min(_slideFactor * 1.1f, 0.25f);

        }

        private void UpdateWheelVelocities()
        {
            for (int i = 0; i < 4; i++)
                _oldWheelPos[i] = _wheels[i];
        }

        private void UpdateWheelPositions()
        {
            _wheels[0] = _position + ((-Right) * WheelDistance.X + (-Forward) * WheelDistance.Y) + Up * WheelDistance.Z; // Front left
            _wheels[1] = _position + (Right * WheelDistance.X + (-Forward) * WheelDistance.Y) + Up * WheelDistance.Z;    // Front right
            _wheels[2] = _position + ((-Right) * WheelDistance.X + (Forward) * WheelDistance.Y) + Up * WheelDistance.Z;  // Rear left
            _wheels[3] = _position + (Right * WheelDistance.X + (Forward) * WheelDistance.Y) + Up * WheelDistance.Z;    // Rear right
        }
    }
}
