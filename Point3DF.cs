using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{
    class Point3DF : IComparable
    {
        public double X;
        public double Y;
        public double Z;
        public MyList<Line> Lines = new MyList<Line>();
        public MyList<Poligon> Poligons = new MyList<Poligon>();
        public bool OnlyPoint = false;
        public bool Visible  = true;
        public Point3DF(double X, double Y, double Z)
        {
            this.X = (float)X;
            this.Y = (float)Y;
            this.Z = (float)Z;
        }
        public Point3DF(double X, double Y, double Z, bool OnlyPoint)
        {
            this.X = (float)X;
            this.Y = (float)Y;
            this.Z = (float)Z;
            this.OnlyPoint = OnlyPoint;
        }
        public static Point3DF NormirVector(Point3DF vector)
        {
            double Len = Point3DF.LenVecor(vector);
            return vector / Len;
        }
        public static double LenVecor(Point3DF vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2) + Math.Pow(vector.Z, 2));
        }
        public static Point3DF operator +(Double[] Mas, Point3DF obj)
        {
            if (Mas.Length != 3)
                return new Point3DF(0, 0, 0);
            return new Point3DF(obj.X + Mas[0], obj.Y + Mas[1], obj.Z + Mas[2], obj.OnlyPoint);
        }
        public static Point3DF operator +(Point3DF obj, Point3DF obj2)
        {
            return new Point3DF(obj.X + obj2.X, obj.Y + obj2.Y, obj.Z + obj2.Z, obj.OnlyPoint);
        }
        public static Point3DF operator -(Point3DF obj, Double[] Mas)
        {
            if (Mas.Length != 3)
                return null;
            return new Point3DF(obj.X - Mas[0], obj.Y - Mas[1], obj.Z - Mas[2], obj.OnlyPoint);
        }
        public static Point3DF operator -(Point3DF obj, Point3DF obj2)
        {
            return new Point3DF(obj.X - obj.X, obj.Y - obj.Y, obj.Z - obj.Y, obj.OnlyPoint);
        }
        public static Point3DF operator *(Double[,] Mas, Point3DF obj)
        {
            double[,] objM = new double[3, 1] { { obj.X }, { obj.Y }, { obj.Z } };
            double[,] PoinMasRez = testgistogr.Matrix.MultiplicMatrix(Mas, objM);
            return new Point3DF(PoinMasRez[0, 0], PoinMasRez[1, 0], PoinMasRez[2, 0], obj.OnlyPoint);
        }
        public static Point3DF operator *(Double Mas, Point3DF obj)
        {
            return new Point3DF(obj.X * Mas, obj.Y * Mas, obj.Z * Mas, obj.OnlyPoint);
        }
        public static Point3DF operator *(Point3DF obj, Double Mas)
        {
            return Mas * obj;
        }
        public static Point3DF operator /(Point3DF obj, Double Mas)
        {
            return new Point3DF(obj.X / Mas, obj.Y / Mas, obj.Z / Mas, obj.OnlyPoint);
        }
        static public bool EgualPoint(Point3DF P1, Point3DF P2)
        {
            bool rezult = P1.X == P2.X;
            rezult = (P1.Y == P2.Y) && rezult;
            rezult = (P1.Z == P2.Z) && rezult;
            return rezult;
        }

        public int CompareTo(object obj)
        {
            if (obj is Point3DF)
            {
                if (X.CompareTo(((Point3DF)obj).X) != 0)
                    return X.CompareTo(((Point3DF)obj).X);
                if (Y.CompareTo(((Point3DF)obj).Y) != 0)
                    return Y.CompareTo(((Point3DF)obj).Y);
                return Z.CompareTo(((Point3DF)obj).Z);
            }
            return 0;
        }
    }
}
