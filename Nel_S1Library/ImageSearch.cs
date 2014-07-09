using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Nel_S1Library.ImageSearch
{
	public class ImageSearch
	{
		public List<Rectangle> location;

		private Bitmap tempImg;

		/// <summary>
		/// Takes a screenshot of Studio One.
		/// </summary>
		/// <param name="s1">Studio One's handle.</param>
		public void CaptureApplication(IntPtr s1)
		{
			var rect = new Rect();
			GetWindowRect(s1, ref rect);

			int width = rect.right - rect.left;
			int height = rect.bottom - rect.top;

			tempImg = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Graphics graphics = Graphics.FromImage(tempImg);
			graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
		}

		/// <summary>
		/// Searches for matching of smallBmp inside Studio One's screenshot.
		/// </summary>
		/// <param name="smallBmp">A bitmap (.bmp) of something you want to search.</param>
		/// <param name="tolerance">Literally.</param>
		/// <returns></returns>
		public List<Rectangle> searchBitmap(Bitmap smallBmp, double tolerance)
		{
			if (tempImg == null)
				throw new Exception("You haven't captured the screen. Use CaptureApplication(IntPtr s1)");
			location = new List<Rectangle>();
			BitmapData smallData =
			  smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
					   System.Drawing.Imaging.ImageLockMode.ReadOnly,
					   System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			BitmapData bigData =
			  tempImg.LockBits(new Rectangle(0, 0, tempImg.Width, tempImg.Height),
					   System.Drawing.Imaging.ImageLockMode.ReadOnly,
					   System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int smallStride = smallData.Stride;
			int bigStride = bigData.Stride;

			int bigWidth = tempImg.Width;
			int bigHeight = tempImg.Height - smallBmp.Height + 1;
			int smallWidth = smallBmp.Width * 3;
			int smallHeight = smallBmp.Height;

			int margin = Convert.ToInt32(255.0 * tolerance);

			unsafe
			{
				byte* pSmall = (byte*)(void*)smallData.Scan0;
				byte* pBig = (byte*)(void*)bigData.Scan0;

				int smallOffset = smallStride - smallBmp.Width * 3;
				int bigOffset = bigStride - tempImg.Width * 3;

				bool matchFound = true;

				for (int y = 0; y < bigHeight; y++)
				{
					for (int x = 0; x < bigWidth; x++)
					{
						byte* pBigBackup = pBig;
						byte* pSmallBackup = pSmall;

						//Look for the small picture.
						for (int i = 0; i < smallHeight; i++)
						{
							int j = 0;
							matchFound = true;
							for (j = 0; j < smallWidth; j++)
							{
								//With tolerance: pSmall value should be between margins.
								int inf = pBig[0] - margin;
								int sup = pBig[0] + margin;
								if (sup < pSmall[0] || inf > pSmall[0])
								{
									matchFound = false;
									break;
								}

								pBig++;
								pSmall++;
							}

							if (!matchFound) break;

							//We restore the pointers.
							pSmall = pSmallBackup;
							pBig = pBigBackup;

							//Next rows of the small and big pictures.
							pSmall += smallStride * (1 + i);
							pBig += bigStride * (1 + i);
						}

						if (matchFound)
						{
							location.Add(new Rectangle(x, y, smallBmp.Width, smallBmp.Height));
							//Restore anyway, adding to a list now, we need to continue!
							pBig = pBigBackup;
							pSmall = pSmallBackup;
							pBig += 3;
							//break;
						}

						//If no match found, we restore the pointers and continue.
						else
						{
							pBig = pBigBackup;
							pSmall = pSmallBackup;
							pBig += 3;
						}
					}

					if (matchFound) break;

					pBig += bigOffset;
				}
			}

			tempImg.UnlockBits(bigData);
			smallBmp.UnlockBits(smallData);

			return location;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

		[StructLayout(LayoutKind.Sequential)]
		private struct Rect
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
	}
}