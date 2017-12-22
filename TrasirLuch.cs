using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace Yurchuk._2sem.laba._4
{
    class TrasirLuch
    {
        List<Object> World;
        double focus;
        double mash;
        double width;
        double height;
        byte[,,] ImageB;// new byte[3, height, width];
        Point3DF Sun;
        Color ColorSun;
        public TrasirLuch(List<Object> WorldCam, Point3DF Sun, Color ColorSun, double focus, double mashtab, int width, int height)
        {
            World = WorldCam;
            this.focus = focus;
            mash = mashtab;
            this.height = height;
            this.width = width;
            this.Sun = Sun;
            this.ColorSun = ColorSun;
            ImageB = new byte[3, height, width];
        }
        public Bitmap GetBitmap()
        {
            return ByteRgbQToBitmap(ImageB, (int)width, (int)height);
        }
        public void ImageBDraw(int M)
        {
            /*CudafyModule km = CudafyModule.TryDeserialize(typeof(Program).Name);
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy(typeof(Program));
                km.Serialize();
            }
            GPGPU gpu = Cudafy.Host.CudafyHost.GetDevice(CudafyModes.Target);
            gpu = CudafyHost.GetDevice(eGPUType.Cuda);
            gpu.LoadModule(km);
            gpu.Launch().ImageBDraw(new Kvadrat((int)0, 0,
                (int)width,
                (int)height));*/
            ImageBDraw(new Kvadrat((int)0 , 0,
                (int)width,
                (int)height ));
            /*
            for (int i =0;i<M;i++)
                for (int j =0;j<M;j++)
                {
                    Thread threaddraw = new Thread(ImageBDraw);
                    threaddraw.Start(new Kvadrat((int)(i * ((width ) / M)), (int)(j * ((height ) / M)),
                        (int)((i + 1) * ((width ) / M) ),
                        (int)((j + 1) * ((height ) / M) )));
                }
            Thread.Sleep(1200);*/
        }
        void ImageBDraw(object obj)
        {
            if (obj is Kvadrat)
            {
                Kvadrat kvadr = (Kvadrat)obj;
                for (int X = kvadr.Xstart;X<kvadr.Xend;X++)
                    for (int Y = kvadr.Ystart; Y < kvadr.Yend; Y++)
                    {
                        Color col = Luch(X, Y);
                        ImageB[0, Y, X] = col.R;
                        ImageB[1, Y, X] = col.G;
                        ImageB[2, Y, X] = col.B;
                    }

            }
        }
        public Color Luch(int wid, int heig)
        {
            Color rez = new Color();
            double wid1 = (wid - width / 2) / mash;
            double heig1 = -(heig - height / 2) / mash;
            Line luch = new Line(new Point3DF(0, 0, 0), new Point3DF(focus, wid1, heig1), true);
            object[] re = LuchPointF(luch, World);
            double Lambda = (double)re[0];
            int numobj = (int)re[1];
            int numpol = (int)re[2];
            if (Lambda < 0)
                return Color.White;
            else
            {
                Line luch2 = new Line(Line.PointF(luch, Lambda - 0.01), Sun,true);
                luch = new Line(new Point3DF(0,0,0),Line.PointF(luch, Lambda), true);
                if (!((double)LuchPointF(luch2, World)[0] < 0))
                    return Color.Black;
                else
                {
                    double len2 = Line.Len(luch2);
                    double len = Line.Len(luch);
                    Color colsunr = RozseivSrast(ColorSun, len);
                    Poligon pol = World[numobj].Poligons[numpol];


                    double[] ABCD = Poligon.ABCDFound(pol,true);
                    Point3DF Normal = Point3DF.NormirVector(new Point3DF(ABCD[0], ABCD[1], ABCD[2]));
                    Point3DF InSun = Point3DF.NormirVector(Line.Vector(luch2));
                    if (InSun.X * Normal.X + InSun.Y * Normal.Y + InSun.Z * Normal.Z < 0)
                        Normal = new Point3DF(-Normal.X,-Normal.Y, -Normal.Z);
                    Point3DF InCam = Point3DF.NormirVector(Line.Vector(Line.LineObr(luch)));

                    Point3DF Perpend = Line.VectorPerpend2Vector(InSun, Normal,true);
                    Point3DF p1 = new Point3DF(Perpend.X, Perpend.Y, 0);
                    if (!(p1.X == 0 && p1.Y == 0))
                    {
                        double AngleZ = Line.AngleFound(
                            new Line(new Point3DF(0, 0, 0), new Point3DF(1, 0, 0),true),
                            new Line(new Point3DF(0, 0, 0), p1, true));

                        double[,] RotateZ = new double[3, 3]
                            {{Math.Cos(AngleZ), Math.Sin(-AngleZ), 0 },
                            { Math.Sin(AngleZ), Math.Cos(AngleZ), 0 },
                            { 0,0,1} };
                        Point3DF test = RotateZ * p1;
                        if (!(Math.Round(test.Y,5) == 0 && test.X > 0))
                            AngleZ *= -1;
                        RotateZ = new double[3, 3]
                            {{Math.Cos(AngleZ), Math.Sin(-AngleZ), 0 },
                            { Math.Sin(AngleZ), Math.Cos(AngleZ), 0 },
                            { 0,0,1} };
                        test = RotateZ * p1;
                        Normal = RotateZ * Normal;
                        InSun = RotateZ * InSun;
                        InCam = RotateZ * InCam;
                        Perpend = RotateZ * Perpend;
                    }
                    double AngleY = Line.AngleFound(
                        new Line(new Point3DF(0, 0, 0), new Point3DF(10, 0, 0), true),
                        new Line(new Point3DF(0, 0, 0), Perpend, true));
                    double[,] RotateY = new double[3, 3]
                            {{Math.Cos(AngleY),0, Math.Sin(-AngleY)},
                            { 0,1, 0 },
                            { Math.Sin(AngleY),0, Math.Cos(AngleY)} };
                    Point3DF p = RotateY * Perpend;
                    if (!(Math.Round(p.Z,4) == 0 && p.X > 0))
                        AngleY *= -1;
                    RotateY = new double[3, 3]
                            {{Math.Cos(AngleY),0, Math.Sin(-AngleY)},
                            { 0,1, 0 },
                            { Math.Sin(AngleY),0, Math.Cos(AngleY)} };
                    p = RotateY * Perpend;

                    Normal = RotateY * Normal;
                    InSun = RotateY * InSun;
                    InCam = RotateY * InCam;
                    Perpend = RotateY * Perpend;

                    double AngleNS = -(InSun.X * Normal.X + InSun.Y * Normal.Y + InSun.Z * Normal.Z);
                    double[,] RotateX = new double[3, 3]
                            {{1, 0, 0},
                            { 0, AngleNS, -Math.Sqrt(1 - Math.Pow(AngleNS,2))},
                            { 0, Math.Sqrt(1 - Math.Pow(AngleNS, 2)), AngleNS} };
                    Point3DF OtragL = RotateX * Normal;//new Point3DF(InSun.X, InSun.Y, InSun.Z);
                    double CosAngleNS2 = InSun.X * OtragL.X + InSun.Y * OtragL.Y + InSun.Z * OtragL.Z;
                    if (Math.Abs(Math.Round(CosAngleNS2, 4)) == 1)
                    {
                        RotateX = new double[3, 3]
                                {{1, 0, 0},
                            { 0, AngleNS, Math.Sqrt(1 - Math.Pow(AngleNS,2))},
                            { 0, -Math.Sqrt(1 - Math.Pow(AngleNS, 2)), AngleNS} };
                        OtragL = RotateX * Normal;
                    }
                    double CosAngleNS = Math.Abs(InSun.X * Normal.X + InSun.Y * Normal.Y + InSun.Z * Normal.Z);
                    CosAngleNS2 = InSun.X * OtragL.X + InSun.Y * OtragL.Y + InSun.Z * OtragL.Z;
                    double CosAngleOC = Math.Max(0,-InCam.X * OtragL.X - InCam.Y * OtragL.Y - InCam.Z * OtragL.Z);
                    /*if (AngleOC < 0)
                        return Color.Black;*/
                    double[] K = new double[3] { (double)pol.ColorPoligon.R / 255, (double)pol.ColorPoligon.G / 255,(double)pol.ColorPoligon.B / 255 };

                    double[] Ks = new double[3] {
                        (double)((255 - pol.ColorPoligon.R )*1 + pol.ColorPoligon.R )/ 255,
                        (double)((255 - pol.ColorPoligon.G )*1 + pol.ColorPoligon.G )/ 255,
                        (double)((255 - pol.ColorPoligon.B )*1 + pol.ColorPoligon.B )/ 255 };

                    double[] Ia = new double[3] {K[0]* colsunr.R , K[1] * colsunr.G, K[2] * colsunr.B};
                    double[] Id = new double[3] { K[0] * CosAngleNS * colsunr.R, K[1] * CosAngleNS * colsunr.G, K[2] * CosAngleNS * colsunr.B };
                    double[] Is = new double[3] {
                        Ks[0] * Math.Pow(CosAngleOC, 5) * colsunr.R,
                        Ks[1] * Math.Pow(CosAngleOC, 5) * colsunr.G,
                        Ks[2] * Math.Pow(CosAngleOC, 5) * colsunr.B };
                    return RozseivSrast(Color.FromArgb(
                        (int)((Ia[0] + Id[0] + Is[0])) / (3),
                        (int)((Ia[1] + Id[1] + Is[1])) / (3),
                        (int)((Ia[2] + Id[2] + Is[2])) / (3))
                        ,len2);
                }
            }
            return rez;
        }
        static public Color RozseivSrast(Color col,double len)
        {
            int  P = 200;
            int R = col.R - (int)(len / P);
            int B = col.B - (int)(len / P);
            int G = col.G - (int)(len / P);
            if (R < 0)
                R = 0;
            if (G < 0)
                G = 0;
            if (B < 0)
                B = 0;
            return Color.FromArgb(R, G, B);
        }
        static public object[] LuchPointF(Line luch,List<Object> World)
        {
            double Lambda = -1;
            bool First = true;
            int numobjr = 0;
            int numpolr = 0;
            for (int numobj = 0; numobj < World.Count; numobj++)
                for (int numpol = 0; numpol < World[numobj].Poligons.Count; numpol++)
                {
                    double[] ABCD = Poligon.ABCDFound(World[numobj].Poligons[numpol], true);
                    double L = Line.PeresechPoint(ABCD, luch);
                    if (L > 0 && Poligon.InArea(World[numobj].Poligons[numpol], Line.PointF(luch, L)))
                    {
                        if (First)
                        {
                            First = false;
                            numobjr = numobj;
                            numpolr = numpol;
                            Lambda = L;
                        }
                        else if (L > 0)
                        {
                            if (Lambda >= L)
                            {
                                numobjr = numobj;
                                numpolr = numpol;
                            }
                            Lambda = Math.Min(Lambda, L);
                        }
                    }

                }
            return new object[3] { Lambda,numobjr,numpolr };
        }
        public unsafe static byte[,,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (byte* _res = res)
                {
                    byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++); ++_b;
                            *_g = *(curpos++); ++_g;
                            *_r = *(curpos++); ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }
        public unsafe static Bitmap ByteRgbQToBitmap(byte[,,] arr, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            byte* curpos;
            fixed (byte* _res = arr)
            {
                byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                for (int h = 0; h < height; h++)
                {
                    byte* ptr = (byte*)bd.Scan0;
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++, _b++, _g++, _r++)
                    {
                        *(curpos++) = *_b;
                        *(curpos++) = *_g;
                        *(curpos++) = *_r;
                        //bmp.SetPixel(w, h, Color.FromArgb(*_r, *_g, *_b));
                    }
                }
            }

            bmp.UnlockBits(bd);
            return bmp;
        }
    }
    class Kvadrat
    {
        public int Xstart;
        public int Ystart;
        public int Xend;
        public int Yend;
        public Kvadrat(int Xstart, int Ystart, int Xend, int Yend)
        {
            this.Xstart = Xstart;
            this.Ystart = Ystart;
            this.Xend = Xend;
            this.Yend = Yend;
        }
    }
}
