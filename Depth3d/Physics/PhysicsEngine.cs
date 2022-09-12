using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Reflection.Metadata.Ecma335;

namespace Depth3d.Physics
{
    public class PhysicsEngine
    {
        private CollisionSystem _collisionSystem;
        private World _world;
        private float _timeStep;

        public World World { get => _world; }

        public PhysicsEngine(double updateFrequency)
        {
            _collisionSystem = new CollisionSystemSAP();
            _world = new World(_collisionSystem);
            _timeStep = 1f / ((float)updateFrequency);
        }

        public void Init(List<RigidBody> bodies)
        {
            _world.Gravity = new JVector(0, -9.82f, 0);

            for (int i = 0; i < bodies.Count; i++)
                _world.AddBody(bodies[i]);
        }

        public void Update()
        {
            _world.Step(_timeStep, true);
        }

        public RaycastHit Raycast(JVector origin, JVector direction, RigidBody ignore)
        {
            RigidBody resBody;
            JVector hitNormal;
            float fraction;
            bool result = _collisionSystem.Raycast(origin, direction, RaycastCallback, out resBody, out hitNormal, out fraction);
            JVector hitPoint = origin + fraction * (direction) ;


            return new RaycastHit(fraction, resBody, hitPoint, hitNormal);

            // local function (delegate) to selectively ignore rigidbodies when casting a ray
            bool RaycastCallback(RigidBody body, JVector normal, float fraction)
            {
                if (RigidBody.ReferenceEquals(body, ignore))
                    return false;
                return true;
            }
        }

        public void CleanUp()
        {
            _world.Clear();
        }
    }

    public class RaycastHit
    {
        public float Distance { get; }
        public RigidBody RigidBody { get; }
        public JVector Point { get; set; }
        public JVector Normal { get; }

        public RaycastHit(float distance, RigidBody rigidBody, JVector point, JVector normal)
        {
            this.Distance = distance;
            this.RigidBody = rigidBody;
            this.Point = point;
            this.Normal = normal;
        }
    }
}
