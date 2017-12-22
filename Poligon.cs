using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{

    class Poligon : IComparable
    {
        public Point3DF P1;
        public Point3DF P2;
        public Point3DF P3;
        public bool B1 = true;
        public bool B2 = true;
        public bool B3 = true;
        public Color ColorPoligon = Color.White;
        public double Rozseiv = 0;
        public double ForseOtbit = 1;
        public List<Point3DF> Points = new List<Point3DF>();
        public List<Line> Lines = new List<Line>();
        public bool Visible = true;
        public Poligon(Point3DF P1, Point3DF P2, Point3DF P3, bool B1, bool B2, bool B3,Color colorPoligon,double Rozseiv, double ForseOtbit)
        {
            this.P1 = P1;
            Points.Add(P1);
            this.P2 = P2;
            Points.Add(P2);
            this.P3 = P3;
            Points.Add(P3);

            this.B1 = B1;
            this.B2 = B2;
            this.B3 = B3;
            ColorPoligon = colorPoligon;
            this.Rozseiv = Rozseiv;
            this.ForseOtbit = ForseOtbit;
        }
        public Poligon(Point3DF P1, Point3DF P2, Point3DF P3)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.P3 = P3;
        }

        public static bool InArea(Poligon pol, Point3DF point)
        {
            bool rez = true;
            Line[] Perpend = new Line[pol.Lines.Count];
            double[] D = new double[pol.Lines.Count];
            for (int i = 0; i < pol.Lines.Count; i++)
            {
                Perpend[i] = (PerpendicFound(pol.Lines[i], pol.Points[i]));
                Point3DF vector = Line.Vector(Perpend[i]);
                D[i] = -(vector.X * Perpend[i].StartLine.X +
                    vector.Y * Perpend[i].StartLine.Y +
                    vector.Z * Perpend[i].StartLine.Z);
                rez = rez && (vector.X * point.X + vector.Y * point.Y + vector.Z * point.Z + D[i] >= 0);
            }

            return rez;
        }

        static public double PointCheck(double[] ABCD,Point3DF point)
        {
            return ABCD[0] * point.X + ABCD[1] * point.Y + ABCD[2] * point.Z + ABCD[3];
        }
        public static double[] ABCDFound(Poligon pol,bool Right)
        {
            double[] ABCD = new double[4];
            Point3DF A = Line.Vector(new Line(pol.P1, pol.P2, true));
            Point3DF B = Line.Vector(new Line(pol.P2, pol.P3, true));
            //Point3DF L3 = Line.Vector(new Line(pol.P1, pol.P3, true));
            Point3DF vectPer = Line.VectorPerpend2Vector(A, B, Right);
            ABCD[0] =vectPer.X;
            ABCD[1] =vectPer.Y;
            ABCD[2] = vectPer.Z;
            if (!Right)
            {
                ABCD[0] *=-1;
                ABCD[1] *=-1;
                ABCD[2] *=-1;
            }
            ABCD[3] = -(ABCD[0] * pol.P1.X + ABCD[1] * pol.P1.Y + ABCD[2] * pol.P1.Z);
            return ABCD;

        }
        public static Line PerpendicFound(Line line,Point3DF C)
        {
            Point3DF a = Line.Vector(line);
            Point3DF A = line.StartLine;
            Line CD = new Line(new Point3DF(0, 0, 0), C, true);
            double D1, D2;
            if (a.X != 0)
            {
                D1 = a.X * C.X + a.Y * C.Y + a.Z * C.Z +
                    ((Math.Pow(a.Y, 2) / a.X) + (Math.Pow(a.Z, 2) / a.X)) * A.X 
                    - a.Y * A.Y - a.Z * A.Z;
                D2 = a.X + (Math.Pow(a.Y, 2) / a.X) + (Math.Pow(a.Z, 2) / a.X);
                CD.StartLine.X = (float)(D1 / D2);
                CD.StartLine.Y = (a.Y * (CD.StartLine.X - A.X) / a.X) + A.Y;
                CD.StartLine.Z = (a.Z * (CD.StartLine.X - A.X) / a.X) + A.Z;
            }
            else if(a.Y != 0)
            {
                D1 = a.X * C.X + a.Y * C.Y + a.Z * C.Z +
                    ((Math.Pow(a.X, 2) / a.Y) + (Math.Pow(a.Z, 2) / a.Y)) * A.Y 
                    - a.X * A.X - a.Z * A.Z;
                D2 = a.Y + (Math.Pow(a.X, 2) / a.Y) + (Math.Pow(a.Z, 2) / a.Y);
                CD.StartLine.Y = (float)(D1 / D2);
                CD.StartLine.Z = (a.Z * (CD.StartLine.Y - A.Y) / a.Y) + A.Z;
                CD.StartLine.X = (a.X * (CD.StartLine.Y - A.Y) / a.Y) + A.X;
            }
            else if(a.Z != 0)
            {
                D1 = a.X * C.X + a.Y * C.Y + a.Z * C.Z +
                    ((Math.Pow(a.X, 2) / a.Z) + (Math.Pow(a.Y, 2) / a.Z)) * A.Z
                    - a.X * A.X - a.Y * A.Y;
                D2 = a.Z + (Math.Pow(a.X, 2) / a.Z) + (Math.Pow(a.Y, 2) / a.Z);
                CD.StartLine.Z = (float)(D1 / D2);
                CD.StartLine.Y = (a.Y * (CD.StartLine.Z - A.Z) / a.Z) + A.Y;
                CD.StartLine.X = (a.X * (CD.StartLine.Z - A.Z) / a.Z) + A.X;
            }
            return CD;
        }
        public static Poligon operator +(Double[] Mas, Poligon obj)
        {
            if (Mas.Length != 3)
                return null;
            Poligon ChangePoligon = new Poligon(Mas + obj.P1, Mas + obj.P2, Mas + obj.P3, obj.B1, obj.B2, obj.B3,obj.ColorPoligon,obj.Rozseiv,obj.ForseOtbit);
            return ChangePoligon;
        }
        public static Poligon operator -(Poligon obj, Double[] Mas)
        {
            if (Mas.Length != 3)
                return null;
            Poligon ChangePoligon = new Poligon(obj.P1 - Mas, obj.P2 - Mas, obj.P3 - Mas, obj.B1, obj.B2, obj.B3,obj.ColorPoligon, obj.Rozseiv, obj.ForseOtbit);
            return ChangePoligon;
        }
        public static Poligon operator *(Double[,] Mas, Poligon obj)
        {
            Poligon ChangePoligon = new Poligon(Mas * obj.P1, Mas * obj.P2, Mas * obj.P3, obj.B1, obj.B2, obj.B3,obj.ColorPoligon, obj.Rozseiv, obj.ForseOtbit);
            return ChangePoligon;
        }
        public int CompareTo(object obj)
        {
            if (obj is Poligon)
            {
                if (P1.CompareTo(((Poligon)obj).P1) != 0)
                    return P1.CompareTo(((Poligon)obj).P1);
                if (P2.CompareTo(((Poligon)obj).P2) != 0)
                    return P2.CompareTo(((Poligon)obj).P2);
                return P3.CompareTo(((Poligon)obj).P3);
            }
            return 0;
        }
    }
}
