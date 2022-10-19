using System;

namespace EnhancedShellExample.Infrastructure.Helpers
{
    public class SkiaHelper
    {


        public static (double X1, double Y1, double X2, double Y2) LinearGradientAngleToPoints(double direction)
        {
            //adapt to css style
            direction -= 90;

            //allow negative angles
            if (direction < 0)
                direction = 360 + direction;

            if (direction > 360)
                direction = 360;

            (double x, double y) pointOfAngle(double a)
            {
                return (x: Math.Cos(a), y: Math.Sin(a));
            };

            double degreesToRadians(double d)
            {
                return ((d * Math.PI) / 180);
            }

            var eps = Math.Pow(2, -52);
            var angle = (direction % 360);
            var startPoint = pointOfAngle(degreesToRadians(180 - angle));
            var endPoint = pointOfAngle(degreesToRadians(360 - angle));

            if (startPoint.x <= 0 || Math.Abs(startPoint.x) <= eps)
                startPoint.x = 0;

            if (startPoint.y <= 0 || Math.Abs(startPoint.y) <= eps)
                startPoint.y = 0;

            if (endPoint.x <= 0 || Math.Abs(endPoint.x) <= eps)
                endPoint.x = 0;

            if (endPoint.y <= 0 || Math.Abs(endPoint.y) <= eps)
                endPoint.y = 0;

            return (startPoint.x, startPoint.y, endPoint.x, endPoint.y);
        }


    }
}
