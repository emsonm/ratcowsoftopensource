using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace System.Drawing.Html
{
  using Renderer;

	public class GraphicsWrapper : IGraphics
	{
		private Graphics g;

		public GraphicsWrapper(Graphics g)
		{
			this.g = g;
		}

		#region IDisposable Member

		public void Dispose()
		{
			g.Dispose();
		}

		#endregion


		#region IGraphics Member

		public void DrawImage(Image image, Rectangle rectangle)
		{
			g.DrawImage(image, rectangle);
		}

		public void DrawString(string p, Font f, SolidBrush b, float x, float y)
		{
			g.DrawString(p, f, b, x, y);
		}

		public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
		{
			g.DrawLine(pen, x1, y1, x2, y2);
		}

		public void FillPath(SolidBrush b, Drawing2D.GraphicsPath graphicsPath)
		{
			g.FillPath(b, graphicsPath);
		}

		public Drawing2D.SmoothingMode SmoothingMode
		{
			get
			{
				return g.SmoothingMode;
			}
			set
			{
				g.SmoothingMode = value;
			}
		}

		public void FillRectangle(Brush b, RectangleF rectangle)
		{
			g.FillRectangle(b, rectangle);
		}

		public Region[] MeasureCharacterRanges(string space, Font font, 
			RectangleF rectangleF, StringFormat sf)
		{
			return g.MeasureCharacterRanges(space, font, rectangleF, sf);
		}

		public Region Clip
		{
			get
			{
				return g.Clip;
			}
			set
			{
				g.Clip = value;
			}
		}

		public void SetClip(RectangleF area)
		{
			g.SetClip(area);
		}

		public void SetClip(Region prevClip, Drawing2D.CombineMode combineMode)
		{
			g.SetClip(prevClip, combineMode);
		}

		public void FillPath(Brush b, Drawing2D.GraphicsPath roundrect)
		{
			g.FillPath(b, roundrect);
		}

		public void DrawRectangle(Pen pen, Rectangle rectangle)
		{
			g.DrawRectangle(pen, rectangle);
		}

		public float GetFontHeight(Font f)
		{
			return f.GetHeight(g);
		}

		public RectangleF GetRegionBounds(Region region)
		{
			return region.GetBounds(g);
		}


		public void TranslateTransform(float x, float y)
		{
			g.TranslateTransform(x, y);
		}

		public SizeF MeasureString(string text, Font font)
		{
			var size = g.MeasureString(text, font);

			return size;
		}

		#endregion



	}
}
