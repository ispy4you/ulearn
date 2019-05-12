using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double angle)
        {
            var a = Manipulator.UpperArm;
            var b = Manipulator.Forearm;
            var c = Manipulator.Palm;
            double wristX = x + (c * Math.Cos(Math.PI - angle));
            double wristY = y + (c * Math.Sin(Math.PI - angle));
            double distanceFromShoulderToWrist = Math.Sqrt((wristX * wristX) + (wristY * wristY));
            var elbow = TriangleTask.GetABAngle(a, b, distanceFromShoulderToWrist);
            var angleToWrist = Math.Atan2(wristY, wristX);
            var count = TriangleTask.GetABAngle(a, distanceFromShoulderToWrist, b);
            var shoulder = count + angleToWrist;
            var wrist = -angle - shoulder - elbow;
            return new[]
            {
                shoulder,
                elbow,
                wrist
            };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void RndTestMoveManipulatorTo()
        {
            var random = new Random();
            var x = random.NextDouble() * random.Next(0, 100);
            var y = random.NextDouble() * random.Next(0, 100);
            var getRandomAngle = random.Next(0, 100);
            var angles = ManipulatorTask.MoveManipulatorTo(x, y, getRandomAngle);
            var joints = AnglesToCoordinatesTask.GetJointPositions(
            angles[0],
            angles[1],
            angles[2]);

            var palm = joints[2];
            Assert.AreEqual(x, palm.X, 1e-5);
            Assert.AreEqual(y, palm.Y, 1e-5);
        }
    }
}
