namespace CognitiveService
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.detectedFaceslistBox = new System.Windows.Forms.ListBox();
            this.detectedFacesLbl = new System.Windows.Forms.Label();
            this.nameLbl = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.beginBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imageBox = new Emgu.CV.UI.ImageBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.detectedFaceslistBox);
            this.panel1.Controls.Add(this.detectedFacesLbl);
            this.panel1.Controls.Add(this.nameLbl);
            this.panel1.Controls.Add(this.nameBox);
            this.panel1.Controls.Add(this.saveBtn);
            this.panel1.Controls.Add(this.beginBtn);
            this.panel1.Location = new System.Drawing.Point(584, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 530);
            this.panel1.TabIndex = 3;
            // 
            // detectedFaceslistBox
            // 
            this.detectedFaceslistBox.FormattingEnabled = true;
            this.detectedFaceslistBox.ItemHeight = 16;
            this.detectedFaceslistBox.Location = new System.Drawing.Point(17, 160);
            this.detectedFaceslistBox.Name = "detectedFaceslistBox";
            this.detectedFaceslistBox.Size = new System.Drawing.Size(300, 260);
            this.detectedFaceslistBox.TabIndex = 8;
            // 
            // detectedFacesLbl
            // 
            this.detectedFacesLbl.AutoSize = true;
            this.detectedFacesLbl.Location = new System.Drawing.Point(14, 131);
            this.detectedFacesLbl.Name = "detectedFacesLbl";
            this.detectedFacesLbl.Size = new System.Drawing.Size(145, 17);
            this.detectedFacesLbl.TabIndex = 7;
            this.detectedFacesLbl.Text = "Rostos reconhecidos:";
            // 
            // nameLbl
            // 
            this.nameLbl.AutoSize = true;
            this.nameLbl.Location = new System.Drawing.Point(14, 16);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(161, 17);
            this.nameLbl.TabIndex = 7;
            this.nameLbl.Text = "Nome para identificação";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(17, 36);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(300, 22);
            this.nameBox.TabIndex = 6;
            // 
            // saveBtn
            // 
            this.saveBtn.BackColor = System.Drawing.Color.Green;
            this.saveBtn.FlatAppearance.BorderSize = 0;
            this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBtn.ForeColor = System.Drawing.Color.White;
            this.saveBtn.Location = new System.Drawing.Point(17, 64);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(300, 40);
            this.saveBtn.TabIndex = 4;
            this.saveBtn.Text = "Recognize face";
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // beginBtn
            // 
            this.beginBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.beginBtn.FlatAppearance.BorderSize = 0;
            this.beginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.beginBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beginBtn.ForeColor = System.Drawing.Color.White;
            this.beginBtn.Location = new System.Drawing.Point(17, 436);
            this.beginBtn.Name = "beginBtn";
            this.beginBtn.Size = new System.Drawing.Size(300, 78);
            this.beginBtn.TabIndex = 5;
            this.beginBtn.Text = "Iniciar captura";
            this.beginBtn.UseVisualStyleBackColor = false;
            this.beginBtn.Click += new System.EventHandler(this.BeginBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.imageBox);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(566, 530);
            this.panel2.TabIndex = 4;
            // 
            // imageBox
            // 
            this.imageBox.Location = new System.Drawing.Point(3, 3);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(557, 524);
            this.imageBox.TabIndex = 3;
            this.imageBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(927, 554);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Face Recognition";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button beginBtn;
        private System.Windows.Forms.Panel panel2;
        private Emgu.CV.UI.ImageBox imageBox;
        private System.Windows.Forms.ListBox detectedFaceslistBox;
        private System.Windows.Forms.Label detectedFacesLbl;
    }
}

