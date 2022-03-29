using BepuPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Physics
{
    public struct WheelHandles
    {
        public BodyHandle Wheel;
        public ConstraintHandle SuspensionSpring;
        public ConstraintHandle SuspensionTrack;
        public ConstraintHandle Hinge;
        public ConstraintHandle Motor;
    }
}
