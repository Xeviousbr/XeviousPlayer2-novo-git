namespace XeviousPlayer2
{
    partial class Overlay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.subtitlesLabel = new XeviousPlayer2.BlendLabel();
            this.flashLabel = new XeviousPlayer2.BlendLabel();
            this.SuspendLayout();
            // 
            // subtitlesLabel
            // 
            this.subtitlesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitlesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitlesLabel.ForeColor = System.Drawing.Color.Yellow;
            this.subtitlesLabel.Location = new System.Drawing.Point(-4, 125);
            this.subtitlesLabel.Name = "subtitlesLabel";
            this.subtitlesLabel.Size = new System.Drawing.Size(302, 34);
            this.subtitlesLabel.TabIndex = 3;
            this.subtitlesLabel.Text = "Subtitles";
            this.subtitlesLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // flashLabel
            // 
            this.flashLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flashLabel.ForeColor = System.Drawing.Color.Red;
            this.flashLabel.Location = new System.Drawing.Point(0, 5);
            this.flashLabel.Name = "flashLabel";
            this.flashLabel.Size = new System.Drawing.Size(293, 158);
            this.flashLabel.TabIndex = 2;
            this.flashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Overlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(294, 172);
            this.Controls.Add(this.subtitlesLabel);
            this.Controls.Add(this.flashLabel);
            this.Name = "Overlay";
            this.Text = "Display Overlay";
            this.VisibleChanged += new System.EventHandler(this.Overlay_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private BlendLabel flashLabel;
        internal BlendLabel subtitlesLabel;
    }
}