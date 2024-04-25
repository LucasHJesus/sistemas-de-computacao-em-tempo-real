namespace Pratica2
{
    partial class FormTelaPrincipal
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.buttonPararMod2 = new System.Windows.Forms.Button();
            this.buttonIniciarMod2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBarEnvioModulo2 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownQtdePacotes = new System.Windows.Forms.NumericUpDown();
            this.timerPlotaModulo2SinaisEnviados = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQtdePacotes)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.stopButton);
            this.groupBox5.Controls.Add(this.buttonPararMod2);
            this.groupBox5.Controls.Add(this.buttonIniciarMod2);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.progressBarEnvioModulo2);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.numericUpDownQtdePacotes);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(344, 249);
            this.groupBox5.TabIndex = 48;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Config. pacotes para gerar";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(116, 199);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(107, 41);
            this.stopButton.TabIndex = 50;
            this.stopButton.Text = "Parar";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonPararMod2
            // 
            this.buttonPararMod2.Enabled = false;
            this.buttonPararMod2.Location = new System.Drawing.Point(198, 157);
            this.buttonPararMod2.Name = "buttonPararMod2";
            this.buttonPararMod2.Size = new System.Drawing.Size(126, 36);
            this.buttonPararMod2.TabIndex = 69;
            this.buttonPararMod2.Text = "Gerar Anomalia";
            this.buttonPararMod2.UseVisualStyleBackColor = true;
            this.buttonPararMod2.Click += new System.EventHandler(this.buttonPararMod2_Click);
            // 
            // buttonIniciarMod2
            // 
            this.buttonIniciarMod2.Location = new System.Drawing.Point(9, 157);
            this.buttonIniciarMod2.Name = "buttonIniciarMod2";
            this.buttonIniciarMod2.Size = new System.Drawing.Size(129, 36);
            this.buttonIniciarMod2.TabIndex = 68;
            this.buttonIniciarMod2.Text = "Iniciar";
            this.buttonIniciarMod2.UseVisualStyleBackColor = true;
            this.buttonIniciarMod2.Click += new System.EventHandler(this.buttonIniciarMod2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 16);
            this.label7.TabIndex = 65;
            this.label7.Text = "Quantidade pacotes enviados:";
            // 
            // progressBarEnvioModulo2
            // 
            this.progressBarEnvioModulo2.Location = new System.Drawing.Point(9, 113);
            this.progressBarEnvioModulo2.Name = "progressBarEnvioModulo2";
            this.progressBarEnvioModulo2.Size = new System.Drawing.Size(315, 22);
            this.progressBarEnvioModulo2.TabIndex = 64;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 16);
            this.label4.TabIndex = 56;
            this.label4.Text = "Quantidade total de pacotes:";
            // 
            // numericUpDownQtdePacotes
            // 
            this.numericUpDownQtdePacotes.Location = new System.Drawing.Point(18, 58);
            this.numericUpDownQtdePacotes.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.numericUpDownQtdePacotes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownQtdePacotes.Name = "numericUpDownQtdePacotes";
            this.numericUpDownQtdePacotes.Size = new System.Drawing.Size(130, 22);
            this.numericUpDownQtdePacotes.TabIndex = 55;
            this.numericUpDownQtdePacotes.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(361, 255);
            this.tableLayoutPanel1.TabIndex = 49;
            // 
            // FormTelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 255);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "FormTelaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerador MockUP - módulo 5b";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQtdePacotes)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBarEnvioModulo2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownQtdePacotes;
        private System.Windows.Forms.Button buttonPararMod2;
        private System.Windows.Forms.Button buttonIniciarMod2;
        private System.Windows.Forms.Timer timerPlotaModulo2SinaisEnviados;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button stopButton;
    }
}

