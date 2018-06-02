namespace PikaLoveBot
{
    partial class MainFormPikaLoveBot
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
            this.labelTotalInBase = new System.Windows.Forms.Label();
            this.labelUsersReq = new System.Windows.Forms.Label();
            this.labelRequests = new System.Windows.Forms.Label();
            this.labelTowns = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.большеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьГородToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.полностьюОбновитьБазуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьЮзераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьГородToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.статистикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.статистикаПоГородамToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьПользователяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поНикуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поТелеграмЛогинуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTotalInBase
            // 
            this.labelTotalInBase.AutoSize = true;
            this.labelTotalInBase.Location = new System.Drawing.Point(9, 36);
            this.labelTotalInBase.Name = "labelTotalInBase";
            this.labelTotalInBase.Size = new System.Drawing.Size(74, 13);
            this.labelTotalInBase.TabIndex = 0;
            this.labelTotalInBase.Text = "Total in base: ";
            // 
            // labelUsersReq
            // 
            this.labelUsersReq.AutoSize = true;
            this.labelUsersReq.Location = new System.Drawing.Point(9, 58);
            this.labelUsersReq.Name = "labelUsersReq";
            this.labelUsersReq.Size = new System.Drawing.Size(40, 13);
            this.labelUsersReq.TabIndex = 1;
            this.labelUsersReq.Text = "Users: ";
            // 
            // labelRequests
            // 
            this.labelRequests.AutoSize = true;
            this.labelRequests.Location = new System.Drawing.Point(9, 81);
            this.labelRequests.Name = "labelRequests";
            this.labelRequests.Size = new System.Drawing.Size(58, 13);
            this.labelRequests.TabIndex = 2;
            this.labelRequests.Text = "Requests: ";
            // 
            // labelTowns
            // 
            this.labelTowns.AutoSize = true;
            this.labelTowns.Location = new System.Drawing.Point(9, 103);
            this.labelTowns.Name = "labelTowns";
            this.labelTowns.Size = new System.Drawing.Size(45, 13);
            this.labelTowns.TabIndex = 3;
            this.labelTowns.Text = "Towns: ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 128);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(494, 105);
            this.textBox1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.большеToolStripMenuItem,
            this.статистикаToolStripMenuItem,
            this.редактироватьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(516, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // большеToolStripMenuItem
            // 
            this.большеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьГородToolStripMenuItem,
            this.полностьюОбновитьБазуToolStripMenuItem,
            this.удалитьЮзераToolStripMenuItem,
            this.удалитьГородToolStripMenuItem});
            this.большеToolStripMenuItem.Name = "большеToolStripMenuItem";
            this.большеToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.большеToolStripMenuItem.Text = "Больше...";
            // 
            // добавитьГородToolStripMenuItem
            // 
            this.добавитьГородToolStripMenuItem.Name = "добавитьГородToolStripMenuItem";
            this.добавитьГородToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.добавитьГородToolStripMenuItem.Text = "Добавить город";
            this.добавитьГородToolStripMenuItem.Click += new System.EventHandler(this.добавитьГородToolStripMenuItem_Click);
            // 
            // полностьюОбновитьБазуToolStripMenuItem
            // 
            this.полностьюОбновитьБазуToolStripMenuItem.Name = "полностьюОбновитьБазуToolStripMenuItem";
            this.полностьюОбновитьБазуToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.полностьюОбновитьБазуToolStripMenuItem.Text = "Полностью обновить базу";
            this.полностьюОбновитьБазуToolStripMenuItem.Click += new System.EventHandler(this.полностьюОбновитьБазуToolStripMenuItem_Click);
            // 
            // удалитьЮзераToolStripMenuItem
            // 
            this.удалитьЮзераToolStripMenuItem.Name = "удалитьЮзераToolStripMenuItem";
            this.удалитьЮзераToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.удалитьЮзераToolStripMenuItem.Text = "Удалить юзера";
            this.удалитьЮзераToolStripMenuItem.Click += new System.EventHandler(this.удалитьЮзераToolStripMenuItem_Click);
            // 
            // удалитьГородToolStripMenuItem
            // 
            this.удалитьГородToolStripMenuItem.Name = "удалитьГородToolStripMenuItem";
            this.удалитьГородToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.удалитьГородToolStripMenuItem.Text = "Удалить город";
            this.удалитьГородToolStripMenuItem.Click += new System.EventHandler(this.удалитьГородToolStripMenuItem_Click);
            // 
            // статистикаToolStripMenuItem
            // 
            this.статистикаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.статистикаПоГородамToolStripMenuItem});
            this.статистикаToolStripMenuItem.Name = "статистикаToolStripMenuItem";
            this.статистикаToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.статистикаToolStripMenuItem.Text = "Статистика";
            // 
            // статистикаПоГородамToolStripMenuItem
            // 
            this.статистикаПоГородамToolStripMenuItem.Name = "статистикаПоГородамToolStripMenuItem";
            this.статистикаПоГородамToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.статистикаПоГородамToolStripMenuItem.Text = "Статистика по городам";
            this.статистикаПоГородамToolStripMenuItem.Click += new System.EventHandler(this.статистикаПоГородамToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(405, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Обновить лог";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.редактироватьПользователяToolStripMenuItem});
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            // 
            // редактироватьПользователяToolStripMenuItem
            // 
            this.редактироватьПользователяToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.поНикуToolStripMenuItem,
            this.поТелеграмЛогинуToolStripMenuItem});
            this.редактироватьПользователяToolStripMenuItem.Name = "редактироватьПользователяToolStripMenuItem";
            this.редактироватьПользователяToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.редактироватьПользователяToolStripMenuItem.Text = "Редактировать пользователя";
            // 
            // поНикуToolStripMenuItem
            // 
            this.поНикуToolStripMenuItem.Name = "поНикуToolStripMenuItem";
            this.поНикуToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.поНикуToolStripMenuItem.Text = "...по нику";
            this.поНикуToolStripMenuItem.Click += new System.EventHandler(this.поНикуToolStripMenuItem_Click);
            // 
            // поТелеграмЛогинуToolStripMenuItem
            // 
            this.поТелеграмЛогинуToolStripMenuItem.Name = "поТелеграмЛогинуToolStripMenuItem";
            this.поТелеграмЛогинуToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.поТелеграмЛогинуToolStripMenuItem.Text = "...по телеграм логину";
            // 
            // MainFormPikaLoveBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 271);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelTowns);
            this.Controls.Add(this.labelRequests);
            this.Controls.Add(this.labelUsersReq);
            this.Controls.Add(this.labelTotalInBase);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFormPikaLoveBot";
            this.Text = "PikaLoveBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTotalInBase;
        private System.Windows.Forms.Label labelUsersReq;
        private System.Windows.Forms.Label labelRequests;
        private System.Windows.Forms.Label labelTowns;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem большеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьГородToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem полностьюОбновитьБазуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьЮзераToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьГородToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem статистикаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem статистикаПоГородамToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьПользователяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поНикуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поТелеграмЛогинуToolStripMenuItem;
    }
}

