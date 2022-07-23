
namespace XeviousPlayer2
{
    partial class Importar
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
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txPasta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.cbListas = new System.Windows.Forms.ComboBox();
            this.txNovo = new System.Windows.Forms.TextBox();
            this.chZerar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(292, 87);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "Importar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(292, 47);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(27, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txPasta
            // 
            this.txPasta.Location = new System.Drawing.Point(15, 50);
            this.txPasta.Name = "txPasta";
            this.txPasta.Size = new System.Drawing.Size(274, 20);
            this.txPasta.TabIndex = 15;
            this.txPasta.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Caminho das Musicas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Lista";
            // 
            // cbListas
            // 
            this.cbListas.FormattingEnabled = true;
            this.cbListas.Location = new System.Drawing.Point(15, 89);
            this.cbListas.Name = "cbListas";
            this.cbListas.Size = new System.Drawing.Size(274, 21);
            this.cbListas.TabIndex = 19;
            this.cbListas.SelectedIndexChanged += new System.EventHandler(this.cbListas_SelectedIndexChanged);
            // 
            // txNovo
            // 
            this.txNovo.Location = new System.Drawing.Point(15, 90);
            this.txNovo.Name = "txNovo";
            this.txNovo.Size = new System.Drawing.Size(274, 20);
            this.txNovo.TabIndex = 20;
            this.txNovo.Visible = false;
            // 
            // chZerar
            // 
            this.chZerar.AutoSize = true;
            this.chZerar.Location = new System.Drawing.Point(15, 12);
            this.chZerar.Name = "chZerar";
            this.chZerar.Size = new System.Drawing.Size(133, 17);
            this.chZerar.TabIndex = 21;
            this.chZerar.Text = "Zerar a base de dados";
            this.chZerar.UseVisualStyleBackColor = true;
            // 
            // Importar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(379, 124);
            this.Controls.Add(this.chZerar);
            this.Controls.Add(this.cbListas);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txPasta);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txNovo);
            this.ForeColor = System.Drawing.SystemColors.GrayText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Importar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Importar";
            this.Load += new System.EventHandler(this.Importar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txPasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbListas;
        private System.Windows.Forms.TextBox txNovo;
        private System.Windows.Forms.CheckBox chZerar;
    }
}