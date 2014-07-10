using Nel_S1Library;
using Nel_S1Library.ImageSearch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Example
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			fxOff_B.Add(new Bitmap("img\\fxOff.bmp"));
			fxOff_B.Add(new Bitmap("img\\fxOffSelected.bmp"));
			fxOff_B.Add(new Bitmap("img\\fxOffFXChannel.bmp"));
			fxOff_B.Add(new Bitmap("img\\fxOffPost.bmp"));
			fxOff_B.Add(new Bitmap("img\\fxOffPostSelected.bmp"));
			fxOn_B.Add(new Bitmap("img\\fxOn.bmp"));
			fxOn_B.Add(new Bitmap("img\\fxOnSelected.bmp"));
			fxOn_B.Add(new Bitmap("img\\fxOnFXChannel.bmp"));
			fxOn_B.Add(new Bitmap("img\\fxOnPost.bmp"));
			fxOn_B.Add(new Bitmap("img\\fxOnPostSelected.bmp"));
		}

		private S1Controller s1Controller = new S1Controller();
		private ImageSearch img = new ImageSearch();
		private IntPtr s1 = IntPtr.Zero;

		private List<Rectangle> location;
		private bool fxOn = true;
		private int offsetY = 47;
		private List<Bitmap> fxOff_B = new List<Bitmap>();
		private List<Bitmap> fxOn_B = new List<Bitmap>();

		private void FXTurnOn()
		{
			for (int loop = 0; loop < fxOff_B.Count(); loop++)
			{
				location = img.searchBitmap(fxOff_B[loop], 0);
				for (int i = 0; i < location.Count; i++)
					s1Controller.LeftClick(location[i].X, location[i].Y - offsetY);
			}
			fxOn = true;
		}

		private void FXTurnOff()
		{
			for (int loop = 0; loop < fxOn_B.Count(); loop++)
			{
				location = img.searchBitmap(fxOn_B[loop], 0);
				for (int i = 0; i < location.Count; i++)
					s1Controller.LeftClick(location[i].X, location[i].Y - offsetY);
			}
			fxOn = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				s1 = s1Controller.GetS1();
				img.CaptureApplication(s1);
				if (fxOn)
					FXTurnOff();
				else
					FXTurnOn();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}
	}
}