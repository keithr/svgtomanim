using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGToManim
{
    public class Line
    {
        public PointF P1;
        public PointF P2;

        public Line(PointF pt1, PointF pt2)
        {
            P1 = pt1; P2 = pt2;
        }

        public PointF MidPoint()
        {
            return new PointF((P1.X + P2.X) / 2f, (P1.Y + P2.Y) / 2f);
        }
    }

    public class Bezier
    {
        public PointF B; // Begin Point
        public PointF P1; // Control Point
        public PointF P2; // Control Point
        public PointF E; // End Point

        // Made these global so I could diagram the top solution
        public Line L12;
        public Line L23;
        public Line L34;

        public PointF P12;
        public PointF P23;
        public PointF P34;

        public Line L1223;
        public Line L2334;

        public PointF P123;
        public PointF P234;

        public Line L123234;
        public PointF P1234;

        public Bezier(PointF b, PointF p1, PointF p2, PointF e)
        {
            B = b;
            P1 = p1;
            P2 = p2;
            E = e;
        }

        /// <summary>
        /// Consider the classic Casteljau diagram
        /// with the bezier points b, p1, p2, e and lines l12, l23, l34
        /// and their midpoint of line l12 being p12 ...
        /// and the line between p12 p23 being L1223
        /// and the midpoint of line L1223 being P1223 ...
        /// </summary>
        /// <param name="lines"></param>
        public void SplitBezier(List<Line> lines)
        {
            L12 = new Line(this.B, this.P1);
            L23 = new Line(this.P1, this.P2);
            L34 = new Line(this.P2, this.E);

            P12 = L12.MidPoint();
            P23 = L23.MidPoint();
            P34 = L34.MidPoint();

            L1223 = new Line(P12, P23);
            L2334 = new Line(P23, P34);

            P123 = L1223.MidPoint();
            P234 = L2334.MidPoint();

            L123234 = new Line(P123, P234);

            P1234 = L123234.MidPoint();

            if (CurveIsFlat())
            {
                lines.Add(new Line(this.B, this.E));
                return;
            }

            var bz1 = new Bezier(this.B, P12, P123, P1234);
            bz1.SplitBezier(lines);

            var bz2 = new Bezier(P1234, P234, P34, this.E);
            bz2.SplitBezier(lines);
        }

        /// <summary>
        /// Check if points B, P1234 and P1 are colinear (enough).
        /// This is very simple-minded algo... there are better...
        /// </summary>
        /// <returns></returns>
        public bool CurveIsFlat()
        {
            float t1 = (P1.Y - B.Y) * (P2.X - P1.X);
            float t2 = (P2.Y - P1.Y) * (P1.X - B.X);

            float delta = Math.Abs(t1 - t2);

            return delta < 0.05; // Hard-coded constant
        }
    }
}
