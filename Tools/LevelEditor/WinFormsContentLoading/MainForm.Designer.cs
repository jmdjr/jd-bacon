namespace WinFormsContentLoading
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelViewerControl = new WinFormsContentLoading.ModelViewerControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenMenuClicked);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClicked);
            // 
            // modelViewerControl
            // 
            this.modelViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelViewerControl.Location = new System.Drawing.Point(0, 24);
            this.modelViewerControl.Model = null;
            this.modelViewerControl.Name = "modelViewerControl";
            this.modelViewerControl.PosX = 0;
            this.modelViewerControl.PosY = 0;
            this.modelViewerControl.PosZ = 0;
            this.modelViewerControl.RotX = 0F;
            this.modelViewerControl.RotY = 0F;
            this.modelViewerControl.Size = new System.Drawing.Size(792, 549);
            this.modelViewerControl.TabIndex = 1;
            this.modelViewerControl.Text = "modelViewerControl";
            this.modelViewerControl.Click += new System.EventHandler(this.modelViewerControl_Click);
            this.modelViewerControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modelViewerControl_KeyDown);
            this.modelViewerControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.modelViewerControl_KeyPress);
            this.modelViewerControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.modelViewerControl_KeyUp);
            this.modelViewerControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelViewerControl_MouseDown);
            this.modelViewerControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelViewerControl_MouseMove);
            this.modelViewerControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelViewerControl_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.modelViewerControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "WinForms Content Loading";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private ModelViewerControl modelViewerControl;

    }
}

