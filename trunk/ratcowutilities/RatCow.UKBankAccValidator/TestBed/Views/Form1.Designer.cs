namespace MVCApplication1
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.button1 = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(67, 84);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "test";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(67, 32);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(96, 20);
      this.textBox1.TabIndex = 1;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(67, 58);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(149, 20);
      this.textBox2.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(5, 35);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Sortcode";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(5, 61);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(47, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Account";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(235, 121);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.button1);
      this.Name = "Form1";
      this.Text = "Test bank acc";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}