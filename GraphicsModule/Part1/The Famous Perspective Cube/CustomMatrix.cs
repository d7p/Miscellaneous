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
	/// <summary>
	/// Point.
	/// </summary>
    [Serializable()]
    public class point : ISerializable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="point"/> class.
		/// </summary>
        public point()
        {
            this.X = new float();
            this.Y = new float();
            this.Z = new float();
            this.W = new float();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="point"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="ctxt">Ctxt.</param>
        public point(SerializationInfo info, StreamingContext ctxt)
        {
            this.X = (float)info.GetValue("X", typeof(float));
            this.Y = (float)info.GetValue("Y", typeof(float));
            this.Z = (float)info.GetValue("Z", typeof(float));
            this.W = (float)info.GetValue("W", typeof(float));

        }

		/// <Docs>To be added: an object of type 'SerializationInfo'</Docs>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="ctxt">Ctxt.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("X", this.X);
            info.AddValue("Y", this.Y);
            info.AddValue("Z", this.Z);
            info.AddValue("W", this.W);
        }
    }

	/// <summary>
	/// Triangle.
	/// </summary>
    public struct Triangle
    {
        public int Number;
        public int FirstPoint;
        public int SecondPoint;
        public int ThirdPoint;
    }

	/// <summary>
	/// Custom matrix.
	/// </summary>
    public class CustomMatrix
    {
        private List<point> Points { get; set; }
        private List<point> verginPoints { get; set; }
        private List<Triangle> Triangles { get; set; }
        public Color lineColor { get; set; }
        public int penThickness { get; set; }
        public Color fillColor { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="The_Famous_Perspective_Cube.CustomMatrix"/> class.
		/// </summary>
        public CustomMatrix()
        {
            Points = new List<point>();
            verginPoints = new List<point>();
            Triangles = new List<Triangle>();
            lineColor = new Color();
            penThickness = new int();
        }

		/// <summary>
		/// Loads object
		/// </summary>
		/// <returns><c>true</c>, if matrix was loaded, <c>false</c> otherwise.</returns>
		/// <param name="fileName">File name.</param>
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

		/// <summary>
		/// Transform the instance to a specified matrix.
		/// </summary>
		/// <param name="matrix">Matrix.</param>
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

		/// <summary>
		/// Rotate the specified matrix.
		/// </summary>
		/// <param name="matrix">Matrix.</param>
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

		/// <summary>
		/// Rescale this instance.
		/// </summary>
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

		/// <summary>
		/// Draw the specified g. As a wireframe
		/// </summary>
		/// <param name="g">The green component.</param>
        public void draw(Graphics g)
        {
			removeHiddenPoints();
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

		/// <summary>
		/// Draws as filled polygon 
		/// </summary>
		/// <param name="g">The green component.</param>
        public void drawAsFilled(Graphics g)
        {
			removeHiddenPoints();
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
           //order the triangles in order of the depth
			Triangles.OrderBy(t => Points[t.FirstPoint].Z).ThenBy(t => Points[t.SecondPoint].Z).ThenBy(t => Points[t.ThirdPoint].Z);
           
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
