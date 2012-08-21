using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

namespace System.Drawing.Html.Renderer
{
	public interface IGraphics : IDisposable
	{

		void DrawImage(Image image, Rectangle rectangle);

		void DrawString(string p, Font f, SolidBrush b, float p_2, float p_3);

		void DrawLine(Pen pen, float x1, float y, float x2, float y_2);

		void FillPath(SolidBrush b, GraphicsPath GraphicsPath);

		Drawing2D.SmoothingMode SmoothingMode { get; set; }

		void FillRectangle(Brush b, RectangleF rectangle);

		Region[] MeasureCharacterRanges(string space, Font font, RectangleF rectangleF, StringFormat sf);

		Region Clip { get; set; }

		void SetClip(RectangleF area);

		void SetClip(Region prevClip, Drawing2D.CombineMode combineMode);

		void FillPath(Brush b, Drawing2D.GraphicsPath roundrect);

		void DrawRectangle(Pen pen, Rectangle rectangle);

		float GetFontHeight(Font f);

		RectangleF GetRegionBounds(Region region);

		void TranslateTransform(float x, float y);

		SizeF MeasureString(string text, Font font);
	}
}
