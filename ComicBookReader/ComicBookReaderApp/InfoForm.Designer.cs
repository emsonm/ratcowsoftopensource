namespace cbr
{
  partial class InfoForm
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
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.titleEdit = new System.Windows.Forms.TextBox();
      this.seriesEdit = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.issueNumEdit = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.numOfIssuesEdit = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(169, 116);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(253, 116);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(27, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Title";
      // 
      // titleEdit
      // 
      this.titleEdit.Location = new System.Drawing.Point(96, 15);
      this.titleEdit.Name = "titleEdit";
      this.titleEdit.Size = new System.Drawing.Size(232, 20);
      this.titleEdit.TabIndex = 3;
      // 
      // seriesEdit
      // 
      this.seriesEdit.Location = new System.Drawing.Point(96, 41);
      this.seriesEdit.Name = "seriesEdit";
      this.seriesEdit.Size = new System.Drawing.Size(232, 20);
      this.seriesEdit.TabIndex = 5;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(36, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Series";
      // 
      // issueNumEdit
      // 
      this.issueNumEdit.Location = new System.Drawing.Point(96, 67);
      this.issueNumEdit.Name = "issueNumEdit";
      this.issueNumEdit.Size = new System.Drawing.Size(36, 20);
      this.issueNumEdit.TabIndex = 7;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 70);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Issue # ";
      // 
      // numOfIssuesEdit
      // 
      this.numOfIssuesEdit.Location = new System.Drawing.Point(202, 67);
      this.numOfIssuesEdit.Name = "numOfIssuesEdit";
      this.numOfIssuesEdit.Size = new System.Drawing.Size(42, 20);
      this.numOfIssuesEdit.TabIndex = 9;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(151, 73);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(16, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "of";
      // 
      // InfoForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(340, 154);
      this.Controls.Add(this.numOfIssuesEdit);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.issueNumEdit);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.seriesEdit);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.titleEdit);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "InfoForm";
      this.Text = "InfoForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox titleEdit;
    private System.Windows.Forms.TextBox seriesEdit;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox issueNumEdit;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox numOfIssuesEdit;
    private System.Windows.Forms.Label label4;
  }
}