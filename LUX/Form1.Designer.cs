namespace LUX
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
            this.FishingHistoryTextbox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.StatsTextBox = new System.Windows.Forms.RichTextBox();
            this.URLTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.AllStatsCheckBox = new System.Windows.Forms.CheckBox();
            this.UploadDataCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // FishingHistoryTextbox
            // 
            this.FishingHistoryTextbox.Location = new System.Drawing.Point(12, 52);
            this.FishingHistoryTextbox.Name = "FishingHistoryTextbox";
            this.FishingHistoryTextbox.Size = new System.Drawing.Size(374, 647);
            this.FishingHistoryTextbox.TabIndex = 0;
            this.FishingHistoryTextbox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter your fishing history";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(500, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(218, 141);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate output";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StatsTextBox
            // 
            this.StatsTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StatsTextBox.Location = new System.Drawing.Point(841, 52);
            this.StatsTextBox.Name = "StatsTextBox";
            this.StatsTextBox.ReadOnly = true;
            this.StatsTextBox.Size = new System.Drawing.Size(548, 647);
            this.StatsTextBox.TabIndex = 3;
            this.StatsTextBox.Text = "";
            this.StatsTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.StatsTextBox_LinkClicked);
            this.StatsTextBox.Click += new System.EventHandler(this.StatsTextBox_Click);
            this.StatsTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StatsTextBox_MouseMove);
            // 
            // URLTextbox
            // 
            this.URLTextbox.Location = new System.Drawing.Point(454, 66);
            this.URLTextbox.Name = "URLTextbox";
            this.URLTextbox.Size = new System.Drawing.Size(322, 23);
            this.URLTextbox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter the URL you were fishing on:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(500, 452);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(218, 141);
            this.button2.TabIndex = 6;
            this.button2.Text = "Show best planets";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AllStatsCheckBox
            // 
            this.AllStatsCheckBox.AutoSize = true;
            this.AllStatsCheckBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AllStatsCheckBox.Location = new System.Drawing.Point(454, 634);
            this.AllStatsCheckBox.Name = "AllStatsCheckBox";
            this.AllStatsCheckBox.Size = new System.Drawing.Size(322, 29);
            this.AllStatsCheckBox.TabIndex = 7;
            this.AllStatsCheckBox.Text = "Show stats of all planets combined";
            this.AllStatsCheckBox.UseVisualStyleBackColor = true;
            // 
            // UploadDataCheckbox
            // 
            this.UploadDataCheckbox.AutoSize = true;
            this.UploadDataCheckbox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UploadDataCheckbox.Location = new System.Drawing.Point(467, 345);
            this.UploadDataCheckbox.Name = "UploadDataCheckbox";
            this.UploadDataCheckbox.Size = new System.Drawing.Size(295, 29);
            this.UploadDataCheckbox.TabIndex = 8;
            this.UploadDataCheckbox.Text = "Upload planet data to database";
            this.UploadDataCheckbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1401, 726);
            this.Controls.Add(this.UploadDataCheckbox);
            this.Controls.Add(this.AllStatsCheckBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.URLTextbox);
            this.Controls.Add(this.StatsTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FishingHistoryTextbox);
            this.Name = "Form1";
            this.Text = "LUX Fishing Tool V1.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox FishingHistoryTextbox;
        private Label label1;
        private Button button1;
        private RichTextBox StatsTextBox;
        private TextBox URLTextbox;
        private Label label2;
        private Button button2;
        private CheckBox AllStatsCheckBox;
        private CheckBox UploadDataCheckbox;
    }
}