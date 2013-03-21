using System;
using System.Collections.Generic;
using System.Text;


namespace The_Famous_Perspective_Cube
{
    static class Dmatrix
    {

        public static double[,] IdentityMatrix()
        {
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

        public static double[,] TranslationMatrix(double tx, double ty, double tz)
        {
            return new double[4, 4] { { 1, 0, 0, tx }, { 0, 1, 0, ty }, { 0, 0, 1, tz }, { 0, 0, 0, 1 } };
        }

        public static double[,] ScalingMatrix(double sx, double sy, double sz)
        {
            return new double[4, 4] { { sx, 0, 0, 0 }, { 0, sy, 0, 0 }, { 0, 0, sz, 0 }, { 0, 0, 0, 1 } };
        }

        public static double[,] PerspectiveMatrix(double f)
        {
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, (1 / f), 0 } };
        }

        public static double[,] RotateXmatrix(double theta)
        {
            double raidians = ToRadians(theta);
            double cos = Math.Cos(raidians);
            double sin = Math.Sin(raidians);
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, cos, sin, 0 }, { 0, -sin, cos, 0 }, { 0, 0, 0, 1 } };
        }

        public static double[,] RotateYmatrix(double theta)
        {
            double raidians = ToRadians(theta);
            double cos = Math.Cos(raidians);
            double sin = Math.Sin(raidians);
            //return new double[4, 4] { { cos, 0, sin }, { 0, 1, 0 }, { -sin, 0, cos }, { 0, 0, 0, 1 } };
            return new double[4, 4] { { cos, 0, -sin, 0 }, { 0, 1, 0, 0 }, { sin, 0, cos,0 },{ 0, 0, 0, 1 } };
        }

        public static double[,] RotateZmatrix(double theta)
        {
            double raidians = ToRadians(theta);
            double cos = Math.Cos(raidians);
            double sin = Math.Sin(raidians);
            return new double[4, 4] { { cos, sin, 0, 0 }, { -sin, cos, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

        public static double[,] multiply(double[,] MatrixA, double[,] MatrixB)
        {
            double[,] result = new double[0, 0];

            if (MatrixA.GetLength(1) == MatrixB.GetLength(0))
            {
                result = new double[MatrixA.GetLength(0), MatrixB.GetLength(1)];
                for (int i = 0; i < MatrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < MatrixA.GetLength(1); j++)
                    {
                        MatrixA[i, j] = 0;
                        for (int k = 0; k < MatrixA.GetLength(1); k++)
                            result[i, j] = MatrixA[i, j] + MatrixA[i, k] * MatrixB[k, j];
                    }
                }
            }
            return result;

        }

        private static double ToRadians(double r)
        {
            return (r * (Math.PI / 180));
        }
    }
}
