using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

using System.Drawing;

namespace System.Windows.Forms
{
  using System.Drawing.Html;
  using System.Drawing.Html.Renderer;
  /// <summary>
  /// Provides HTML rendering on the tooltips
  /// </summary>
  public class HtmlToolTip
      : ToolTip
  {
    #region Fields

    private InitialContainer container;

    #endregion

    #region Ctor

    public HtmlToolTip()
    {

      OwnerDraw = true;

      Popup += new PopupEventHandler( HtmlToolTip_Popup );
      Draw += new DrawToolTipEventHandler( HtmlToolTip_Draw );

    }

    #endregion

    void HtmlToolTip_Popup( object sender, PopupEventArgs e )
    {
      string text = this.GetToolTip( e.AssociatedControl );
      string font = string.Format( NumberFormatInfo.InvariantInfo, "font: {0}pt {1}", e.AssociatedControl.Font.Size, e.AssociatedControl.Font.FontFamily.Name );

      //Create fragment container
      container = new InitialContainer( "<table class=htmltooltipbackground cellspacing=5 cellpadding=0 style=\"" + font + "\"><tr><td style=border:0px>" + text + "</td></tr></table>" );
      container.SetBounds( new Rectangle( 0, 0, 10, 10 ) );
      container.AvoidGeometryAntialias = true;

      //Measure bounds of the container
      using ( IGraphics g = new GraphicsWrapper( e.AssociatedControl.CreateGraphics() ) )
      {
        container.MeasureBounds( g );
      }

      //Set the size of the tooltip
      e.ToolTipSize = Size.Round( container.MaximumSize );

    }

    void HtmlToolTip_Draw( object sender, DrawToolTipEventArgs e )
    {
      e.Graphics.Clear( Color.White );

      if ( container != null )
      {
        //Draw HTML!
        container.Paint( new GraphicsWrapper( e.Graphics ) );
      }

    }
  }
}
