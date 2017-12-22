using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{

    class ConvertCoordinate
    {
        static public Object ConvertWorldtoCam(Point3DF VectorVzgl, Point3DF Location, Object obj)
        {
            Object ConvertedObj = new Object();
            double[] Mas = new double[3] { Location.X, Location.Y, Location.Z };
            double AngleRotateZ = Line.AngleFound(new Line(new Point3DF(0, 0, 0), new Point3DF(10, 0, 0), false),
                new Line(new Point3DF(0, 0, 0), new Point3DF(VectorVzgl.X, VectorVzgl.Y, 0), false));
            if (VectorVzgl.Y > 0)
                AngleRotateZ *= -1;
            double AngleRotateY = Line.AngleFound(
                new Line(new Point3DF(0, 0, 0), new Point3DF(VectorVzgl.X, VectorVzgl.Y, VectorVzgl.Z), false),
                new Line(new Point3DF(0, 0, 0), new Point3DF(VectorVzgl.X, VectorVzgl.Y, 0), false));
            if (VectorVzgl.Z > 0)
                AngleRotateY *= -1;
            double[,] InversY = new double[3, 3]
            {{1,0,0 },
            { 0,-1,0},
            { 0,0,1}};
            double[,] RotateZ = new double[3, 3]
                {{Math.Cos(AngleRotateZ), Math.Sin(-AngleRotateZ), 0 },
                { Math.Sin(AngleRotateZ), Math.Cos(AngleRotateZ), 0 },
                { 0,0,1} };
            double[,] RotateY = new double[3, 3]
                {{Math.Cos(AngleRotateY),0, Math.Sin(-AngleRotateY)},
                { 0, 1, 0 },
                { Math.Sin(AngleRotateY),0,Math.Cos(AngleRotateY)} };
            ConvertedObj = obj - Mas;
            ConvertedObj = RotateZ * ConvertedObj;
            ConvertedObj = InversY * ConvertedObj;
            ConvertedObj = RotateY * ConvertedObj;
            return ConvertedObj;
        }
        static public List<Line> Camto2D(Object obj, double Focus)
        {
            double L = Focus;
            List<Line> Lines = new List<Line>();
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                double tx1 = L / obj.Lines[i].StartLine.X;
                double tx2 = L / obj.Lines[i].EndLine.X;
                Lines.Add(new Line(
                    new Point3DF(obj.Lines[i].StartLine.Y * tx1, obj.Lines[i].StartLine.Z * tx1, 
                    Math.Sqrt(Math.Pow(obj.Lines[i].StartLine.X,2) 
                    + Math.Pow(obj.Lines[i].StartLine.Y,2) +
                    Math.Pow(obj.Lines[i].StartLine.Z,2))
                    ),
                    new Point3DF(obj.Lines[i].EndLine.Y * tx2, obj.Lines[i].EndLine.Z * tx2,
                    Math.Sqrt(Math.Pow(obj.Lines[i].EndLine.X, 2)
                    + Math.Pow(obj.Lines[i].EndLine.Y, 2) +
                    Math.Pow(obj.Lines[i].EndLine.Z, 2))),
                    obj.Lines[i].ColLine, obj.Lines[i].OnlyLine, obj.Lines[i].Visible));
            }

            return Lines;
        }
        static public List<Line> Camto2D(List<Line> lines, double Focus)
        {
            double L = Focus;
            List<Line> Lines = new List<Line>();
            for (int i = 0; i < lines.Count; i++)
            {
                double tx1 = L / lines[i].StartLine.X;
                double tx2 = L / lines[i].EndLine.X;
                Lines.Add(new Line(
                    new Point3DF(lines[i].StartLine.Y * tx1, lines[i].StartLine.Z * tx1,
                    Math.Sqrt(Math.Pow(lines[i].StartLine.X, 2)
                    + Math.Pow(lines[i].StartLine.Y, 2) +
                    Math.Pow(lines[i].StartLine.Z, 2))
                    ),
                    new Point3DF(lines[i].EndLine.Y * tx2, lines[i].EndLine.Z * tx2,
                    Math.Sqrt(Math.Pow(lines[i].EndLine.X, 2)
                    + Math.Pow(lines[i].EndLine.Y, 2) +
                    Math.Pow(lines[i].EndLine.Z, 2))),
                    lines[i].ColLine, lines[i].OnlyLine, lines[i].Visible));
            }

            return Lines;
        }
        static public Object ObjectHideLine(Object obj, List<Object> World)
        {
            for (int NumPoint = 0;NumPoint<obj.Points.Count;NumPoint++)
            {
                obj.Points[NumPoint].Visible = PointHide(obj.Points[NumPoint], World);
                /*Line LinePoint= new Line(obj.Points[NumPoint],new Point3DF(0,0,0),true);
                for (int NumObj = 0;NumObj<World.Count;NumObj++)
                {
                    for (int NumPoligon = 0; NumPoligon < World[NumObj].Poligons.Count; NumPoligon++)
                    {
                        double[] ABCD = Poligon.ABCDFound(World[NumObj].Poligons[NumPoligon], true);
                        double t = Line.PeresechPoint(ABCD, LinePoint);
                        if (Math.Round(t,5) <= 0 ||t>1)
                            continue;
                        else
                        {
                            Point3DF PointPeresch = Line.PointF(LinePoint, t);
                            if (Poligon.InArea(World[NumObj].Poligons[NumPoligon], PointPeresch))
                            {
                                obj.Points[NumPoint].Visible = false;
                                goto m1;
                            }
                        }
                    }
                }
                m1:;*/
            }
            for (int NumLine = 0; NumLine < obj.Lines.Count; NumLine++)
            {
                if (obj.Lines[NumLine].StartLine.Visible == false && obj.Lines[NumLine].EndLine.Visible == false)
                    obj.Lines[NumLine].Visible = false;
            }
            return obj;
        }
        static public bool PointHide(Point3DF point, List<Object> World)
        {
                Line LinePoint = new Line(point, new Point3DF(0, 0, 0), true);
                for (int NumObj = 0; NumObj < World.Count; NumObj++)
                {
                    for (int NumPoligon = 0; NumPoligon < World[NumObj].Poligons.Count; NumPoligon++)
                    {
                        double[] ABCD = Poligon.ABCDFound(World[NumObj].Poligons[NumPoligon], true);
                        double t = Line.PeresechPoint(ABCD, LinePoint);
                        if (Math.Round(t, 5) <= 0 || t > 1)
                            continue;
                        else
                        {
                            Point3DF PointPeresch = Line.PointF(LinePoint, t);
                            if (Poligon.InArea(World[NumObj].Poligons[NumPoligon], PointPeresch))
                            {
                            return false;
                            }
                        }
                    }
                }
            m1:;
            return true;
        }
        static public bool LineHide(Line line, List<Object> World)
        {
            Point3DF p1 = Line.PointF(line, 0.02);
            Point3DF p2 = Line.PointF(line, 0.98);
            bool b1 = PointHide(p1, World);
            bool b2 = PointHide(p2, World);
            if (!b1 && !b2)
                return false;
            if (b1 && b2)
                return true;
            else
                return true;
        }
        static public List<Object> CorectObj(List<Object> World)
        {
            List<Object> CorectWorld = new List<Object>();
            for (int NumObj = 0; NumObj < World.Count; NumObj++)
            {
                CorectWorld.Add(new Object());
                for (int NumPol = 0; NumPol < World[NumObj].Poligons.Count; NumPol++)
                {
                    for (int NumObj2 = 0; NumObj2 < World.Count; NumObj2++)
                    {
                        for (int NumPol2 = 0; NumPol2 < World[NumObj2].Poligons.Count; NumPol2++)
                        {

                            if (NumObj == NumObj2 && NumPol == NumPol2)
                                continue;
                            else
                            {
                                double[] ABCD1 = Poligon.ABCDFound(World[NumObj].Poligons[NumPol], true);
                                double[] ABCD2 = Poligon.ABCDFound(World[NumObj2].Poligons[NumPol2], true);
                                Line linepersch = Line.LinePeresechPlosk(ABCD1, ABCD2);
                            }
                                CorectWorld[NumObj].Poligons.Add(World[NumObj2].Poligons[NumPol2]);

                        }
                    }
                }
            }/*
            for (int NumObj = 0; NumObj < World.Count; NumObj++)
            {
                CorectWorld.Add(new Object());
                for (int NumLine = 0; NumLine < World[NumObj].Lines.Count; NumLine++)
                {

                    for (int NumObj2 = 0; NumObj2 < World.Count; NumObj2++)
                    {
                        for (int NumPol2 = 0; NumPol2 < World.Count; NumPol2++)
                        {

                            if (NumObj == NumObj2 && NumPol == NumPol2)
                                //    break;
                                CorectWorld[NumObj].Poligons.Add(CorectWorld[NumObj2].Poligons[NumPol2]);

                        }
                    }
                }
            }*/
            return CorectWorld;
        }
    }
}
