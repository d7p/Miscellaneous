using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Android.App;

namespace graphicP2_test
{
	public class CustomMatrix
    {
        private float[] Points { get; set; }
        private int[] Triangles { get; set; }

        public CustomMatrix()
        {
			Points = new float[1];
			Triangles =new int[1];
        }

        public bool LoadMatrix(Stream fileStream)
        {
            try
            {
				int i =0;
				string[] line;
				using (StreamReader file = new StreamReader(fileStream))
                {
                    int totalVertices = Convert.ToInt32(file.ReadLine());
					Points= new float[totalVertices*3];
                    for (int currentLineNum = totalVertices; currentLineNum > 0; currentLineNum--)
                    {
                        line = file.ReadLine().Trim().Split(' ');
						Points[i] = (float)Convert.ToDouble(line[0]);
						Points[i+1] =(float)Convert.ToDouble(line[1]);
						Points[i+2] =(float)Convert.ToDouble(line[2]);
						i+=3;
                    }
                    int totalTriangles = Convert.ToInt32(file.ReadLine());
					Triangles = new int[totalTriangles*3];
					i=0;
                    for (int currenrtNum = totalTriangles; currenrtNum > 0; currenrtNum--)
                    {
                        line = file.ReadLine().Trim().Split(' ');

						Triangles[i] = Convert.ToInt32(line[1]);
						Triangles[i+1] =Convert.ToInt32(line[2]);
						Triangles[i+2] =Convert.ToInt32(line[3]);
						i+=3;
                    }
                }
			}
				catch (FileNotFoundException e) {
				#if DEBUG
      	       throw e;
				#endif
            }
            catch (FileLoadException e)
			{
				#if DEBUG
				throw e;
				#endif
            }

            return true;
        }

       public float[] CreateVertex()
		{
			return Points;
		}

		public int[] CreateFaceArray()
		{ 
			return Triangles;
		}
    }
}
