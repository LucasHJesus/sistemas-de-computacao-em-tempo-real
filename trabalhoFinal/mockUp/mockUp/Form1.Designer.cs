namespace mockUp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNumeroDePacotes = new Label();
            nudTotalDePacotes = new NumericUpDown();
            gbModoDeGeracao = new GroupBox();
            rbFurtoDeEnergia = new RadioButton();
            rbFaltaDeEnergia = new RadioButton();
            rbNormal = new RadioButton();
            nudPacotesFalta = new NumericUpDown();
            lblPacotesFalta = new Label();
            lblPacoteslFurto = new Label();
            nudPacoteslFurto = new NumericUpDown();
            btnGerar = new Button();
            btnParar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudTotalDePacotes).BeginInit();
            gbModoDeGeracao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPacotesFalta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPacoteslFurto).BeginInit();
            SuspendLayout();
            // 
            // lblNumeroDePacotes
            // 
            lblNumeroDePacotes.AutoSize = true;
            lblNumeroDePacotes.Location = new Point(28, 166);
            lblNumeroDePacotes.Name = "lblNumeroDePacotes";
            lblNumeroDePacotes.Size = new Size(175, 20);
            lblNumeroDePacotes.TabIndex = 0;
            lblNumeroDePacotes.Text = "Numero de pacotes total";
            // 
            // nudTotalDePacotes
            // 
            nudTotalDePacotes.Location = new Point(28, 189);
            nudTotalDePacotes.Maximum = new decimal(new int[] { 300000, 0, 0, 0 });
            nudTotalDePacotes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudTotalDePacotes.Name = "nudTotalDePacotes";
            nudTotalDePacotes.Size = new Size(175, 27);
            nudTotalDePacotes.TabIndex = 1;
            nudTotalDePacotes.Value = new decimal(new int[] { 30000, 0, 0, 0 });
            // 
            // gbModoDeGeracao
            // 
            gbModoDeGeracao.Controls.Add(rbFurtoDeEnergia);
            gbModoDeGeracao.Controls.Add(rbFaltaDeEnergia);
            gbModoDeGeracao.Controls.Add(rbNormal);
            gbModoDeGeracao.Location = new Point(28, 12);
            gbModoDeGeracao.Name = "gbModoDeGeracao";
            gbModoDeGeracao.Size = new Size(175, 137);
            gbModoDeGeracao.TabIndex = 2;
            gbModoDeGeracao.TabStop = false;
            gbModoDeGeracao.Text = "Modo de geração";
            // 
            // rbFurtoDeEnergia
            // 
            rbFurtoDeEnergia.AutoSize = true;
            rbFurtoDeEnergia.Location = new Point(6, 86);
            rbFurtoDeEnergia.Name = "rbFurtoDeEnergia";
            rbFurtoDeEnergia.Size = new Size(139, 24);
            rbFurtoDeEnergia.TabIndex = 2;
            rbFurtoDeEnergia.TabStop = true;
            rbFurtoDeEnergia.Text = "Furto de energia";
            rbFurtoDeEnergia.UseVisualStyleBackColor = true;
            rbFurtoDeEnergia.CheckedChanged += rbFurtoDeEnergia_CheckedChanged;
            // 
            // rbFaltaDeEnergia
            // 
            rbFaltaDeEnergia.AutoSize = true;
            rbFaltaDeEnergia.Location = new Point(6, 56);
            rbFaltaDeEnergia.Name = "rbFaltaDeEnergia";
            rbFaltaDeEnergia.Size = new Size(136, 24);
            rbFaltaDeEnergia.TabIndex = 1;
            rbFaltaDeEnergia.TabStop = true;
            rbFaltaDeEnergia.Text = "Falta de energia";
            rbFaltaDeEnergia.UseVisualStyleBackColor = true;
            rbFaltaDeEnergia.CheckedChanged += rbFaltaDeEnergia_CheckedChanged;
            // 
            // rbNormal
            // 
            rbNormal.AutoSize = true;
            rbNormal.Location = new Point(6, 26);
            rbNormal.Name = "rbNormal";
            rbNormal.Size = new Size(80, 24);
            rbNormal.TabIndex = 0;
            rbNormal.TabStop = true;
            rbNormal.Text = "Normal";
            rbNormal.UseVisualStyleBackColor = true;
            rbNormal.CheckedChanged += rbNormal_CheckedChanged;
            // 
            // nudPacotesFalta
            // 
            nudPacotesFalta.Location = new Point(28, 285);
            nudPacotesFalta.Maximum = new decimal(new int[] { 300000, 0, 0, 0 });
            nudPacotesFalta.Name = "nudPacotesFalta";
            nudPacotesFalta.Size = new Size(175, 27);
            nudPacotesFalta.TabIndex = 3;
            nudPacotesFalta.Value = new decimal(new int[] { 300, 0, 0, 0 });
            // 
            // lblPacotesFalta
            // 
            lblPacotesFalta.AutoSize = true;
            lblPacotesFalta.Location = new Point(28, 240);
            lblPacotesFalta.Name = "lblPacotesFalta";
            lblPacotesFalta.Size = new Size(177, 40);
            lblPacotesFalta.TabIndex = 4;
            lblPacotesFalta.Text = "Numero de pacotes com \r\nfalta de energia";
            // 
            // lblPacoteslFurto
            // 
            lblPacoteslFurto.AutoSize = true;
            lblPacoteslFurto.Location = new Point(28, 334);
            lblPacoteslFurto.Name = "lblPacoteslFurto";
            lblPacoteslFurto.Size = new Size(177, 40);
            lblPacoteslFurto.TabIndex = 6;
            lblPacoteslFurto.Text = "Numero de pacotes com \r\nroubo de energia";
            // 
            // nudPacoteslFurto
            // 
            nudPacoteslFurto.Location = new Point(28, 379);
            nudPacoteslFurto.Maximum = new decimal(new int[] { 300000, 0, 0, 0 });
            nudPacoteslFurto.Name = "nudPacoteslFurto";
            nudPacoteslFurto.Size = new Size(175, 27);
            nudPacoteslFurto.TabIndex = 7;
            nudPacoteslFurto.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // btnGerar
            // 
            btnGerar.Location = new Point(28, 460);
            btnGerar.Name = "btnGerar";
            btnGerar.Size = new Size(87, 29);
            btnGerar.TabIndex = 8;
            btnGerar.Text = "Gerar Pacotes";
            btnGerar.UseVisualStyleBackColor = true;
            btnGerar.Click += btnGerar_Click;
            // 
            // btnParar
            // 
            btnParar.Location = new Point(116, 460);
            btnParar.Name = "btnParar";
            btnParar.Size = new Size(87, 29);
            btnParar.TabIndex = 9;
            btnParar.Text = "Parar";
            btnParar.UseVisualStyleBackColor = true;
            btnParar.Click += btnParar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(251, 513);
            Controls.Add(btnParar);
            Controls.Add(btnGerar);
            Controls.Add(nudPacoteslFurto);
            Controls.Add(lblPacoteslFurto);
            Controls.Add(lblPacotesFalta);
            Controls.Add(nudPacotesFalta);
            Controls.Add(gbModoDeGeracao);
            Controls.Add(nudTotalDePacotes);
            Controls.Add(lblNumeroDePacotes);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)nudTotalDePacotes).EndInit();
            gbModoDeGeracao.ResumeLayout(false);
            gbModoDeGeracao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPacotesFalta).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPacoteslFurto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNumeroDePacotes;
        private NumericUpDown nudTotalDePacotes;
        private GroupBox gbModoDeGeracao;
        private RadioButton rbFurtoDeEnergia;
        private RadioButton rbFaltaDeEnergia;
        private RadioButton rbNormal;
        private NumericUpDown nudPacotesFalta;
        private Label lblPacotesFalta;
        private Label lblPacoteslFurto;
        private NumericUpDown nudPacoteslFurto;
        private Button btnGerar;
        private Button btnParar;
    }
}
