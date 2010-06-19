using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Point
    {
        public float x, y, z, angle;
        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
            angle = 0;
        }
        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            angle = 0;
        }
        public Point(float x, float z)
        {
            this.x = x;
            this.z = z;
            angle = 0;
            y = 100; // most likely
        }
        public Point(float x, float y, float z, float angle)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angle = angle;
        }
        // PLACE ALL OTHER MOVEMENT/POSITION COMPRATION FUNCTIONS HERE.

        public float Distance2D(Point otherpoint)
        {
            return Distance2D(otherpoint.x, otherpoint.z);
        }
        public float Distance2D(float x, float z)
        {
            return (float)(Math.Sqrt(
                (this.x - x) * (this.x - x) +
                (this.z - z) * (this.z - z)));
        }
        public static bool operator ==(Point one, Point two)
        {
            return one.IsInRectangle(two, 0.01f);
        }
        public static bool operator !=(Point one, Point two)
        {
            return !one.IsInRectangle(two, 0.01f);
        }

        public float Distance3D(Point otherpoint)
        {
            return Distance3D(otherpoint.x, otherpoint.y, otherpoint.z);
        }
        public float Distance3D(float x, float y, float z)
        {
            return (float)(Math.Sqrt(
                (this.x - x) * (this.x - x) +
                (this.y - y) * (this.y - y) +
                (this.z - z) * (this.z - z)));
        }
        public bool IsInCircle(Point otherpoint, float radius)
        {
            return (Math.Pow((x - otherpoint.x), 2) + Math.Pow(z - otherpoint.z, 2)) <= radius * radius;
        }
        public bool IsInSphere(Point op, float radius)
        {
            Point dist = new Point(
                x - op.x,
                y - op.y,
                z - op.z);
            return (dist.x * dist.x + dist.y * dist.y + dist.z * dist.z) <= radius * radius;
        }
        public bool IsInRectangle(Point op, float radius)
        {
            return (x + radius > x && x - radius < x && z + radius > z && z - radius < z);
        }
        public static float Distance2DPhysics(int movementSpeed, long lastMovementCalculation)
        {
            long tickDiff = DateTime.Now.Ticks - lastMovementCalculation;
            return movementSpeed / tickDiff;
        }
        public void MoveByAngleDegree2D(float distance, float angle)
        {
            MoveByAngleRadian2D(distance, (float)(angle * (Math.PI / 180)));
        }
        public void MoveByAngleRadian2D(float distance, float angle)
        {
            x += (float)Math.Sin(angle) * distance;
            z += (float)-Math.Cos(angle) * distance;
        }
        public bool IsVisible(Point other)
        {
            return IsInCircle(other, 75f);
        }
        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} Z: {2} angle: {3}", x, y, z, angle);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
