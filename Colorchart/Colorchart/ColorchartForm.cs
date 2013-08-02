/*
 * Copyright 2013 Rat Cow Software and Matt Emson. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used
 *    to endorse or promote products derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 *
 */

//set this to validate color's are being set correctly whilst sorting
//#define DEBUG_COLOR_ASSIGNMENT

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RatCow.Colorchart
{
    public partial class ColorchartForm : Form
    {
        #region Fields

        private List<ColorItem> items;

        private static ColorSort sortKind = ColorSort.Alpha;

        #endregion Fields

        #region Constructor

        public ColorchartForm()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Private methods

        private void LoadColors()
        {
            colorsListView.VirtualMode = true;

            items = new List<ColorItem>();
            smallImages.Images.Clear();
            largeImages.Images.Clear();

            //get all the static properties
            var pia = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

            //filter out anything that's not a "Color"
            var piaf = from c in pia where c.PropertyType.Name == "Color" select c;

            //generate a list, fixing up all images used to show as icons
            foreach (var pi in piaf)
            {
                var color = (Color)pi.GetValue(null, null);

                var bmp = GetBitmap(color, 8, 8);
                smallImages.Images.Add(bmp);

                bmp = GetBitmap(color, 32, 32);
                largeImages.Images.Add(bmp);

                items.Add(new ColorItem() { Name = pi.Name, Color = color, ImageIndex = largeImages.Images.Count - 1 });
            }

            items.Sort(alphaSort);

            colorsListView.VirtualListSize = items.Count;
        }

        private Bitmap GetBitmap(Color color, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, width - 1, height - 1);
            g.DrawRectangle(new Pen(Color.Black), 0, 0, width - 1, height - 1);
            g.Dispose();
            return bmp;
        }

        private Color GetSelectedItemColor()
        {
            Color result = Color.Transparent;

            for (int i = 0; i < colorsListView.Items.Count; i++)
            {
                if (colorsListView.Items[i].Selected)
                {
                    result = ((ColorItem)items[i]).Color;
                    break;
                }
            }
            return result;
        }

        #endregion Private methods

        #region Event handlers

        private void ColorchartForm_Load(object sender, EventArgs e)
        {
            LoadColors();
        }

        private void colorsListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.Item == null)
                e.Item = new ListViewItem();

            var item = items[e.ItemIndex];

#if DEBUG_COLOR_ASSIGNMENT
            e.Item.BackColor = item.Color;
#endif

            e.Item.ImageIndex = item.ImageIndex;
            e.Item.Text = item.Name;
        }

        private void largeViewButton_Click(object sender, EventArgs e)
        {
            colorsListView.View = View.LargeIcon;
        }

        private void smallViewButton_Click(object sender, EventArgs e)
        {
            colorsListView.View = View.SmallIcon;
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            colorsListView.View = View.List;
        }

        private void detailsButton_Click(object sender, EventArgs e)
        {
            colorsListView.View = View.Details;
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            sortKind = nextItem(sortKind);

            if (sortKind == ColorSort.Alpha)
            {
                items.Sort(alphaSort);
            }
            else if (sortKind == ColorSort.HSB)
            {
                items = (from x in items
                         orderby x.Color.GetHue(), x.Color.GetSaturation(), x.Color.GetBrightness()
                         select x).ToList();
            }
            else if (sortKind == ColorSort.SBH)
            {
                items = (from x in items
                         orderby x.Color.GetSaturation(), x.Color.GetBrightness(), x.Color.GetHue()
                         select x).ToList();
            }
            else if (sortKind == ColorSort.SHB)
            {
                items = (from x in items
                         orderby x.Color.GetSaturation(), x.Color.GetHue(), x.Color.GetBrightness()
                         select x).ToList();
            }
            else if (sortKind == ColorSort.BHS)
            {
                items = (from x in items
                         orderby x.Color.GetBrightness(), x.Color.GetHue(), x.Color.GetSaturation()
                         select x).ToList();
            }
            else if (sortKind == ColorSort.BSH)
            {
                items = (from x in items
                         orderby x.Color.GetBrightness(), x.Color.GetSaturation(), x.Color.GetHue()
                         select x).ToList();
            }
            else
            {
                items.Sort(argbSort);
            }

            sortButton.Text = sortKind.ToString();

            colorsListView.Refresh();
        }

        private void colorsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var selectedColor = GetSelectedItemColor();
            if (selectedColor != Color.Transparent)
            {
                rbgValue.Text = String.Format("R: {0} G: {1} B: {2} A: {3}", selectedColor.R, selectedColor.G, selectedColor.B, selectedColor.A);
            }
            else rbgValue.Text = "...";

        }

        private void colorsListView_DoubleClick(object sender, EventArgs e)
        {
            var selectedColor = GetSelectedItemColor();
            if (selectedColor != Color.Transparent)
            {
                ColorChangeDialog.Show(selectedColor);
            }
        }

        #endregion Event handlers

        #region Sort related

        private ColorSort nextItem(ColorSort sortKind)
        {
            switch (sortKind)
            {
                case ColorSort.Alpha: return ColorSort.HSB;
                case ColorSort.HSB: return ColorSort.SBH;
                case ColorSort.SBH: return ColorSort.BSH;
                case ColorSort.BSH: return ColorSort.BHS;
                case ColorSort.BHS: return ColorSort.SHB;
                case ColorSort.SHB: return ColorSort.Argb;
                case ColorSort.Argb: return ColorSort.R;
                case ColorSort.R: return ColorSort.G;
                case ColorSort.G: return ColorSort.B;
                case ColorSort.B:
                default:
                    return ColorSort.Alpha;
            }
        }

        private static int alphaSort(ColorItem x, ColorItem y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }

        private static int argbSort(ColorItem x, ColorItem y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    switch (sortKind)
                    {
                        case ColorSort.Argb:
                            return x.Color.ToArgb().CompareTo(y.Color.ToArgb());
                        case ColorSort.R:
                            return x.Color.R.CompareTo(y.Color.R);
                        case ColorSort.G:
                            return x.Color.G.CompareTo(y.Color.G);
                        case ColorSort.B:
                            return x.Color.B.CompareTo(y.Color.B);
                        default:
                            return alphaSort(x, y);
                    }
                }
            }
        }

        #endregion Sort related

        #region Private classes and enums

        private enum ColorSort : byte { Alpha = 0, HSB, SBH, SHB, BHS, BSH, Argb, R, G, B };

        private class ColorItem
        {
            public Color Color { get; set; } //as a back-up

            public string Name { get; set; }

            public int ImageIndex { get; set; }
        }

        #endregion Private classes and enums
    }
}