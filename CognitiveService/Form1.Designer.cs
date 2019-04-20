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
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.detectedFacesLbl = new System.Windows.Forms.Label();
            this.nameLbl = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.beginBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imageBox = new Emgu.CV.UI.ImageBox();
            this.facesReconizedTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.detectedFacesLbl);
            this.panel1.Controls.Add(this.nameLbl);
            this.panel1.Controls.Add(this.nameBox);
            this.panel1.Controls.Add(this.saveBtn);
            this.panel1.Controls.Add(this.beginBtn);
            this.panel1.Location = new System.Drawing.Point(1102, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 635);
            this.panel1.TabIndex = 3;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(17, 152);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(300, 380);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
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
            this.beginBtn.Location = new System.Drawing.Point(17, 538);
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
            this.panel2.Size = new System.Drawing.Size(1084, 635);
            this.panel2.TabIndex = 4;
            // 
            // imageBox
            // 
            this.imageBox.Location = new System.Drawing.Point(3, 3);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(1078, 629);
            this.imageBox.TabIndex = 3;
            this.imageBox.TabStop = false;
            this.imageBox.Click += new System.EventHandler(this.ImageBox_Click);
            // 
            // facesReconizedTimer
            // 
            this.facesReconizedTimer.Enabled = true;
            this.facesReconizedTimer.Interval = 1000;
            this.facesReconizedTimer.Tick += new System.EventHandler(this.FacesReconizedTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1444, 659);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Face Recognition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Leave += new System.EventHandler(this.Form1_Leave);
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
        private System.Windows.Forms.Label detectedFacesLbl;
        private System.Windows.Forms.Timer facesReconizedTimer;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

