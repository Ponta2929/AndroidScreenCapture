
namespace AndroidScreenCapture
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timer_Capture = new System.Windows.Forms.Timer(this.components);
            this.PictureBox_View = new System.Windows.Forms.PictureBox();
            this.ContextMenuStrip_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer_Swipe = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_View)).BeginInit();
            this.ContextMenuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer_Capture
            // 
            this.Timer_Capture.Enabled = true;
            this.Timer_Capture.Interval = 50;
            this.Timer_Capture.Tick += new System.EventHandler(this.Timer_Capture_Tick);
            // 
            // PictureBox_View
            // 
            this.PictureBox_View.ContextMenuStrip = this.ContextMenuStrip_Main;
            this.PictureBox_View.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PictureBox_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox_View.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_View.Name = "PictureBox_View";
            this.PictureBox_View.Size = new System.Drawing.Size(313, 450);
            this.PictureBox_View.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox_View.TabIndex = 0;
            this.PictureBox_View.TabStop = false;
            this.PictureBox_View.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_View_MouseDown);
            this.PictureBox_View.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_View_MouseMove);
            this.PictureBox_View.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_View_MouseUp);
            // 
            // ContextMenuStrip_Main
            // 
            this.ContextMenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Save});
            this.ContextMenuStrip_Main.Name = "ContextMenuStrip_Main";
            this.ContextMenuStrip_Main.Size = new System.Drawing.Size(113, 26);
            // 
            // ToolStripMenuItem_Save
            // 
            this.ToolStripMenuItem_Save.Name = "ToolStripMenuItem_Save";
            this.ToolStripMenuItem_Save.Size = new System.Drawing.Size(112, 22);
            this.ToolStripMenuItem_Save.Text = "保存(&S)";
            this.ToolStripMenuItem_Save.Click += new System.EventHandler(this.ToolStripMenuItem_Save_Click);
            // 
            // Timer_Swipe
            // 
            this.Timer_Swipe.Enabled = true;
            this.Timer_Swipe.Tick += new System.EventHandler(this.Timer_Swipe_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 450);
            this.Controls.Add(this.PictureBox_View);
            this.Name = "MainForm";
            this.Text = "AndroidScreenCapture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_View)).EndInit();
            this.ContextMenuStrip_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Timer_Capture;
        private System.Windows.Forms.PictureBox PictureBox_View;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Save;
        private System.Windows.Forms.Timer Timer_Swipe;
    }
}

