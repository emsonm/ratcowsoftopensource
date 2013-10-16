namespace RatCow.MvcFramework.MvcMapTool
{
  partial class AddGeneralItemForm
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
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.basedOnCombo = new System.Windows.Forms.ComboBox();
      this.eventCombo = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(167, 93);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(248, 93);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // basedOnCombo
      // 
      this.basedOnCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.basedOnCombo.FormattingEnabled = true;
      this.basedOnCombo.Location = new System.Drawing.Point(83, 12);
      this.basedOnCombo.Name = "basedOnCombo";
      this.basedOnCombo.Size = new System.Drawing.Size(240, 21);
      this.basedOnCombo.Sorted = true;
      this.basedOnCombo.TabIndex = 2;
      // 
      // eventCombo
      // 
      this.eventCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.eventCombo.FormattingEnabled = true;
      this.eventCombo.Location = new System.Drawing.Point(83, 39);
      this.eventCombo.Name = "eventCombo";
      this.eventCombo.Size = new System.Drawing.Size(240, 21);
      this.eventCombo.Sorted = true;
      this.eventCombo.TabIndex = 3;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Based on...";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Event";
      // 
      // AddGeneralItemForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(334, 128);
      this.ControlBox = false;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.eventCombo);
      this.Controls.Add(this.basedOnCombo);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "AddGeneralItemForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add...";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.ComboBox basedOnCombo;
    private System.Windows.Forms.ComboBox eventCombo;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}