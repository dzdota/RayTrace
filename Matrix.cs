using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLib;

namespace testgistogr
{
    class Matrix
    {

        static public double Determinant(double[,] a)
        {
            
            //double[,] S0 = S0ij(Data[0], Data[1]);
            var matrix1 = new MatrixLib.Matrix(a);
            double det1 = matrix1.Determinant;
                return det1;
            /*if (a.GetLength(0) != a.GetLength(1))
                return 0;
            double rez = 0;
            if (a.Length == 1)
                return a[0, 0];
            for (int i1 = 0; i1 < a.GetLength(1); i1++)
                rez += Math.Pow(-1, i1) * a[0, i1] * Determinant(SubMatrix(a, 0, i1));
            return rez;*/
        }
        /*
        static public double Determinant(double[,] a)
        {
            int n = a.GetLength(0);
            int i, j, k;
            double det = 0;
            for (i = 0; i < n - 1; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    det = a[j, i] / a[i, i];
                    for (k = i; k < n; k++)
                        a[j, k] = a[j, k] - det * a[i, k]; // Here's exception
                }
            }
            det = 1;
            for (i = 0; i < n; i++)
                det = det * a[i, i];
            return det;
        }*/
        static public double[,] SubMatrix(double[,] A, int row, int col)
        {
            double[,] rez = new double[A.GetLength(0)-1, A.GetLength(1)-1];
            for (int i1 = 0, i2 = 0; i1 < A.GetLength(0); i1++,i2++)
            {
                if (i1 == row)
                {
                    i2--;
                    continue;
                }
                for (int i3 = 0, i4 = 0; i3 < A.GetLength(1); i3++)
                {
                    if ( i3 != col)
                    {
                        rez[i2, i4++] = A[i1, i3];
                    }
                }
            }
            return rez;
        }
        static public double[,] SwapRows(double[,] A, int row1, int row2)
        {
            double rez;
            double.TryParse("1+2", out rez);
            double[,] Arez = new double[A.GetLength(0), A.GetLength(1)];
            for (int row = 0; row < A.GetLength(0); row++)
                for (int col = 0; col < A.GetLength(1); col++)
                    Arez[row, col] = A[row, col];
            for (int col = 0; col < A.GetLength(1); col++)
            {
                double swap = Arez[row1,col];
                Arez[row1, col] = Arez[row2, col];
                Arez[row2, col] = swap;
            }
            return Arez;
        }
        static public double[,] MultiplicMatrix(double[,] A, double[,] B)
        {
            double[,] rez = new double[A.GetLength(0), B.GetLength(1)];
            for (int i1 = 0; i1 < A.GetLength(0); i1++)
            {
                for (int i2 = 0; i2 < B.GetLength(1); i2++)
                {
                    double hlpdoubl = 0;
                    for (int i3 = 0; i3 < A.GetLength(1); i3++)
                        hlpdoubl += A[i1, i3] * B[i3, i2];
                    rez[i1, i2] = hlpdoubl;
                }
            }
            return rez;
        }
        static public double[,] TranspMatrix(double[,] A)
        {
            double[,] rez = new double[A.GetLength(1), A.GetLength(0)];
            for (int i1 = 0; i1 < A.GetLength(1); i1++)
                for (int i2 = 0; i2 < A.GetLength(0); i2++)
                    rez[i1, i2] = A[i2, i1];
            return rez;
        }
        static public double[,] InverseMatrix(double[,] A) 
        {
            double[,] M = new double[A.GetLength(0), A.GetLength(1)];
            for (int col = 0; col < A.GetLength(1);col++ ) 
                for (int row = 0; row < A.GetLength(0); row++) 
                    M[row, col] = Math.Pow(-1,row + col) * Determinant(SubMatrix(A, row, col));
            M = TranspMatrix(M);
            double det = Determinant(A);
            M = MultiplicNumber(M, 1 / det);
            return M;
        }

        static public double[,] MultiplicNumber(double[,] A, double Num)
        {
            double[,] rez = new double[A.GetLength(0), A.GetLength(1)];
            for (int i1 = 0; i1 < A.GetLength(0); i1++)
            {
                for (int i2 = 0; i2 < A.GetLength(1); i2++)
                {
                    rez[i1, i2] = A[i1,i2]*Num;
                }
            }
            return rez;
        }
        
        static public double[,] Substraction(double[,] A, double[,] B)
        {

            double[,] rez = new double[A.GetLength(0), A.GetLength(1)];
            /*if (B.GetLength(0) == 1)
                for (int i1 = 0; i1 < A.GetLength(0); i1++)
                {
                    for (int i2 = 0; i2 < A.GetLength(1); i2++)
                    {
                        rez[i1, i2] = A[i1, i2] - B[0, i2];
                    }
                }*/
            if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
                return new double[1,1]{{0}};
            for (int i1 = 0; i1 < A.GetLength(0); i1++)
            {
                for (int i2 = 0; i2 < A.GetLength(1); i2++)
                {
                    rez[i1, i2] = A[i1,i2]-B[i1,i2];
                }
            }
            return rez;
        }

        static public double[,] Addition(double[,] A, double[,] B)
        {
            if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
                return new double[1, 1] { { 0 } };
            double[,] rez = new double[A.GetLength(0), A.GetLength(1)];
            for (int i1 = 0; i1 < A.GetLength(0); i1++)
            {
                for (int i2 = 0; i2 < A.GetLength(1); i2++)
                {
                    rez[i1, i2] = A[i1, i2] + B[i1, i2];
                }
            }
            return rez;
        }

    }
}
