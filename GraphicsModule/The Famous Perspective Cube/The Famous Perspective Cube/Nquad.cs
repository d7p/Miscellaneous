//********************************************************************//
//                                                                    //
// Class to implement a single quad - a 4 sides polygon.   My brain   //
// works better with quads than triangles, though most 3D APIs draw   //
// triangles..                                                        //
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
using System.Drawing;
using System.Media;
using System.Drawing.Drawing2D;


namespace The_Famous_Perspective_Cube
{
    class Nquad
    {
        public Npoint[] points = new Npoint[4];
        public String quadNameName = "Quad";

        public Nquad()
            {
            for (int i = 0; i < 4; i++) points[i] = new Npoint(0, 0, 0);
            }

        public Nquad(Npoint v0, Npoint v1, Npoint v2, Npoint v3)
                {
                points[0] = v0;
                points[1] = v1;
                points[2] = v2;
                points[3] = v3;
                }

        public Nquad transform(double[,] matrix)
        {
            Nquad transformedQuad = new Nquad();

            for (int i = 0; i < 4; i++)
            {
                transformedQuad.points[i] = points[i].transform(matrix);
                transformedQuad.points[i] = transformedQuad.points[i].rescale();
            }
            return transformedQuad;
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);

            for (int i = 0; i < 3; i++)     // Draw the three lines as a loop.
                g.DrawLine(pen, 
                    (int) points[i].point[0],   (int) points[i].point[1],
                    (int) points[i+1].point[0], (int) points[i+1].point[1]);

                //And back to the first point for the last line.
                g.DrawLine(pen,
                    (int)points[3].point[0], (int)points[3].point[1],
                    (int)points[0].point[0], (int)points[0].point[1]);
            
        }

        public void drawAsFilledPolygon(Graphics g)
        {
            Color brushColour = Color.FromArgb(255, 0, 0); //Red, but pass as a parameter.
            SolidBrush brush = new SolidBrush(brushColour);

            //Construct an array of .net points to draw.
            Point[] pointsarray = new Point[]{
            new Point ( (int)points[0].point[0], (int)points[0].point[1])
            ,new Point ( (int)points[1].point[0], (int)points[1].point[1])
            ,new Point ( (int)points[2].point[0], (int)points[2].point[1])
            ,new Point ( (int)points[3].point[0], (int)points[3].point[1])
            };
            byte[] bite = new byte[4]{1,1,1,1};
            GraphicsPath netPoints = new GraphicsPath(pointsarray,bite);
            
            g.FillPath(brush, netPoints);
        }

    }
}
