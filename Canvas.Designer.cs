namespace Project2
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.controlElem = new System.Windows.Forms.GroupBox();
            this.save = new System.Windows.Forms.Button();
            this.styleGraf = new System.Windows.Forms.Button();
            this.styleBack = new System.Windows.Forms.Button();
            this.randFunction = new System.Windows.Forms.Button();
            this.canvasPanel = new Project2.DoubleBufferedPanel();
            this.TurnRain = new System.Windows.Forms.Button();
            this.controlElem.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlElem
            // 
            this.controlElem.BackColor = System.Drawing.Color.CornflowerBlue;
            this.controlElem.Controls.Add(this.TurnRain);
            this.controlElem.Controls.Add(this.save);
            this.controlElem.Controls.Add(this.styleGraf);
            this.controlElem.Controls.Add(this.styleBack);
            this.controlElem.Controls.Add(this.randFunction);
            this.controlElem.Dock = System.Windows.Forms.DockStyle.Right;
            this.controlElem.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.controlElem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.controlElem.Location = new System.Drawing.Point(622, 0);
            this.controlElem.Name = "controlElem";
            this.controlElem.Size = new System.Drawing.Size(178, 450);
            this.controlElem.TabIndex = 0;
            this.controlElem.TabStop = false;
            this.controlElem.Text = "Элементы управления";
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.save.BackColor = System.Drawing.Color.OliveDrab;
            this.save.ForeColor = System.Drawing.SystemColors.Control;
            this.save.Location = new System.Drawing.Point(6, 315);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(168, 31);
            this.save.TabIndex = 3;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = false;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // styleGraf
            // 
            this.styleGraf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.styleGraf.BackColor = System.Drawing.Color.RoyalBlue;
            this.styleGraf.ForeColor = System.Drawing.SystemColors.Control;
            this.styleGraf.Location = new System.Drawing.Point(6, 214);
            this.styleGraf.Name = "styleGraf";
            this.styleGraf.Size = new System.Drawing.Size(168, 31);
            this.styleGraf.TabIndex = 2;
            this.styleGraf.Text = "Стиль графика";
            this.styleGraf.UseVisualStyleBackColor = false;
            this.styleGraf.Click += new System.EventHandler(this.styleGraf_Click);
            // 
            // styleBack
            // 
            this.styleBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.styleBack.BackColor = System.Drawing.Color.SlateBlue;
            this.styleBack.ForeColor = System.Drawing.SystemColors.Control;
            this.styleBack.Location = new System.Drawing.Point(6, 177);
            this.styleBack.Name = "styleBack";
            this.styleBack.Size = new System.Drawing.Size(168, 31);
            this.styleBack.TabIndex = 1;
            this.styleBack.Text = "Стиль фона";
            this.styleBack.UseVisualStyleBackColor = false;
            this.styleBack.Click += new System.EventHandler(this.styleBack_Click);
            // 
            // randFunction
            // 
            this.randFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.randFunction.BackColor = System.Drawing.Color.Chocolate;
            this.randFunction.ForeColor = System.Drawing.SystemColors.Control;
            this.randFunction.Location = new System.Drawing.Point(6, 74);
            this.randFunction.Name = "randFunction";
            this.randFunction.Size = new System.Drawing.Size(168, 31);
            this.randFunction.TabIndex = 0;
            this.randFunction.Text = "Случайная функция";
            this.randFunction.UseVisualStyleBackColor = false;
            this.randFunction.Click += new System.EventHandler(this.randFunction_Click);
            // 
            // canvasPanel
            // 
            this.canvasPanel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.canvasPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasPanel.Location = new System.Drawing.Point(0, 0);
            this.canvasPanel.Name = "canvasPanel";
            this.canvasPanel.Size = new System.Drawing.Size(622, 450);
            this.canvasPanel.TabIndex = 1;
            // 
            // TurnRain
            // 
            this.TurnRain.BackColor = System.Drawing.Color.Crimson;
            this.TurnRain.ForeColor = System.Drawing.SystemColors.Control;
            this.TurnRain.Location = new System.Drawing.Point(6, 389);
            this.TurnRain.Name = "TurnRain";
            this.TurnRain.Size = new System.Drawing.Size(168, 31);
            this.TurnRain.TabIndex = 4;
            this.TurnRain.Text = "Дождь (выкл.)";
            this.TurnRain.UseVisualStyleBackColor = false;
            this.TurnRain.Click += new System.EventHandler(this.TurnRain_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.canvasPanel);
            this.Controls.Add(this.controlElem);
            this.Name = "Main";
            this.Text = "Конструктор графиков";
            this.controlElem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox controlElem;
        private DoubleBufferedPanel canvasPanel;
        private System.Windows.Forms.Button styleBack;
        private System.Windows.Forms.Button randFunction;
        private System.Windows.Forms.Button styleGraf;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button TurnRain;
    }
}

