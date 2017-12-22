using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yurchuk._2sem.laba._4
{
    public partial class Form1 : Form
    {
        

        Bitmap DrawArea;
        Graphics g;
        double mash;
        Point3DF LocationCam = new Point3DF(200, 200, 200);
        Point3DF Sun = new Point3DF(40, 80, 200,true);
        Color ColSun = Color.FromArgb(255, 255, 255);
        Point3DF VzgladCam = Point3DF.NormirVector(new Point3DF(-1, -1, 1));
        double Angle = 0;
        double Step = 5;
        double focus = 5;
        List<Object> World = new List<Object>();
        Object Pol = new Object();
        Object Pol2 = new Object();
        Object OsCoord = new Object();
        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            g = Graphics.FromImage(DrawArea);
            mash = 75;
            ObjSet();
            //Pol.Points.Sort();
        }
        public void ObjSet()
        {
            Point3DF p1 = new Point3DF(0, 0, 10);
            Point3DF p2 = new Point3DF(0, 50, 10);
            Point3DF p3 = new Point3DF(50, 50, 10);
            Point3DF p4 = new Point3DF(50, 0, 10);

            Point3DF p5 = new Point3DF(60, 0-20, 30);
            Point3DF p6 = new Point3DF(60, 50 - 20, 30);
            Point3DF p7 = new Point3DF(110, 50 - 20, 30);
            Point3DF p8 = new Point3DF(110, 0 - 20, 30);
            double P1 = 40;
            double P2 = 45;
            double P3 = 0;
            p1 = new Point3DF(0 + P1, 0 + P2, 10 + P3);
            p2 = new Point3DF(0 + P1, 50 + P2, 10 + P3);
            p3 = new Point3DF(50 + P1, 50 + P2, 10 + P3);
            p4 = new Point3DF(50 + P1, 0 + P2, 10 + P3);

            p5 = new Point3DF(60 + P1, 0 - 20 + P2, 30 + P3);
            p6 = new Point3DF(60 + P1, 50 - 20 + P2, 30 + P3);
            p7 = new Point3DF(110 + P1, 50 - 20 + P2, 30 + P3);
            p8 = new Point3DF(110 + P1, 0 - 20 + P2, 30 + P3);
            Pol = new Object();
            //Pol.Poligons.OnAdd += new EventHandler(Object);
            Pol.PloskAdd(p1,p2,p3,p4, Color.Gray, 0, 1);
            Pol.PloskAdd(p5,p6,p7,p8, Color.Gray, 0, 1);

            Pol.PloskAdd(p2,p3,p7,p6, Color.Gray, 0, 1);
            Pol.PloskAdd(p3,p4, p8,p7, Color.Gray, 0, 1);
            Pol.PloskAdd(p4,p1,p5,p8, Color.Gray, 0, 1);
            Pol.PloskAdd(p1, p2,p6,p5, Color.Gray, 0, 1);

            OsCoord = new Object();
            OsCoord.Lines.Add(new Line(new Point3DF(0, 0, 0), new Point3DF(100, 0, 0), Color.Red));
            OsCoord.Lines.Add(new Line(new Point3DF(0, 0, 0), new Point3DF(0, 100, 0), Color.Green));
            OsCoord.Lines.Add(new Line(new Point3DF(0, 0, 0), new Point3DF(0, 0, 100), Color.Blue));
            OsCoord.PloskAdd(new Point3DF(0, 0, 0),
                new Point3DF(200, 0, 0),
                new Point3DF(200, 200, 0),
                new Point3DF(0, 200, 0),Color.Red, 0, 1);
            OsCoord.PloskAdd(new Point3DF(0, 0, 0),
                new Point3DF(200, 0, 0),
                new Point3DF(200, 0, 200),
                new Point3DF(0, 0, 200),Color.Green, 0, 1);
            OsCoord.PloskAdd(new Point3DF(0, 0, 0),
                new Point3DF(0, 200, 0),
                new Point3DF(0, 200, 200),
                new Point3DF(0, 0, 200),Color.Blue,0,1);
            World.Add(OsCoord);
            World.Add(Pol);
            //World.Add(Pol2);
        }
        internal void RefreshPictureBox(Point3DF LocationCam, Point3DF VzgladCam,List<Object> Wolrd)
        {
            List<Object> WorldCam = new List<Object>();
            List<Object> WorldHide = new List<Object>();
            List<List<Line>> WorldConvertTo2D = new List<List<Line>>();
            g.Clear(Color.White);
            //List<Object> Corworld = ConvertCoordinate.CorectObj(World);
            for (int i =0;i<Wolrd.Count;i++)
            {
                WorldCam.Add(ConvertCoordinate.ConvertWorldtoCam(VzgladCam, LocationCam, World[i]));
            }
            //List<Object> CorworldCam = ConvertCoordinate.CorectObj(WorldCam);
            List<Line> lines =  HideLineCusk.CuskLineHide( WorldCam);
            List<Line> lines2D = ConvertCoordinate.Camto2D(lines, 5);
            drawing(lines2D, mash);/*
            for (int i = 0; i < Wolrd.Count; i++)
                WorldHide.Add(ConvertCoordinate.ObjectHideLine(WorldCam[i], WorldCam));
            for (int i = 0; i < Wolrd.Count; i++)
                WorldConvertTo2D.Add(ConvertCoordinate.Camto2D(WorldHide[i], 5));
            for (int i = 0; i < Wolrd.Count; i++)
                drawing(WorldConvertTo2D[i], mash);*/
            pictureBox1.Image = DrawArea;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double Z = 100;
            /*if (Angle > 4 * Math.PI)
            {
                Angle = 0;
                timer1.Stop();
            }
            else*/
            {
                Angle += Math.PI / 500;
                Point3DF vectVision = new Point3DF(-Z * Math.Cos(Angle), -Z * Math.Sin(Angle), 30);
                Point3DF vectLocation = new Point3DF(50+Z * Math.Cos(Angle), 25+Z * Math.Sin(Angle), -30);

                RefreshPictureBox(vectLocation, vectVision,World);/*
                Object obj = ConvertCoordinate.ConvertWorldtoCam(vectVision, vectLocation, Pol);
                Object objOs = ConvertCoordinate.ConvertWorldtoCam(vectVision, vectLocation, OsCoord);
                g.Clear(Color.White);
                drawing(ConvertCoordinate.Camto2D(obj,5), mash);
                drawing(ConvertCoordinate.Camto2D(objOs,5), mash);
                pictureBox1.Image = DrawArea;*/
            }

        }
        private void drawing(List<Line> Lines, double mash)
        {


            List<Line> LinesDrw = new List<Line>();
            for (int i = 0; i < Lines.Count; i++)
            {
                LinesDrw.Add(new Line(
                    new Point3DF((float)(DrawArea.Width / 2 + Lines[i].StartLine.X * mash),
                        (float)(DrawArea.Height * 0.5 - Lines[i].StartLine.Y * mash), 0),
                    new Point3DF((float)(DrawArea.Width / 2 + Lines[i].EndLine.X * mash),
                        (float)(DrawArea.Height * 0.5 - Lines[i].EndLine.Y * mash), 0),
                    Lines[i].ColLine,
                    Lines[i].OnlyLine,
                    Lines[i].Visible));
            }
            for (int i = 0; i < LinesDrw.Count; i++)
                if (LinesDrw[i].Visible)
                    g.DrawLine(new Pen(new SolidBrush(LinesDrw[i].ColLine)),
                        (float)LinesDrw[i].StartLine.X,
                        (float)LinesDrw[i].StartLine.Y,
                        (float)LinesDrw[i].EndLine.X,
                        (float)LinesDrw[i].EndLine.Y);
        }

        private void pictureBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Keys.W == e.KeyData)
            {
                LocationCam += VzgladCam * Step;
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
            else if (Keys.S == e.KeyData)
            {
                LocationCam += VzgladCam * (-Step);
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
            else if(Keys.A == e.KeyData)
            {
                LocationCam += new Point3DF(-VzgladCam.Y, VzgladCam.X,0,true) * (Step);
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
            else if(Keys.D == e.KeyData)
            {
                LocationCam += new Point3DF(VzgladCam.Y, -VzgladCam.X, 0, true) * (Step);
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
            else if((Keys.ShiftKey | Keys.Shift) == e.KeyData)
            {
                LocationCam += new Point3DF(0,0,Step);
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
            else if((Keys.ControlKey | Keys.Control)== e.KeyData)
            {
                LocationCam += new Point3DF(0, 0, -Step);
                VzgladCam = Point3DF.NormirVector(new Point3DF(-LocationCam.X, -LocationCam.Y, -LocationCam.Z));
                RefreshPictureBox(LocationCam, VzgladCam, World);
            }
        }

        private void вращениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled)
                timer1.Stop();
            else
            {
                timer1.Interval = 10;
                timer1.Start();
            }
            this.pictureBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Focus();
            RefreshPictureBox(LocationCam, VzgladCam, World);
        }

        private void закраситьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<Object> WorldCam = new List<Object>();
            for (int i = 0; i < World.Count; i++)
            {
                WorldCam.Add(ConvertCoordinate.ConvertWorldtoCam(VzgladCam, LocationCam, World[i]));
            }
            Object SunO = new Object();
            SunO.Points.Add(Sun);
            Object SunOC = ConvertCoordinate.ConvertWorldtoCam(VzgladCam, LocationCam, SunO);
            TrasirLuch TRrLuch = new TrasirLuch(WorldCam,SunOC.Points[0], ColSun, focus,mash,pictureBox1.Width,pictureBox1.Height);
            TRrLuch.ImageBDraw(35);
            pictureBox1.Image = TRrLuch.GetBitmap();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            List<Object> WorldCam = new List<Object>();
            for (int i = 0; i < World.Count; i++)
            {
                WorldCam.Add(ConvertCoordinate.ConvertWorldtoCam(VzgladCam, LocationCam, World[i]));
            }
            Object SunO = new Object();
            SunO.Points.Add(Sun);
            Object SunOC = ConvertCoordinate.ConvertWorldtoCam(VzgladCam, LocationCam, SunO);
            TrasirLuch TRrLuch = new TrasirLuch(WorldCam, SunOC.Points[0], ColSun, focus, mash, pictureBox1.Width, pictureBox1.Height);

            TRrLuch.Luch(e.X, e.Y);
        }
    }
}
