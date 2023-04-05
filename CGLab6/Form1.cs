using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CGLab6
{


    public partial class Form1 : Form
    {
		Graphics field;
		List<float[,]> triangle = new List<float[,]>();
		Engine engine = new Engine();

		public Form1()
        {
            InitializeComponent();
			field = pictureBox1.CreateGraphics();
		}
		
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
			
			//engine.Update(field,pictureBox1,trackBar1,trackBar2,trackBar3);
		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
			engine.Update(field, pictureBox1, trackBar1, trackBar2, trackBar3, trackBar4, checkedListBox1);
		}

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
			engine.Update(field, pictureBox1, trackBar1, trackBar2, trackBar3, trackBar4, checkedListBox1);
		}

        private void Form1_Load(object sender, EventArgs e)
        {
			engine.CreateObj(pictureBox1);
			engine.CreateCube();
			engine.Update(field, pictureBox1, trackBar1, trackBar2, trackBar3, trackBar4, checkedListBox1);
		}

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
			engine.Update(field, pictureBox1, trackBar1, trackBar2, trackBar3, trackBar4, checkedListBox1);
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

		private void button1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;
			string filename = openFileDialog1.FileName;
			engine.LoadFromObjectFile(filename);
		}

        private void button2_Click(object sender, EventArgs e)
        {
			engine.CreateCube();
		}

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
			engine.Update(field, pictureBox1, trackBar1, trackBar2, trackBar3, trackBar4, checkedListBox1);
		}
    }



    public class Engine : Form
	{
		List<float[,]> triangle = new List<float[,]>();
		float[,] m = new float[4, 4];
		float[] Camera = new float[3];
		



		Pen penV = new Pen(Color.FromArgb(255, 0, 0, 0));
		Pen penI = new Pen(Color.FromArgb(50, 0, 100, 255));
		SolidBrush Br = new SolidBrush(Color.FromArgb(255, 0, 0, 0));



		public bool LoadFromObjectFile(string sFilename)
		{
			triangle.Clear();


			// Local cache of verts
			using (StreamReader sr = File.OpenText(sFilename))
			{
				string s;
				List<float[]> verts = new List<float[]>();
				while ((s = sr.ReadLine()) != null)
				{


					if (s.StartsWith("v"))
					{

						double result;
						string[] buff = s.Split(' ');
						List<double> v = new List<double>();
						for (int i=0;i<buff.Count();i++)
						{
						double.TryParse(buff[i], NumberStyles.Any, CultureInfo.InvariantCulture, out result);
							v.Add(result);
						}
						verts.Add(new float[3] { (float)v[1], (float)v[2], (float)v[3] });
						v.Clear();
					}

					if (s.StartsWith("f"))
					{
						int result; ;
						string[] buff = s.Split(' ');
						List<int> v = new List<int>();
						for (int i = 0; i < buff.Count(); i++)
						{
							int.TryParse(buff[i], NumberStyles.Any, CultureInfo.InvariantCulture, out result);
							v.Add(result);
						}
						//s >> junk >> f[0] >> f[1] >> f[2];
						triangle.Add(new float[3, 3] { { verts[v[1] - 1][0], verts[v[1] - 1][1], verts[v[1] - 1][2] },
						{ verts[v[2] - 1][0], verts[v[2] - 1][1], verts[v[2] - 1][2] },
						{ verts[v[3] - 1][0], verts[v[3] - 1][1], verts[v[3] - 1][2] } });	
					}
				}
			}
			return true;
		}

			
	







		private void MultiplayMatrix(float[,] i, float[,] o, float[,] mat, int iter)
        {
			o[iter,0] = i[iter, 0] * mat[0,0] + i[iter, 1] * mat[1,0] + i[iter, 2] * mat[2,0] + mat[3,0];
			o[iter,1] = i[iter, 0] * mat[0,1] + i[iter, 1] * mat[1,1] + i[iter, 2] * mat[2,1] + mat[3,1];
			o[iter,2] = i[iter, 0] * mat[0,2] + i[iter, 1] * mat[1,2] + i[iter, 2] * mat[2,2] + mat[3,2];
			float w = i[iter, 0] * mat[0,3] + i[iter, 1] * mat[1,3] + i[iter, 2] * mat[2,3] + mat[3,3];

			if (w != 0.0f)
			{
				o[iter, 0] /= w; o[iter, 1] /= w; o[iter, 2] /= w;
			}
		}


		public void CreateCube()
        {
			triangle.Clear();
			triangle.Add(new float[3, 3] { { 0.0f, 0.0f, 0.0f }, { 0.0f, 1.0f, 0.0f }, { 1.0f, 1.0f, 0.0f } });
			triangle.Add(new float[3, 3] { { 0.0f, 0.0f, 0.0f }, { 1.0f, 1.0f, 0.0f }, { 1.0f, 0.0f, 0.0f } });

			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 0.0f }, { 1.0f, 1.0f, 0.0f }, { 1.0f, 1.0f, 1.0f } });
			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 0.0f }, { 1.0f, 1.0f, 1.0f }, { 1.0f, 0.0f, 1.0f } });

			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 1.0f }, { 1.0f, 1.0f, 1.0f }, { 0.0f, 1.0f, 1.0f } });
			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 1.0f }, { 0.0f, 1.0f, 1.0f }, { 0.0f, 0.0f, 1.0f } });

			triangle.Add(new float[3, 3] { { 0.0f, 0.0f, 1.0f }, { 0.0f, 1.0f, 1.0f }, { 0.0f, 1.0f, 0.0f } });
			triangle.Add(new float[3, 3] { { 0.0f, 0.0f, 1.0f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, 0.0f } });

			triangle.Add(new float[3, 3] { { 0.0f, 1.0f, 0.0f }, { 0.0f, 1.0f, 1.0f }, { 1.0f, 1.0f, 1.0f } });
			triangle.Add(new float[3, 3] { { 0.0f, 1.0f, 0.0f }, { 1.0f, 1.0f, 1.0f }, { 1.0f, 1.0f, 0.0f } });

			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f } });
			triangle.Add(new float[3, 3] { { 1.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f }, { 1.0f, 0.0f, 0.0f } });
		}


		public bool CreateObj(PictureBox pictureBox1)
		{
			float fNear = 0.1f;
			float fFar = 1000.0f;
			float fFov = 90.0f;
			float fAspectRatio = (float)pictureBox1.Height /(float)pictureBox1.Width;
			float fFovRad = (float)(1.0f / Math.Tan(fFov * 0.5f / 180.0f * 3.14159f));

			m[0,0] = fAspectRatio * fFovRad;
			m[1,1] = fFovRad;
			m[2,2] = fFar / (fFar - fNear);
			m[3,2] = (-fFar * fNear) / (fFar - fNear);
			m[2,3] = 1.0f;
			m[3,3] = 0.0f;

			return true;
		}

		public bool Update(Graphics field,PictureBox pictureBox1,TrackBar trackBar1, TrackBar trackBar2, TrackBar trackBar3, TrackBar trackBar4, CheckedListBox checkedListBox1)
        {
			field.Clear(Color.White);


			float[,] mrotz = new float[4, 4];
			float[,] mrotx = new float[4, 4];
			float[,] mroty = new float[4, 4];
			float fThetaZ = 0.04f * trackBar1.Value;
			float fThetaX = 0.04f * trackBar2.Value;
			float fThetaY = 0.04f * trackBar3.Value;

			mrotz[0,0] = (float)Math.Cos(fThetaZ * 0.5f);
			mrotz[0,1] = (float)Math.Sin(fThetaZ * 0.5f);
			mrotz[1,0] = -(float)Math.Sin(fThetaZ * 0.5f);
			mrotz[1,1] = (float)Math.Cos(fThetaZ * 0.5f);
			mrotz[2,2] = 1;
			mrotz[3,3] = 1;

			
			mrotx[0,0] = 1;
			mrotx[1,1] = (float)Math.Cos(fThetaX * 0.5f);
			mrotx[1,2] = (float)Math.Sin(fThetaX * 0.5f);
			mrotx[2,1] = -(float)Math.Sin(fThetaX * 0.5f);
			mrotx[2,2] = (float)Math.Cos(fThetaX * 0.5f);
			mrotx[3,3] = 1;


			mroty[0, 0] = (float)Math.Cos(fThetaY * 0.5f);
			mroty[0, 2] = -(float)Math.Sin(fThetaY * 0.5f);
			mroty[2, 0] = (float)Math.Sin(fThetaY * 0.5f);
			mroty[1, 1] = 1;
			mroty[2, 2] = (float)Math.Cos(fThetaY * 0.5f);
			mroty[3, 3] = 1;


			List<float[,]> vecTrianglesToRaster = new List<float[,]>();
			//List<float> Col = new List<float>();

			for (int i = 0;i<triangle.Count;i++)
            {
				float[,] tri = new float[3, 3];
				float[,] triTrans = new float[3, 3];
				float[,] triRotZ = new float[3, 3];
				float[,] triRotX = new float[3, 3];
				float[,] triRotY = new float[3, 3];

				//крутим по z
				MultiplayMatrix(triangle[i], triRotZ, mrotz, 0);
				MultiplayMatrix(triangle[i], triRotZ, mrotz, 1);
				MultiplayMatrix(triangle[i], triRotZ, mrotz, 2);

				//крутим по x
				MultiplayMatrix(triRotZ, triRotX, mrotx, 0);
				MultiplayMatrix(triRotZ, triRotX, mrotx, 1);
				MultiplayMatrix(triRotZ, triRotX, mrotx, 2);

				//крутим по y
				MultiplayMatrix(triRotX, triRotY, mroty, 0);
				MultiplayMatrix(triRotX, triRotY, mroty, 1);
				MultiplayMatrix(triRotX, triRotY, mroty, 2);


				triTrans = triRotY;
				triTrans[0, 2] = triRotY[0, 2] + (float)Convert.ToDouble(trackBar4.Value);
				triTrans[1, 2] = triRotY[1, 2] + (float)Convert.ToDouble(trackBar4.Value);
				triTrans[2, 2] = triRotY[2, 2] + (float)Convert.ToDouble(trackBar4.Value);


				float[] line1 = new float[3];
				float[] line2 = new float[3];
				float[] normal = new float[3];


				line1[0] = triTrans[1, 0] - triTrans[0, 0];
				line1[1] = triTrans[1, 1] - triTrans[0, 1];
				line1[2] = triTrans[1, 2] - triTrans[0, 2];
					
				line2[0] = triTrans[2, 0] - triTrans[0, 0];
				line2[1] = triTrans[2, 1] - triTrans[0, 1];
				line2[2] = triTrans[2, 2] - triTrans[0, 2];

				normal[0] = line1[1] * line2[2] - line1[2] * line2[1];
				normal[1] = line1[2] * line2[0] - line1[0] * line2[2];
				normal[2] = line1[0] * line2[1] - line1[1] * line2[0];

				float lenght = (float)Math.Sqrt(normal[0] * normal[0] + normal[1] * normal[1] + normal[2] * normal[2]);
				normal[0] /= lenght; normal[1] /= lenght; normal[2] /= lenght;


				if (normal[0] * (triTrans[0, 0] - Camera[0]) +
					normal[1] * (triTrans[0, 1] - Camera[1]) +
					normal[2] * (triTrans[0, 2] - Camera[2]) < 0.0f)

				//if (normal[2] < 0 || checkBox1.Checked)
				{

					float[] light_source = new float[3] { 0.0f, 0.0f, -1.0f };
					float l = (float)Math.Sqrt(light_source[0] * light_source[0] + light_source[1] * light_source[1] + light_source[2] * light_source[2]);
					light_source[0] /= l; light_source[1] /= l; light_source[2] /= l;

					float dp = normal[0] * light_source[0] + normal[1] * light_source[1] + normal[2] * light_source[2];

					//Br = new SolidBrush(Color.FromArgb(255, Math.Abs((int)(255 * dp)), Math.Abs((int)(255 * dp)), Math.Abs((int)(255 * dp))));
					//поектируем 3D объект на плоскость
					MultiplayMatrix(triTrans, tri, m, 0);
					MultiplayMatrix(triTrans, tri, m, 1);
					MultiplayMatrix(triTrans, tri, m, 2);

					tri[0, 0] += 1.0f; tri[0, 1] += 1.0f;
					tri[1, 0] += 1.0f; tri[1, 1] += 1.0f;
					tri[2, 0] += 1.0f; tri[2, 1] += 1.0f;

					tri[0, 0] *= 0.5f * (float)pictureBox1.Width;
					tri[0, 1] *= 0.5f * (float)pictureBox1.Height;
					tri[1, 0] *= 0.5f * (float)pictureBox1.Width;
					tri[1, 1] *= 0.5f * (float)pictureBox1.Height;
					tri[2, 0] *= 0.5f * (float)pictureBox1.Width;
					tri[2, 1] *= 0.5f * (float)pictureBox1.Height;

					vecTrianglesToRaster.Add(new float[4,3]{ { tri[0,0],tri[0, 1],tri[0, 1]}, { tri[1, 0],tri[1, 1],tri[1, 2] }, { tri[2, 0],tri[2, 1],tri[2, 2] }, {dp,0.0f,0.0f } });
					//Col.Add(dp);

					Point[] point = new Point[3];
					point[0] = new Point((int)tri[0, 0], (int)tri[0, 1]);
					point[1] = new Point((int)tri[1, 0], (int)tri[1, 1]);
					point[2] = new Point((int)tri[2, 0], (int)tri[2, 1]);
					/*if(checkedListBox1.GetItemChecked(2))
					field.FillPolygon(Br, point);*/
					if(checkedListBox1.GetItemChecked(0))
					field.DrawPolygon(penV, point);

				}
				else
					if(checkedListBox1.GetItemChecked(1))
					{
						MultiplayMatrix(triTrans, tri, m, 0);
						MultiplayMatrix(triTrans, tri, m, 1);
						MultiplayMatrix(triTrans, tri, m, 2);

						tri[0, 0] += 1.0f; tri[0, 1] += 1.0f;
						tri[1, 0] += 1.0f; tri[1, 1] += 1.0f;
						tri[2, 0] += 1.0f; tri[2, 1] += 1.0f;

						tri[0, 0] *= 0.5f * (float)pictureBox1.Width;
						tri[0, 1] *= 0.5f * (float)pictureBox1.Height;
						tri[1, 0] *= 0.5f * (float)pictureBox1.Width;
						tri[1, 1] *= 0.5f * (float)pictureBox1.Height;
						tri[2, 0] *= 0.5f * (float)pictureBox1.Width;
						tri[2, 1] *= 0.5f * (float)pictureBox1.Height;
						Point[] point = new Point[3];
						point[0] = new Point((int)tri[0, 0], (int)tri[0, 1]);
						point[1] = new Point((int)tri[1, 0], (int)tri[1, 1]);
						point[2] = new Point((int)tri[2, 0], (int)tri[2, 1]);
						field.DrawPolygon(penI, point);

					}
			}

			vecTrianglesToRaster.Sort((t1, t2) =>
			{
				float z1 = (t1[0, 2] + t1[1, 2] + t1[2, 2]) / 3.0f;
				float z2 = (t2[0, 2] + t2[1, 2] + t2[2, 2]) / 3.0f;
				return z1.CompareTo(z2);
			});
			


			for(int i = 0; i < vecTrianglesToRaster.Count; i++)
            {
				Point[] point = new Point[3];
				point[0] = new Point((int)vecTrianglesToRaster[i][0, 0], (int)vecTrianglesToRaster[i][0, 1]);
				point[1] = new Point((int)vecTrianglesToRaster[i][1, 0], (int)vecTrianglesToRaster[i][1, 1]);
				point[2] = new Point((int)vecTrianglesToRaster[i][2, 0], (int)vecTrianglesToRaster[i][2, 1]);
				Br = new SolidBrush(Color.FromArgb(255, Math.Abs((int)(255 * vecTrianglesToRaster[i][3,0])), Math.Abs((int)(255 * vecTrianglesToRaster[i][3, 0])), Math.Abs((int)(255 * vecTrianglesToRaster[i][3, 0]))));
				if (checkedListBox1.GetItemChecked(2))
					field.FillPolygon(Br, point);
				if (checkedListBox1.GetItemChecked(0))
					field.DrawPolygon(penV, point);
			}


			return true;
        }
	}
    


}






