using BepuPhysics;
using BepuPhysics.Constraints;
using Depth3d.Models;
using Depth3d.Physics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth3d.Entities
{
    public class Vehicle
    {
        public BodyHandle Body;
        public WheelHandles FrontLeftWheel;
        public WheelHandles FrontRightWheel;
        public WheelHandles BackLeftWheel;
        public WheelHandles BackRightWheel;

        private Vector3 _suspensionDirection;
        private AngularHinge _hingeDescription;

        public void Steer()
        {

        }
    }
}
