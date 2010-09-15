/*
 * Copyright 2010 Rat Cow Software and Matt Emson. All rights reserved.
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

namespace Outliner
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
      this.components = new System.ComponentModel.Container();
      this.panel1 = new System.Windows.Forms.Panel();
      this.mainView = new System.Windows.Forms.TreeView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.insertAtBeginningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.insertBeforeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.insertAfterSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.insertAtEnddefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addButton = new System.Windows.Forms.Button();
      this.removeButton = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.mainView);
      this.panel1.Location = new System.Drawing.Point(1, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(528, 285);
      this.panel1.TabIndex = 0;
      // 
      // mainView
      // 
      this.mainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.mainView.ContextMenuStrip = this.contextMenuStrip1;
      this.mainView.HideSelection = false;
      this.mainView.LabelEdit = true;
      this.mainView.Location = new System.Drawing.Point(3, 3);
      this.mainView.Name = "mainView";
      this.mainView.Size = new System.Drawing.Size(520, 276);
      this.mainView.TabIndex = 0;
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertAtBeginningToolStripMenuItem,
            this.insertBeforeToolStripMenuItem,
            this.insertAfterSelectedToolStripMenuItem,
            this.insertAtEnddefaultToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(193, 114);
      // 
      // insertAtBeginningToolStripMenuItem
      // 
      this.insertAtBeginningToolStripMenuItem.Name = "insertAtBeginningToolStripMenuItem";
      this.insertAtBeginningToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.insertAtBeginningToolStripMenuItem.Text = "Add...";
      this.insertAtBeginningToolStripMenuItem.Click += new System.EventHandler(this.insertAtBeginningToolStripMenuItem_Click);
      // 
      // insertBeforeToolStripMenuItem
      // 
      this.insertBeforeToolStripMenuItem.Name = "insertBeforeToolStripMenuItem";
      this.insertBeforeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.insertBeforeToolStripMenuItem.Text = "Insert before selected";
      this.insertBeforeToolStripMenuItem.Click += new System.EventHandler(this.insertBeforeToolStripMenuItem_Click);
      // 
      // insertAfterSelectedToolStripMenuItem
      // 
      this.insertAfterSelectedToolStripMenuItem.Name = "insertAfterSelectedToolStripMenuItem";
      this.insertAfterSelectedToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.insertAfterSelectedToolStripMenuItem.Text = "Insert after selected";
      this.insertAfterSelectedToolStripMenuItem.Click += new System.EventHandler(this.insertAfterSelectedToolStripMenuItem_Click);
      // 
      // insertAtEnddefaultToolStripMenuItem
      // 
      this.insertAtEnddefaultToolStripMenuItem.Name = "insertAtEnddefaultToolStripMenuItem";
      this.insertAtEnddefaultToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.insertAtEnddefaultToolStripMenuItem.Text = "Add child...";
      this.insertAtEnddefaultToolStripMenuItem.Click += new System.EventHandler(this.addButton_Click);
      // 
      // addButton
      // 
      this.addButton.Location = new System.Drawing.Point(5, 292);
      this.addButton.Name = "addButton";
      this.addButton.Size = new System.Drawing.Size(75, 23);
      this.addButton.TabIndex = 1;
      this.addButton.Text = "Add";
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new System.EventHandler(this.addButton_Click);
      // 
      // removeButton
      // 
      this.removeButton.Location = new System.Drawing.Point(87, 292);
      this.removeButton.Name = "removeButton";
      this.removeButton.Size = new System.Drawing.Size(75, 23);
      this.removeButton.TabIndex = 2;
      this.removeButton.Text = "Remove";
      this.removeButton.UseVisualStyleBackColor = true;
      this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(529, 320);
      this.Controls.Add(this.removeButton);
      this.Controls.Add(this.addButton);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "MainForm";
      this.Text = "Outliner";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.panel1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TreeView mainView;
    private System.Windows.Forms.Button addButton;
    private System.Windows.Forms.Button removeButton;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem insertBeforeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem insertAfterSelectedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem insertAtBeginningToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem insertAtEnddefaultToolStripMenuItem;
  }
}

