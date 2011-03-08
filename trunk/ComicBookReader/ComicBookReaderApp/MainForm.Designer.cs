namespace cbr
{
  partial class MainForm
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
      this.openButton = new System.Windows.Forms.Button();
      this.pageList = new System.Windows.Forms.ListBox();
      this.pageImage = new System.Windows.Forms.PictureBox();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.pageImage)).BeginInit();
      this.SuspendLayout();
      // 
      // openButton
      // 
      this.openButton.Location = new System.Drawing.Point(12, 12);
      this.openButton.Name = "openButton";
      this.openButton.Size = new System.Drawing.Size(75, 23);
      this.openButton.TabIndex = 7;
      this.openButton.Text = "Open file...";
      this.openButton.UseVisualStyleBackColor = true;
      // 
      // pageList
      // 
      this.pageList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.pageList.DisplayMember = "Filename";
      this.pageList.FormattingEnabled = true;
      this.pageList.HorizontalScrollbar = true;
      this.pageList.Location = new System.Drawing.Point(12, 64);
      this.pageList.Name = "pageList";
      this.pageList.Size = new System.Drawing.Size(222, 355);
      this.pageList.Sorted = true;
      this.pageList.TabIndex = 6;
      this.pageList.ValueMember = "RawData";
      // 
      // pageImage
      // 
      this.pageImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.pageImage.Location = new System.Drawing.Point(250, 6);
      this.pageImage.Name = "pageImage";
      this.pageImage.Size = new System.Drawing.Size(275, 432);
      this.pageImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pageImage.TabIndex = 5;
      this.pageImage.TabStop = false;
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(534, 452);
      this.Controls.Add(this.openButton);
      this.Controls.Add(this.pageList);
      this.Controls.Add(this.pageImage);
      this.Name = "MainForm";
      this.Text = "Comic Book Viewer Test";
      ((System.ComponentModel.ISupportInitialize)(this.pageImage)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button openButton;
    private System.Windows.Forms.ListBox pageList;
    private System.Windows.Forms.PictureBox pageImage;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
  }
}