﻿
namespace XeviousPlayer2
{
    partial class Listas
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
            this.lsLista = new System.Windows.Forms.ListBox();
            this.btAcionar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lsLista
            // 
            this.lsLista.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsLista.FormattingEnabled = true;
            this.lsLista.ItemHeight = 25;
            this.lsLista.Location = new System.Drawing.Point(12, 12);
            this.lsLista.Name = "lsLista";
            this.lsLista.Size = new System.Drawing.Size(270, 579);
            this.lsLista.TabIndex = 0;
            this.lsLista.DoubleClick += new System.EventHandler(this.lsLista_DoubleClick);
            // 
            // btAcionar
            // 
            this.btAcionar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAcionar.Location = new System.Drawing.Point(64, 616);
            this.btAcionar.Name = "btAcionar";
            this.btAcionar.Size = new System.Drawing.Size(75, 23);
            this.btAcionar.TabIndex = 1;
            this.btAcionar.Text = "Acionar";
            this.btAcionar.UseVisualStyleBackColor = true;
            this.btAcionar.Click += new System.EventHandler(this.btAcionar_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(158, 616);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancelar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Listas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 651);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btAcionar);
            this.Controls.Add(this.lsLista);
            this.MaximizeBox = false;
            this.Name = "Listas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista";
            this.Load += new System.EventHandler(this.Listas_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsLista;
        private System.Windows.Forms.Button btAcionar;
        private System.Windows.Forms.Button button1;
    }
}