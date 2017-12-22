using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{
    static class HideLineCusk
    {
        static public List<Line> CuskLineHide( List<Object> World)
        {
            List<Line> lines = new List<Line>();
            for (int numobj = 0;numobj<World.Count;numobj++)
            {
                for (int numline = 0;numline < World[numobj].Lines.Count;numline++)
                {
                    List<Line> lines1 = CuskLineHide(World[numobj].Lines[numline], World);
                    for (int i = 0; i < lines1.Count; i++)
                        lines.Add(lines1[i]);
                }
            }
            return lines;
        }
        static public List<Line> CuskLineHide(Line line, List<Object> World)
        {
            List<double> Lambda = CuskLine(line, World);
            List<Line> lines = new List<Line>();
            for (int i =0;i< Lambda.Count - 1; i++)
            {
                lines.Add(new Line(Line.PointF(line, Lambda[i]), Line.PointF(line, Lambda[i + 1]),line.ColLine, line.OnlyLine,line.Visible));
            }
            for (int i = 0; i < Lambda.Count - 1; i++)
            {
                if(lines[i].Visible)
                    lines[i].Visible = ConvertCoordinate.LineHide(lines[i], World);
            }
            List<Line> linesCor = new List<Line>();
            linesCor.Add(lines[0]);
            for (int i = 1; i < lines.Count; i++)
            {
                if (linesCor[linesCor.Count - 1].Visible == lines[i].Visible)
                    linesCor[linesCor.Count - 1].EndLine = lines[i].EndLine;
                else
                    linesCor.Add(lines[i]);
            }

            return linesCor;
        }
        static public List<double> CuskLine(Line line,List<Object> World)
        {
            List<double> Lambda = new List<double>();
            for (int numworld = 0;numworld< World.Count;numworld++)
            {
                for (int i = 0; i < World[numworld].Poligons.Count; i++)
                {
                    List<double> Lam1 = CuskLine(line, World[numworld].Poligons[i]);
                    if (Lam1 != null)
                        for (int j = 0; j < Lam1.Count; j++)
                            if(Lam1[j]<1|| Lam1[j] >0)
                                Lambda.Add(Lam1[j]);
                }     
            }
            Lambda.Sort(delegate (double l1, double l2)
            {
                if (double.IsNaN(l1))
                    return -1;
                if (double.IsNaN(l2))
                    return 1;
                return l1.CompareTo(l2);
            });
            List<double> LambdaCor = new List<double>();
            LambdaCor.Add(0);
            for (int i = 0;i<Lambda.Count;i++)
            {
                if (double.IsNaN(Lambda[i]))
                    continue;
                if (LambdaCor.Count != 0 && 
                    Math.Round(LambdaCor[LambdaCor.Count - 1],6) == Math.Round(Lambda[i],6))
                    continue;
                if (Math.Round(Lambda[i],10) < 0 || Math.Round(Lambda[i], 10) > 1)
                    continue;
                LambdaCor.Add(Lambda[i]);
            }
            LambdaCor.Add(1);
            return LambdaCor;
        }
        static public List<double> CuskLine(Line line, Poligon pol)
        {
            List<Point3DF> Point = new List<Point3DF>();
            List<double> Lambda = new List<double>();
            double[] ABCD = Poligon.ABCDFound(pol, true);
            double l1 = Poligon.PointCheck(ABCD, new Point3DF(0, 0, 0));
            double l2 = Poligon.PointCheck(ABCD, line.StartLine);
            if ((l1 > 0 && l2 > 0) || (l1 < 0 && l2 < 0))
            {
                return null;
            }
            for (int i =0;i<3;i++)
            {
                Poligon poligcam = new Poligon(pol.Points[i % 3], pol.Points[(i + 1) % 3], new Point3DF(0, 0, 0));
                Lambda.Add(Line.PeresechPoint(Poligon.ABCDFound(poligcam, true), line));
            }/*
            Lambda.Sort(delegate (double d1, double d2)
            {
                if (double.IsNaN(d1))
                    return -1;
                if (double.IsNaN(d2))
                    return 1;
                return d1.CompareTo(d2);
            });*/
            return Lambda;
        }
        
    }
}
