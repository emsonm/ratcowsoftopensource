namespace testapp2
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
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.button1 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.listView1 = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
      this.checkBox2 = new System.Windows.Forms.CheckBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel2 = new System.Windows.Forms.Panel();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.checkBox3 = new System.Windows.Forms.CheckBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
      this.button4 = new System.Windows.Forms.Button();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(13, 85);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(166, 17);
      this.checkBox1.TabIndex = 0;
      this.checkBox1.Text = "Change label to textbox value";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(189, 81);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Alert!";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 57);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "This is a label";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(13, 24);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(251, 20);
      this.textBox1.TabIndex = 3;
      this.textBox1.Text = "This is an text box";
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listView1.Location = new System.Drawing.Point(15, 141);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(249, 146);
      this.listView1.TabIndex = 4;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // checkBox2
      // 
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new System.Drawing.Point(12, 118);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new System.Drawing.Size(135, 17);
      this.checkBox2.TabIndex = 5;
      this.checkBox2.Text = "Use Form1Data source";
      this.checkBox2.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.Silver;
      this.panel1.Controls.Add(this.textBox3);
      this.panel1.Controls.Add(this.button4);
      this.panel1.Controls.Add(this.dateTimePicker1);
      this.panel1.Controls.Add(this.panel2);
      this.panel1.Location = new System.Drawing.Point(271, 24);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(200, 263);
      this.panel1.TabIndex = 6;
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.DimGray;
      this.panel2.Controls.Add(this.tabControl1);
      this.panel2.Location = new System.Drawing.Point(3, 131);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(194, 128);
      this.panel2.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Location = new System.Drawing.Point(3, 3);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(188, 122);
      this.tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.textBox2);
      this.tabPage1.Controls.Add(this.button2);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(180, 96);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "tabPage1";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.checkBox3);
      this.tabPage2.Controls.Add(this.button3);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(180, 96);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(7, 21);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(167, 23);
      this.button2.TabIndex = 0;
      this.button2.Text = "button2";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(17, 17);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(150, 23);
      this.button3.TabIndex = 0;
      this.button3.Text = "button3";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // checkBox3
      // 
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new System.Drawing.Point(17, 59);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new System.Drawing.Size(80, 17);
      this.checkBox3.TabIndex = 1;
      this.checkBox3.Text = "checkBox3";
      this.checkBox3.UseVisualStyleBackColor = true;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(7, 51);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(167, 20);
      this.textBox2.TabIndex = 1;
      // 
      // dateTimePicker1
      // 
      this.dateTimePicker1.Location = new System.Drawing.Point(17, 94);
      this.dateTimePicker1.Name = "dateTimePicker1";
      this.dateTimePicker1.Size = new System.Drawing.Size(173, 20);
      this.dateTimePicker1.TabIndex = 1;
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(17, 16);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(173, 23);
      this.button4.TabIndex = 2;
      this.button4.Text = "button4";
      this.button4.UseVisualStyleBackColor = true;
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(17, 57);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(173, 20);
      this.textBox3.TabIndex = 3;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(485, 301);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.checkBox2);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.checkBox1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.CheckBox checkBox2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.CheckBox checkBox3;
    private System.Windows.Forms.Button button3;
  }
}

