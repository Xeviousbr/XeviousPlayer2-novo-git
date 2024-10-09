
namespace XeviousPlayer2
{
    partial class Full
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackBarFull = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFull)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 2;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // trackBarFull
            // 
            this.trackBarFull.Location = new System.Drawing.Point(22, 203);
            this.trackBarFull.Name = "trackBarFull";
            this.trackBarFull.Size = new System.Drawing.Size(757, 45);
            this.trackBarFull.TabIndex = 3;
            this.trackBarFull.TickFrequency = 10000;
            this.trackBarFull.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarFull.Visible = false;
            // 
            // Full
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.trackBarFull);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Full";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Full";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Full_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFull)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar trackBarFull;
    }
}