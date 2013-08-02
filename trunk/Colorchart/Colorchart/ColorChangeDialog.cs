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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RatCow.Colorchart
{
    public partial class ColorChangeDialog : Form
    {
        #region Fields

        enum TargetColorElement { R, G, B };

        Color _startColor;
        Color _currentColor;
        TargetColorElement _targetColorElement = TargetColorElement.R;
        Font defaultFont = new Font("Arial", 9);
        Font selectedFont = new Font("Arial", 9, FontStyle.Bold);

        #endregion

        #region Constructor

        public ColorChangeDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void SetUI()
        {
            colorPanel.BackColor = _currentColor;
            rLabel.Text = String.Format("R : {0}", _currentColor.R);
            gLabel.Text = String.Format("G : {0}", _currentColor.G);
            bLabel.Text = String.Format("B : {0}", _currentColor.B);

            rLabel.Font = (_targetColorElement == TargetColorElement.R ? selectedFont : defaultFont);
            gLabel.Font = (_targetColorElement == TargetColorElement.G ? selectedFont : defaultFont);
            bLabel.Font = (_targetColorElement == TargetColorElement.B ? selectedFont : defaultFont);

            switch (_targetColorElement)
            {
                case TargetColorElement.R:
                    shadeControl.Value = _currentColor.R;
                    break;

                case TargetColorElement.G:
                    shadeControl.Value = _currentColor.G;
                    break;

                case TargetColorElement.B:
                    shadeControl.Value = _currentColor.B;
                    break;
            }
        }


        public static Color Show(Color startColor)
        {
            using (var dialog = new ColorChangeDialog())
            {
                dialog._startColor = startColor;
                dialog._currentColor = startColor;

                dialog.ShowDialog();

                return dialog._currentColor;
            }
        }

        #endregion

        #region Event handlers

        private void ColorChangeDialog_Load(object sender, EventArgs e)
        {
            SetUI();
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (sender == rLabel)
            {
                _targetColorElement = TargetColorElement.R;
            }
            else if (sender == gLabel)
            {
                _targetColorElement = TargetColorElement.G;
            }
            else if (sender == bLabel)
            {
                _targetColorElement = TargetColorElement.B;
            }

            SetUI();
        }

        private void shadeControl_ValueChanged(object sender, EventArgs e)
        {
            switch (_targetColorElement)
            {
                case TargetColorElement.R:
                    _currentColor = Color.FromArgb(shadeControl.Value, _currentColor.G, _currentColor.B);
                    break;

                case TargetColorElement.G:
                    _currentColor = Color.FromArgb(_currentColor.R, shadeControl.Value, _currentColor.B);
                    break;

                case TargetColorElement.B:
                    _currentColor = Color.FromArgb(_currentColor.R, _currentColor.G, shadeControl.Value);
                    break;
            }

            SetUI();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            _currentColor = Color.FromArgb(_currentColor.R + 10 >= 255 ? 255 : _currentColor.R + 10, _currentColor.G + 10 >= 255 ? 255 : _currentColor.G + 10, _currentColor.B + 10 >= 255 ? 255 : _currentColor.B + 10);

            SetUI();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            _currentColor = Color.FromArgb(_currentColor.R - 10 <= 0 ? 0 : _currentColor.R - 10, _currentColor.G - 10 <= 0 ? 0 : _currentColor.G - 10, _currentColor.B - 10 <= 0 ? 0 : _currentColor.B - 10);


            SetUI();
        }

        private void colorPanel_Click(object sender, EventArgs e)
        {
            _currentColor = _startColor;

            SetUI();
        }

        #endregion
    }
}
