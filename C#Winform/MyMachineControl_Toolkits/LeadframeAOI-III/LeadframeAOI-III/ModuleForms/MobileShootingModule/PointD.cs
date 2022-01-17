/* 
 * 二维点
 * byLL 2018/8/7
 */

using System;

namespace MobileShooting
{
    /// <summary>
    /// 二维点
    /// </summary>
    class PointD
    {
        public PointD() { }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public static double GetDistance(PointD pointA, PointD pointB)
        {
            return Math.Sqrt((pointB.X - pointA.X) * (pointB.X - pointA.X) + (pointB.Y - pointA.Y) * (pointB.Y - pointA.Y));
        }
    }
}
