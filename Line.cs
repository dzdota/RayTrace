using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{

    class Line
    {
        public Point3DF StartLine;
        public Point3DF EndLine;
        public MyList<Poligon> Poligons = new MyList<Poligon>();
        public bool OnlyLine = true;
        public bool Visible = true;
        public Color ColLine = Color.Black;
        public Line(Point3DF StartLine, Point3DF EndLine, bool Visible)
        {
            this.StartLine = StartLine;
            this.EndLine = EndLine;
            this.Visible = Visible;
        }
        public Line(Point3DF StartLine, Point3DF EndLine, Color ColLine)
        {
            this.StartLine = StartLine;
            this.EndLine = EndLine;
            this.ColLine = ColLine;
        }
        public Line(Point3DF StartLine, Point3DF EndLine, bool OnlyLine, bool Visible)
        {
            this.StartLine = StartLine;
            this.EndLine = EndLine;
            this.OnlyLine = OnlyLine;
            this.Visible = Visible;
        }
        public Line(Point3DF StartLine, Point3DF EndLine, Color ColLine, bool OnlyLine, bool Visible)
        {
            this.StartLine = StartLine;
            this.EndLine = EndLine;
            this.OnlyLine = OnlyLine;
            this.Visible = Visible;
            this.ColLine = ColLine;
        }
        static public Point3DF Vector(Line line)
        {
            Point3DF Vector = new Point3DF(line.EndLine.X - line.StartLine.X,
                line.EndLine.Y - line.StartLine.Y,
                line.EndLine.Z - line.StartLine.Z);
            return Vector;
        }
        public static double Len(Line line)
        {
            double Len = Math.Sqrt(Math.Pow(line.EndLine.X - line.StartLine.X, 2)
                + Math.Pow(line.EndLine.Y - line.StartLine.Y, 2)
                + Math.Pow(line.EndLine.Z - line.StartLine.Z, 2));
            return Len;
        }
        public static Point3DF PointF(Line line,double Lambda)
        {
            Point3DF vector = Line.Vector(line);
            Point3DF Point = line.StartLine + vector * Lambda;
            return Point;
        }
        public static double PeresechPoint(double[] ABCD, Line line)
        {
            Point3DF Vector = Line.Vector(line);
            double Znam = (ABCD[0] * Vector.X
                + ABCD[1] * Vector.Y
                + ABCD[2] * Vector.Z);
            double Lambda;
            if (Znam != 0)
                Lambda = -(ABCD[0] * line.StartLine.X
                    + ABCD[1] * line.StartLine.Y
                    + ABCD[2] * line.StartLine.Z + ABCD[3])
                    / Znam;
            else Lambda = double.NaN;
            return Lambda;
        }
        public static bool EgualLine(Line line1, Line line2)
        {
            bool rezult = (Math.Min(line1.EndLine.X, line1.StartLine.X) ==
                Math.Min(line2.EndLine.X, line2.StartLine.X));
            if (false == rezult)
                return rezult;
            rezult = (Math.Min(line1.EndLine.Y, line1.StartLine.Y) == 
                Math.Min(line2.EndLine.Y, line2.StartLine.Y))
                && rezult;
            rezult = (Math.Min(line1.EndLine.Z, line1.StartLine.Z) == 
                Math.Min(line2.EndLine.Z, line2.StartLine.Z))
                && rezult;

            rezult = (Math.Max(line1.EndLine.X, line1.StartLine.X) == 
                Math.Max(line2.EndLine.X, line2.StartLine.X))
                && rezult;
            rezult = (Math.Max(line1.EndLine.Y, line1.StartLine.Y) ==
                Math.Max(line2.EndLine.Y, line2.StartLine.Y))
                && rezult;
            rezult = (Math.Max(line1.EndLine.Z, line1.StartLine.Z) == 
                Math.Max(line2.EndLine.Z, line2.StartLine.Z))
                && rezult;
            return rezult;
        }
        public static Line LinePeresechPlosk(double[] ABCD1, double[] ABCD2)
        {
            Point3DF A = new Point3DF(ABCD1[0], ABCD1[1], ABCD1[2]);
            Point3DF B = new Point3DF(ABCD2[0], ABCD2[1], ABCD2[2]);
            double j = A.X * B.X + A.Y * B.Y + A.Z * B.Z;
            Point3DF C = VectorPerpend2Vector(A, B, true);
            Point3DF startline = new Point3DF(0, 0, 0);
            startline.Z = 0;
            if (0 == ABCD1[0])
            {
                startline.Y = -(ABCD1[3] / ABCD1[1]);
                if (0 == ABCD1[0] && 0 == ABCD2[0])
                    startline.X = 0;
                else
                    startline.X = -(ABCD2[1] / ABCD2[0]) * startline.Y - (ABCD2[3] / ABCD2[0]);
            }
            else if (0 == ABCD2[0])
            {
                startline.Y = -(ABCD2[3] / ABCD2[1]);
                if (0 == ABCD1[0] && 0 == ABCD2[0])
                    startline.X = 0;
                else
                    startline.X = -(ABCD1[1] / ABCD1[0]) * startline.Y - (ABCD1[3] / ABCD1[0]);
            }
            else
            {

                startline.Y = ((ABCD2[3] * ABCD1[0]) - (ABCD1[3] * ABCD2[0]))
                    / ((ABCD1[1] * ABCD2[0]) - (ABCD2[1] * ABCD1[0]));
                startline.X = -(ABCD1[1] / ABCD1[0]) * startline.Y - (ABCD1[3] / ABCD1[0]);
            }
            Line line = new Line(startline,startline + C,true);
            return line;

        }
        public static Point3DF VectorPerpend2Vector(Point3DF A, Point3DF B,bool Right)
        {
            Point3DF rez = new Point3DF(0,0,0);
            rez.X = A.Y * B.Z - A.Z * B.Y;
            rez.Y = A.Z * B.X - A.X * B.Z;
            rez.Z = A.X * B.Y - A.Y * B.X;
            if (!Right)
            {
                rez = rez * (-1);
                rez.X *= -1;
                rez.Y  *= -1;
                rez.Z  *= -1;
            }
            return rez;
        }
        static public double AngleFound(Line line1, Line line2)
        {
            Point3DF vect1 = Line.Vector(line1);
            Point3DF vect2 = Line.Vector(line2);

            double acosVal = (vect1.X * vect2.X + vect1.Y * vect2.Y + vect1.Z * vect2.Z)
                / Math.Sqrt((Math.Pow(vect1.X, 2) + Math.Pow(vect1.Y, 2) + Math.Pow(vect1.Z, 2)) *
                (Math.Pow(vect2.X, 2) + Math.Pow(vect2.Y, 2) + Math.Pow(vect2.Z, 2)));
            double angle = Math.Acos(acosVal);
            return angle;
        }

        public static Line LineObr(Line line)
        {
            return new Line(line.EndLine,line.StartLine,line.Visible);
        }
        public static Line operator +(Double[] Mas, Line obj)
        {
            if (Mas.Length != 3)
                return null;
            Line ChangeLine = new Line(Mas + obj.StartLine, Mas + obj.StartLine, obj.ColLine, obj.OnlyLine, obj.Visible);
            return ChangeLine;
        }
        public static Line operator -(Line obj, Double[] Mas)
        {
            if (Mas.Length != 3)
                return null;
            Line ChangeLine = new Line(obj.StartLine - Mas, obj.EndLine - Mas, obj.ColLine, obj.OnlyLine, obj.Visible);
            return ChangeLine;
        }
        public static Line operator *(Double[,] Mas, Line obj)
        {
            Line ChangeLine = new Line(Mas * obj.StartLine, Mas * obj.EndLine, obj.ColLine, obj.OnlyLine, obj.Visible);
            return ChangeLine;
        }
    }
}
