using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{

    class Object
    {
        public MyList<Point3DF> Points = new MyList<Point3DF>();
        public MyList<Line> Lines = new MyList<Line>();
        public MyList<Poligon> Poligons = new MyList<Poligon>();
        public Object()
        {
            Poligons.OnAdd += new EventHandler(PoligonsAdd);
            Lines.OnAdd += new EventHandler(LinesAdd);
            Points.OnAdd += new EventHandler(PointsAdd);
        }
        public void PloskAdd(Point3DF P1, Point3DF P2, Point3DF P3, Point3DF P4, Color ColorPoligon, double Rozseiv, double ForseOtbit)
        {
            Poligons.Add(new Poligon(P1, P2, P3, true, true, false, ColorPoligon,Rozseiv,ForseOtbit));
            Poligons.Add(new Poligon(P3, P1, P4, false, true, true, ColorPoligon, Rozseiv, ForseOtbit));
        }
        public void PointsAdd(object sender, EventArgs e)
        {
        }
        private void LinesAdd(object sender, EventArgs e)
        {
            if (sender is Line)
            {
                Point3DF PointF;
                if (null == (PointF = PointFound(((Line)sender).StartLine)))
                {
                    Points.Add(((Line)sender).StartLine);
                    ((Line)sender).StartLine.Lines.Add((Line)sender);
                }
                else
                {
                    ((Line)sender).StartLine = PointF;
                    PointF.Lines.Add(((Line)sender));
                }
                if (null == (PointF = PointFound(((Line)sender).EndLine)))
                {
                    Points.Add(((Line)sender).EndLine);
                    ((Line)sender).EndLine.Lines.Add((Line)sender);
                }
                else
                {
                    ((Line)sender).EndLine = PointF;
                    PointF.Lines.Add(((Line)sender));
                }
            }
        }
        private void PoligonsAdd(object sender, EventArgs e)
        {
            if (sender is Poligon)
            {
                Line line1 = new Line(((Poligon)sender).P1, ((Poligon)sender).P2, false, ((Poligon)sender).B1);
                Line line2 = new Line(((Poligon)sender).P2, ((Poligon)sender).P3, false, ((Poligon)sender).B2);
                Line line3 = new Line(((Poligon)sender).P3, ((Poligon)sender).P1, false, ((Poligon)sender).B3);
                ((Poligon)sender).Lines.Add(line2);
                ((Poligon)sender).Lines.Add(line3);
                ((Poligon)sender).Lines.Add(line1);
                Line lineF;
                if (null == (lineF = LineFound(line1)))
                {
                    Lines.Add(line1);
                    line1.Poligons.Add(((Poligon)sender));
                }
                else
                {
                    ((Poligon)sender).Lines[2] = lineF;
                    lineF.Poligons.Add(((Poligon)sender));
                }
                if (null == (lineF = LineFound(line2)))
                {
                    Lines.Add(line2);
                    line2.Poligons.Add(((Poligon)sender));
                }
                else
                {
                    ((Poligon)sender).Lines[0] = lineF;
                    lineF.Poligons.Add(((Poligon)sender));
                }
                if (null == (lineF = LineFound(line3)))
                {
                    Lines.Add(line3);
                    line3.Poligons.Add(((Poligon)sender));
                }
                else
                {
                    ((Poligon)sender).Lines[1] = lineF;
                    lineF.Poligons.Add(((Poligon)sender));
                }
            }
        }
        public Point3DF PointFound(Point3DF point)
        {
            for (int i = 0; i < Points.Count; i++)
                if (true == Point3DF.EgualPoint(point, Points[i]))
                    return Points[i];
            return null;
        }
        public Line LineFound(Line line)
        {
            for (int i = 0; i < Lines.Count; i++)
                if (true == Line.EgualLine(line, Lines[i]))
                    return Lines[i];
            return null;
        }
        public static Object operator +(Double[] Mas, Object obj)
        {
            if (Mas.Length != 3)
                return null;
            Object RezultObject = new Object();
            for (int i = 0; i < obj.Poligons.Count; i++)
            {
                Poligon ChangePoligon = Mas + obj.Poligons[i];
                RezultObject.Poligons.Add(ChangePoligon);
            }
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                if (obj.Lines[i].OnlyLine)
                {
                    Line ChangeLine = Mas + obj.Lines[i];
                    RezultObject.Lines.Add(ChangeLine);
                }
            }
            for (int i = 0; i < obj.Points.Count; i++)
            {
                if (obj.Points[i].OnlyPoint)
                {
                    Point3DF ChangePoint = Mas + obj.Points[i];
                    RezultObject.Points.Add(ChangePoint);
                }
            }
            return RezultObject;
        }
        public static Object operator -(Object obj, Double[] Mas)
        {
            if (Mas.Length != 3)
                return null;
            Object RezultObject = new Object();
            for (int i = 0; i < obj.Poligons.Count; i++)
            {
                Poligon ChangePoligon = obj.Poligons[i] - Mas;
                RezultObject.Poligons.Add(ChangePoligon);
            }
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                if (obj.Lines[i].OnlyLine)
                {
                    Line ChangeLine = obj.Lines[i] - Mas;
                    RezultObject.Lines.Add(ChangeLine);
                }
            }
            for (int i = 0; i < obj.Points.Count; i++)
            {
                if (obj.Points[i].OnlyPoint)
                {
                    Point3DF ChangePoint = obj.Points[i] - Mas;
                    RezultObject.Points.Add(ChangePoint);
                }
            }
            return RezultObject;
        }
        public static Object operator *(Double[,] Mas, Object obj)
        {
            Object RezultObject = new Object();
            for (int i = 0; i < obj.Poligons.Count; i++)
            {
                Poligon ChangePoligon = Mas * obj.Poligons[i];
                RezultObject.Poligons.Add(ChangePoligon);
            }
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                if (obj.Lines[i].OnlyLine)
                {
                    Line ChangeLine = Mas * obj.Lines[i];
                    RezultObject.Lines.Add(ChangeLine);
                }
            }
            for (int i = 0; i < obj.Points.Count; i++)
            {
                if (obj.Points[i].OnlyPoint)
                {
                    Point3DF ChangePoint = Mas * obj.Points[i];
                    RezultObject.Points.Add(ChangePoint);
                }
            }
            return RezultObject;
        }
    }
}
