using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using System.Drawing.Html;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using PdfSharp.Pdf;


namespace System.Drawing.Html.PDF
{
  using Renderer;

  public class PDFGraphics : IGraphics
  {
    private XGraphics g;
    private Graphics tmpGraphics;
    private XPdfFontOptions fontOptions = new XPdfFontOptions(true);

    public PDFGraphics(XGraphics g)
    {
      this.g = g;

      var ctrl = new Control();
      tmpGraphics = ctrl.CreateGraphics();
    }

    #region IDisposable Member

    public void Dispose()
    {
      g.Dispose();
    }

    #endregion


    #region IGraphics Member

    private XFont GetXFont(Font f)
    {
      var worldFont = new Font(f.FontFamily, f.Size, f.Style, GraphicsUnit.World);
      return new XFont(worldFont, fontOptions);
    }

    private XBrush GetXBrush(Brush b)
    {
      XBrush brush = null;
      LinearGradientBrush lgBrush;
      if ((lgBrush = b as LinearGradientBrush) != null)
      {
        if (lgBrush.LinearColors.Length == 2)
        {
          brush = new XLinearGradientBrush(lgBrush.Rectangle,
            lgBrush.LinearColors[0], lgBrush.LinearColors[1],
            XLinearGradientMode.ForwardDiagonal);
        }
      }
      else if (b is SolidBrush && (b as SolidBrush).Color.A != 0)
      {
        brush = b;
      }
      return brush;
    }


    public void DrawImage(Image image, Rectangle rectangle)
    {
      g.DrawImage(image, rectangle);
    }

    public void DrawString(string p, Font f, SolidBrush b, float x, float y)
    {
      try
      {
        g.DrawString(p, GetXFont(f), b, x, y, XStringFormats.TopLeft);
      }
      catch (Exception ex)
      {
        //TODO
      }
    }

    public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
    {
      g.DrawLine(pen, x1, y1, x2, y2);
    }

    public void FillPath(SolidBrush b, System.Drawing.Drawing2D.GraphicsPath graphicsPath)
    {
      this.FillPath((Brush)b, graphicsPath);
    }

    public System.Drawing.Drawing2D.SmoothingMode SmoothingMode { get; set; }

    public void FillRectangle(Brush b, RectangleF rectangle)
    {
      var brush = GetXBrush(b);
      if (brush != null)
      {
        g.DrawRectangle(brush, rectangle);
      }
    }

    public Region[] MeasureCharacterRanges(string space, Font f, RectangleF rectangleF, StringFormat sf)
    {
      var worldFont = new Font(f.FontFamily, f.Size, f.Style, GraphicsUnit.World);

      return tmpGraphics.MeasureCharacterRanges(space, worldFont,
        rectangleF, sf);
    }

    public SizeF MeasureString(string text, Font f)
    {
      var worldFont = new Font(f.FontFamily, f.Size, f.Style, GraphicsUnit.World);

      return tmpGraphics.MeasureString(text, worldFont);
    }


    private Region clip;
    public Region Clip
    {
      get
      {
        return clip;
      }
      set
      {
        clip = value;
        // TODO:
        //g.IntersectClip(clip.GetP);
      }
    }

    public void SetClip(RectangleF area)
    {
      g.IntersectClip(area);
    }

    public void SetClip(Region prevClip, System.Drawing.Drawing2D.CombineMode combineMode)
    {
      // TODO:
      //g.IntersectClip(clip.GetP);
    }

    public void FillPath(Brush b, GraphicsPath roundrect)
    {
      var brush = GetXBrush(b);
      if (brush != null)
      {
        var path = new XGraphicsPath(roundrect.PathPoints, roundrect.PathTypes, (XFillMode)roundrect.FillMode);
        g.DrawPath(brush, path);
      }
    }

    public void DrawRectangle(Pen pen, Rectangle rectangle)
    {
      g.DrawRectangle(pen, rectangle);
    }

    public float GetFontHeight(Font f)
    {
      return f.GetHeight(tmpGraphics);
    }
    
    public RectangleF GetRegionBounds(Region region)
    {
      return region.GetBounds(tmpGraphics);
    }

    public void TranslateTransform(float x, float y)
    {
      g.TranslateTransform(x, y);
    }

    #endregion


    /// <summary>
    /// This distills the steps to render the PDF from HTML
    /// </summary>
    public static void CreatePDF(string html, string filename)
    {
      CreatePDF(html, filename, new RectangleF( 20, 20, 550, 800 ));
    }

    /// <summary>
    /// This distills the steps to render the PDF from HTML
    /// 
    /// Allows the bounds to be set (print margins)
    /// </summary>
    public static void CreatePDF( string html, string filename, RectangleF bounds )
    {
      // Create a new PDF document
      PdfDocument document = new PdfDocument();

      // Create an empty page
      PdfPage page = document.AddPage();

      // Get an XGraphics object for drawing
      XGraphics gfx = XGraphics.FromPdfPage( page );

      HtmlRenderer.Render( new PDFGraphics( gfx ), html, bounds, true );

      // Save the document...
      document.Save( filename );
    }

  }
}
