//********************************************************************//
//                                                                    //
// Class to implement a simple homogeneous coordinate.                //
//                                                                    //
//                                                                    //
// This code and the class structures were written by me (Nigel       //
// Barlow)(nigel@soc.plymouth.ac.uk).   You may use this code for any //
// educational or non-profit making purpose, so long as you leave my  //
// name on it.  And please cirrext some of the typinf and spellig     //                                                       
// errirs.    Nigel Barlow <nigel@soc.plymouth.ac.uk>                 //
//                                                                    //
//********************************************************************//

using System;
using System.Collections.Generic;
using System.Text;

namespace The_Famous_Perspective_Cube
{
    class Npoint
    {
        public double[] point = new double[4];
        public String pointName = "Point";

        public Npoint()
        {
            for (int i = 0; i < 3; i++) point[i] = 0; point[3] = 1;
        }

        public Npoint(double x, double y, double z)
        {
            point[0] = x; point[1] = y; point[2] = z; point[3] = 1;
        }

        public Npoint(String name)
        {
            for (int i = 0; i < 3; i++) point[i] = 0; point[3] = 1;
            pointName = name;
        }

        public Npoint(double x, double y, double z, String name)
        {
            point[0] = x; point[1] = y; point[2] = z; point[3] = 1;
            pointName = name;
        }

        public Npoint transform(double[,] matrix)
        {
            double total;
            Npoint newPoint = new Npoint(pointName);

            for (int col = 0; col < 4; col++)
            {
                total = 0;
                for (int row = 0; row < 4; row++)
                    total += point[row] * matrix[col, row];

                newPoint.point[col] = total;
            }

            return newPoint;
        }

        public Npoint rescale()
        {
            Npoint newPoint = new Npoint(0, 0, 0);
            for (int i = 0; i < 4; i++)
                newPoint.point[i] = point[i] / point[3];

            return newPoint;
        }

        public override String ToString()
        {
            String s = pointName + "\t";

            for (int i = 0; i < 4; i++)
            {
                s += Convert.ToString(point[i] + "\t");
            }

            return s;
        }

    }
}
