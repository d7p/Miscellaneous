using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace The_Famous_Perspective_Cube
{
    [Serializable()]
    public class point : ISerializable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public point()
        {
            this.X = new float();
            this.Y = new float();
            this.Z = new float();
            this.W = new float();
        }

        public point(SerializationInfo info, StreamingContext ctxt)
        {
            this.X = (float)info.GetValue("X", typeof(float));
            this.Y = (float)info.GetValue("Y", typeof(float));
            this.Z = (float)info.GetValue("Z", typeof(float));
            this.W = (float)info.GetValue("W", typeof(float));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("X", this.X);
            info.AddValue("Y", this.Y);
            info.AddValue("Z", this.Z);
            info.AddValue("W", this.W);
        }
    }


    public struct Triangle
    {
        public int Number;
        public int FirstPoint;
        public int SecondPoint;
        public int ThirdPoint;
    }

    public class CustomMatrix
    {
        private List<point> Points { get; set; }
        private List<point> verginPoints { get; set; }
        private List<Triangle> Triangles { get; set; }
        public Color lineColor { get; set; }
        public int penThickness { get; set; }
        public Color fillColor { get; set; }

        public CustomMatrix()
        {
            Points = new List<point>();
            verginPoints = new List<point>();
            Triangles = new List<Triangle>();
            lineColor = new Color();
            penThickness = new int();
        }

        public bool LoadMatrix(string fileName)
        {
            verginPoints = new List<point>();
            try
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    string[] line;
                    int totalVertices = Convert.ToInt32(file.ReadLine());
                    for (int currentLineNum = totalVertices; currentLineNum > 0; currentLineNum--)
                    {
                        line = file.ReadLine().Trim().Split(' ');
                        verginPoints.Add(new point
                        {
                            X = (float)Convert.ToDouble(line[0]),
                            Y = (float)Convert.ToDouble(line[1]),
                            Z = (float)Convert.ToDouble(line[2]),
                            W = 1
                        });
                    }
                    int totalTriangles = Convert.ToInt32(file.ReadLine());
                    for (int currenrtNum = totalTriangles; currenrtNum > 0; currenrtNum--)
                    {
                        line = file.ReadLine().Trim().Split(' ');

                        Triangles.Add(new Triangle
                        {
                            Number = Convert.ToInt32(line[0]),
                            FirstPoint = Convert.ToInt32(line[1]),
                            SecondPoint = Convert.ToInt32(line[2]),
                            ThirdPoint = Convert.ToInt32(line[3])
                        });
                    }
                }

            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (FileLoadException e)
            {
                throw e;
            }

            return true;
        }

        public void Transform(double[,] matrix)
        {
            if (matrix.GetLength(1) == 4)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    point p = new point();

                    p.X = (float)((Points[i].X * matrix[0, 0])
                        + (Points[i].Y * matrix[0, 1])
                        + (Points[i].Z * matrix[0, 2])
                        + (Points[i].W * matrix[0, 3]));


                    p.Y = (float)((Points[i].X * matrix[1, 0])
                         + (Points[i].Y * matrix[1, 1])
                         + (Points[i].Z * matrix[1, 2])
                         + (Points[i].W * matrix[1, 3]));

                    p.Z = (float)((Points[i].X * matrix[2, 0])
                         + (Points[i].Y * matrix[2, 1])
                         + (Points[i].Z * matrix[2, 2])
                         + (Points[i].W * matrix[2, 3]));

                    p.W = (float)((Points[i].X * matrix[3, 0])
                         + (Points[i].Y * matrix[3, 1])
                         + (Points[i].Z * matrix[3, 2])
                         + (Points[i].W * matrix[3, 3]));
                    Points[i] = p;
                }
            }
            else
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    point p = new point();

                    p.X = (float)(
                        (Points[i].X * matrix[0, 0])
                         + (Points[i].Y * matrix[0, 1])
                         + (Points[i].Z * matrix[0, 2]));
                    p.Y = (float)(
                        (Points[i].X * matrix[1, 0])
                       + (Points[i].Y * matrix[1, 1])
                       + (Points[i].Z * matrix[1, 2]));
                    p.Z = (float)(
                        (Points[i].X * matrix[2, 0])
                         + (Points[i].Y * matrix[2, 1])
                         + (Points[i].Z * matrix[2, 2]));
                    Points[i] = p;
                }

            }
            Rescale();
        }

        public void Rotate(double[,] matrix)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                point p = new point();
                p.X = (float)((Points[i].X * matrix[0, 0])
                     + (Points[i].Y * matrix[0, 1])
                     + (Points[i].Z * matrix[0, 2]));

                p.Y = (float)((Points[i].X * matrix[1, 0])
                     + (Points[i].Y * matrix[1, 1])
                     + (Points[i].Z * matrix[1, 2]));

                p.Z = (float)((Points[i].X * matrix[2, 0])
                     + (Points[i].Y * matrix[2, 1])
                     + (Points[i].Z * matrix[2, 2]));
            }
            Rescale();
        }

        public void Rescale()
        {
            Parallel.For(0, Points.Count, i =>
             {
                 point newPoint = new point();
                 newPoint.X = Points[i].X / Points[i].W;
                 newPoint.Y = Points[i].Y / Points[i].W;
                 newPoint.Z = Points[i].Z / Points[i].W;
                 newPoint.W = 1;//Points[i].F / Points[i].F;
                 Points[i] = newPoint;
             });
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(new SolidBrush(lineColor), penThickness);
            foreach (Triangle Tri in Triangles)
            {
                Point[] pointsarray = new Point[]
                 {
                     new Point ((int)Points[Tri.FirstPoint].X, (int)Points[Tri.FirstPoint].Y)
                     ,new Point ((int)Points[Tri.SecondPoint].X, (int)Points[Tri.SecondPoint].Y)
                     ,new Point ((int)Points[Tri.ThirdPoint].X, (int)Points[Tri.ThirdPoint].Y)
                   };
                g.DrawLines(pen, pointsarray);
            }
        }

        public void drawAsFilled(Graphics g)
        {
            SolidBrush brush = new SolidBrush(fillColor);

            foreach (Triangle Tri in Triangles)
            {
                //Construct an array of .net points to draw.
                Point[] pointsarray = new Point[]
                {
                    new Point ( (int)Points[Tri.FirstPoint].X, (int)Points[Tri.FirstPoint].Y)
                    ,new Point ( (int)Points[Tri.SecondPoint].X, (int)Points[Tri.SecondPoint].Y)
                    ,new Point ( (int)Points[Tri.ThirdPoint].X, (int)Points[Tri.ThirdPoint].Y)
                  };

                byte[] bite = new byte[3] { 1, 1, 1 };
                GraphicsPath netPoints = new GraphicsPath(pointsarray, bite);
                g.FillPath(brush, netPoints);
            }
        }

        public void removeHiddenPoints()
        {
            //Points.OrderBy(p => p.Z);

           Triangles.OrderBy(t => Points[t.FirstPoint].Z);
            // join the points and triangles list together and then sort on the new list
            //return triangles in z order
           
        }

        public override string ToString()
        {
            StringBuilder pointStrings = new StringBuilder();

            Points.ForEach(tmp =>
            {
                pointStrings.Append(string.Format(" X:{0}, Y:{1}, Z:{2},", tmp.X, tmp.Y, tmp.Z));
            });
            return string.Format("Points:{0}", pointStrings);
        }

        public void ResetPoints()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, verginPoints);
                ms.Position = 0;

                Points = (List<point>)formatter.Deserialize(ms);
            }
        }
    }
}
