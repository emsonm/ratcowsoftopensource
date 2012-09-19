namespace RatCow.MvcFramework.MvcMapTool
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
      this.globalEventsListView = new System.Windows.Forms.ListView();
      this.controlsListView = new System.Windows.Forms.ListView();
      this.controlDetailListView = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.addGeneralButton = new System.Windows.Forms.Button();
      this.removeGeneralButton = new System.Windows.Forms.Button();
      this.removeEvent = new System.Windows.Forms.Button();
      this.addEvent = new System.Windows.Forms.Button();
      this.removeControl = new System.Windows.Forms.Button();
      this.addControl = new System.Windows.Forms.Button();
      this.saveButton = new System.Windows.Forms.Button();
      this.closeButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // globalEventsListView
      // 
      this.globalEventsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.globalEventsListView.FullRowSelect = true;
      this.globalEventsListView.Location = new System.Drawing.Point(26, 12);
      this.globalEventsListView.MultiSelect = false;
      this.globalEventsListView.Name = "globalEventsListView";
      this.globalEventsListView.Size = new System.Drawing.Size(525, 127);
      this.globalEventsListView.TabIndex = 0;
      this.globalEventsListView.UseCompatibleStateImageBehavior = false;
      this.globalEventsListView.View = System.Windows.Forms.View.Details;
      // 
      // controlsListView
      // 
      this.controlsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
      this.controlsListView.FullRowSelect = true;
      this.controlsListView.Location = new System.Drawing.Point(26, 185);
      this.controlsListView.MultiSelect = false;
      this.controlsListView.Name = "controlsListView";
      this.controlsListView.Size = new System.Drawing.Size(128, 152);
      this.controlsListView.TabIndex = 1;
      this.controlsListView.UseCompatibleStateImageBehavior = false;
      this.controlsListView.View = System.Windows.Forms.View.Details;
      // 
      // controlDetailListView
      // 
      this.controlDetailListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
      this.controlDetailListView.FullRowSelect = true;
      this.controlDetailListView.Location = new System.Drawing.Point(184, 187);
      this.controlDetailListView.MultiSelect = false;
      this.controlDetailListView.Name = "controlDetailListView";
      this.controlDetailListView.Size = new System.Drawing.Size(367, 150);
      this.controlDetailListView.TabIndex = 2;
      this.controlDetailListView.UseCompatibleStateImageBehavior = false;
      this.controlDetailListView.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Event name";
      this.columnHeader1.Width = 150;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "EventType";
      this.columnHeader2.Width = 150;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "EventArg";
      this.columnHeader3.Width = 150;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Event name";
      this.columnHeader4.Width = 120;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "EventType";
      this.columnHeader5.Width = 120;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "EventArg";
      this.columnHeader6.Width = 110;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Control";
      this.columnHeader7.Width = 120;
      // 
      // addGeneralButton
      // 
      this.addGeneralButton.Location = new System.Drawing.Point(26, 145);
      this.addGeneralButton.Name = "addGeneralButton";
      this.addGeneralButton.Size = new System.Drawing.Size(75, 23);
      this.addGeneralButton.TabIndex = 3;
      this.addGeneralButton.Text = "Add";
      this.addGeneralButton.UseVisualStyleBackColor = true;
      // 
      // removeGeneralButton
      // 
      this.removeGeneralButton.Location = new System.Drawing.Point(108, 145);
      this.removeGeneralButton.Name = "removeGeneralButton";
      this.removeGeneralButton.Size = new System.Drawing.Size(75, 23);
      this.removeGeneralButton.TabIndex = 6;
      this.removeGeneralButton.Text = "Remove";
      this.removeGeneralButton.UseVisualStyleBackColor = true;
      // 
      // removeEvent
      // 
      this.removeEvent.Location = new System.Drawing.Point(266, 343);
      this.removeEvent.Name = "removeEvent";
      this.removeEvent.Size = new System.Drawing.Size(75, 23);
      this.removeEvent.TabIndex = 8;
      this.removeEvent.Text = "Remove";
      this.removeEvent.UseVisualStyleBackColor = true;
      // 
      // addEvent
      // 
      this.addEvent.Location = new System.Drawing.Point(184, 343);
      this.addEvent.Name = "addEvent";
      this.addEvent.Size = new System.Drawing.Size(75, 23);
      this.addEvent.TabIndex = 7;
      this.addEvent.Text = "Add";
      this.addEvent.UseVisualStyleBackColor = true;
      // 
      // removeControl
      // 
      this.removeControl.Location = new System.Drawing.Point(88, 343);
      this.removeControl.Name = "removeControl";
      this.removeControl.Size = new System.Drawing.Size(66, 23);
      this.removeControl.TabIndex = 10;
      this.removeControl.Text = "Remove";
      this.removeControl.UseVisualStyleBackColor = true;
      // 
      // addControl
      // 
      this.addControl.Location = new System.Drawing.Point(26, 343);
      this.addControl.Name = "addControl";
      this.addControl.Size = new System.Drawing.Size(56, 23);
      this.addControl.TabIndex = 9;
      this.addControl.Text = "Add";
      this.addControl.UseVisualStyleBackColor = true;
      // 
      // saveButton
      // 
      this.saveButton.Location = new System.Drawing.Point(415, 372);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(75, 23);
      this.saveButton.TabIndex = 11;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      // 
      // closeButton
      // 
      this.closeButton.Location = new System.Drawing.Point(496, 372);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(75, 23);
      this.closeButton.TabIndex = 12;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(584, 400);
      this.ControlBox = false;
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.saveButton);
      this.Controls.Add(this.removeControl);
      this.Controls.Add(this.addControl);
      this.Controls.Add(this.removeEvent);
      this.Controls.Add(this.addEvent);
      this.Controls.Add(this.removeGeneralButton);
      this.Controls.Add(this.addGeneralButton);
      this.Controls.Add(this.controlDetailListView);
      this.Controls.Add(this.controlsListView);
      this.Controls.Add(this.globalEventsListView);
      this.Name = "MainForm";
      this.Text = "Quick MVCMAP editor";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView globalEventsListView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ListView controlsListView;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ListView controlDetailListView;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.Button addGeneralButton;
    private System.Windows.Forms.Button removeGeneralButton;
    private System.Windows.Forms.Button removeEvent;
    private System.Windows.Forms.Button addEvent;
    private System.Windows.Forms.Button removeControl;
    private System.Windows.Forms.Button addControl;
    private System.Windows.Forms.Button saveButton;
    private System.Windows.Forms.Button closeButton;
  }
}