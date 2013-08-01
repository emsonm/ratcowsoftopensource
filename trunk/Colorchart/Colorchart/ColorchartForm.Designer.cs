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

namespace RatCow.Colorchart
{
    partial class ColorchartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorchartForm));
            this.largeImages = new System.Windows.Forms.ImageList(this.components);
            this.smallImages = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbgValue = new System.Windows.Forms.Label();
            this.sortButton = new System.Windows.Forms.Button();
            this.detailButton = new System.Windows.Forms.Button();
            this.controlImages = new System.Windows.Forms.ImageList(this.components);
            this.smallViewButton = new System.Windows.Forms.Button();
            this.listButton = new System.Windows.Forms.Button();
            this.largeViewButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.colorsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // largeImages
            // 
            this.largeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeImages.ImageSize = new System.Drawing.Size(32, 32);
            this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallImages
            // 
            this.smallImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.smallImages.ImageSize = new System.Drawing.Size(8, 8);
            this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rbgValue);
            this.panel1.Controls.Add(this.sortButton);
            this.panel1.Controls.Add(this.detailButton);
            this.panel1.Controls.Add(this.smallViewButton);
            this.panel1.Controls.Add(this.listButton);
            this.panel1.Controls.Add(this.largeViewButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 32);
            this.panel1.TabIndex = 1;
            // 
            // rbgValue
            // 
            this.rbgValue.AutoSize = true;
            this.rbgValue.Location = new System.Drawing.Point(148, 9);
            this.rbgValue.Name = "rbgValue";
            this.rbgValue.Size = new System.Drawing.Size(16, 13);
            this.rbgValue.TabIndex = 5;
            this.rbgValue.Text = "...";
            // 
            // sortButton
            // 
            this.sortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sortButton.BackColor = System.Drawing.Color.Gainsboro;
            this.sortButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.sortButton.Font = new System.Drawing.Font("Agency FB", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortButton.Location = new System.Drawing.Point(633, 3);
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(46, 23);
            this.sortButton.TabIndex = 4;
            this.sortButton.Text = "Alpha";
            this.sortButton.UseVisualStyleBackColor = false;
            this.sortButton.Click += new System.EventHandler(this.sortButton_Click);
            // 
            // detailButton
            // 
            this.detailButton.BackColor = System.Drawing.Color.Gainsboro;
            this.detailButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.detailButton.ImageIndex = 3;
            this.detailButton.ImageList = this.controlImages;
            this.detailButton.Location = new System.Drawing.Point(108, 3);
            this.detailButton.Name = "detailButton";
            this.detailButton.Size = new System.Drawing.Size(29, 23);
            this.detailButton.TabIndex = 3;
            this.detailButton.UseVisualStyleBackColor = false;
            this.detailButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // controlImages
            // 
            this.controlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("controlImages.ImageStream")));
            this.controlImages.TransparentColor = System.Drawing.Color.Transparent;
            this.controlImages.Images.SetKeyName(0, "lv1.ico");
            this.controlImages.Images.SetKeyName(1, "lv2.ico");
            this.controlImages.Images.SetKeyName(2, "lv3.ico");
            this.controlImages.Images.SetKeyName(3, "lv4.ico");
            // 
            // smallViewButton
            // 
            this.smallViewButton.BackColor = System.Drawing.Color.Gainsboro;
            this.smallViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.smallViewButton.ImageIndex = 1;
            this.smallViewButton.ImageList = this.controlImages;
            this.smallViewButton.Location = new System.Drawing.Point(38, 3);
            this.smallViewButton.Name = "smallViewButton";
            this.smallViewButton.Size = new System.Drawing.Size(29, 23);
            this.smallViewButton.TabIndex = 2;
            this.smallViewButton.UseVisualStyleBackColor = false;
            this.smallViewButton.Click += new System.EventHandler(this.smallViewButton_Click);
            // 
            // listButton
            // 
            this.listButton.BackColor = System.Drawing.Color.Gainsboro;
            this.listButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.listButton.ImageIndex = 2;
            this.listButton.ImageList = this.controlImages;
            this.listButton.Location = new System.Drawing.Point(73, 3);
            this.listButton.Name = "listButton";
            this.listButton.Size = new System.Drawing.Size(29, 23);
            this.listButton.TabIndex = 1;
            this.listButton.UseVisualStyleBackColor = false;
            this.listButton.Click += new System.EventHandler(this.listButton_Click);
            // 
            // largeViewButton
            // 
            this.largeViewButton.BackColor = System.Drawing.Color.Gainsboro;
            this.largeViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.largeViewButton.ImageIndex = 0;
            this.largeViewButton.ImageList = this.controlImages;
            this.largeViewButton.Location = new System.Drawing.Point(3, 3);
            this.largeViewButton.Name = "largeViewButton";
            this.largeViewButton.Size = new System.Drawing.Size(29, 23);
            this.largeViewButton.TabIndex = 0;
            this.largeViewButton.UseVisualStyleBackColor = false;
            this.largeViewButton.Click += new System.EventHandler(this.largeViewButton_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.colorsListView);
            this.panel2.Location = new System.Drawing.Point(3, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(680, 360);
            this.panel2.TabIndex = 2;
            // 
            // colorsListView
            // 
            this.colorsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.colorsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorsListView.LargeImageList = this.largeImages;
            this.colorsListView.Location = new System.Drawing.Point(0, 0);
            this.colorsListView.Name = "colorsListView";
            this.colorsListView.Size = new System.Drawing.Size(680, 360);
            this.colorsListView.SmallImageList = this.smallImages;
            this.colorsListView.TabIndex = 1;
            this.colorsListView.UseCompatibleStateImageBehavior = false;
            this.colorsListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.colorsListView_ItemSelectionChanged);
            this.colorsListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.colorsListView_RetrieveVirtualItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Detail";
            this.columnHeader1.Width = 250;
            // 
            // ColorchartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 397);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ColorchartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNet Color explorer";
            this.Load += new System.EventHandler(this.ColorchartForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList largeImages;
        private System.Windows.Forms.ImageList smallImages;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button detailButton;
        private System.Windows.Forms.ImageList controlImages;
        private System.Windows.Forms.Button smallViewButton;
        private System.Windows.Forms.Button listButton;
        private System.Windows.Forms.Button largeViewButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView colorsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button sortButton;
        private System.Windows.Forms.Label rbgValue;

    }
}

