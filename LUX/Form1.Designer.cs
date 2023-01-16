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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.FishingHistoryTextbox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.StatsTextBox = new System.Windows.Forms.RichTextBox();
            this.URLTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.AllStatsCheckBox = new System.Windows.Forms.CheckBox();
            this.UploadDataCheckbox = new System.Windows.Forms.CheckBox();
            this.whitelistFish = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.whitelistmultiplier = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FishingHistoryTextbox
            // 
            this.FishingHistoryTextbox.Location = new System.Drawing.Point(12, 45);
            this.FishingHistoryTextbox.Name = "FishingHistoryTextbox";
            this.FishingHistoryTextbox.Size = new System.Drawing.Size(351, 611);
            this.FishingHistoryTextbox.TabIndex = 0;
            this.FishingHistoryTextbox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter your fishing history";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(430, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 139);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate output";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StatsTextBox
            // 
            this.StatsTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StatsTextBox.Location = new System.Drawing.Point(719, 45);
            this.StatsTextBox.Name = "StatsTextBox";
            this.StatsTextBox.ReadOnly = true;
            this.StatsTextBox.Size = new System.Drawing.Size(531, 611);
            this.StatsTextBox.TabIndex = 3;
            this.StatsTextBox.Text = "";
            this.StatsTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.StatsTextBox_LinkClicked);
            this.StatsTextBox.Click += new System.EventHandler(this.StatsTextBox_Click);
            this.StatsTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StatsTextBox_MouseMove);
            // 
            // URLTextbox
            // 
            this.URLTextbox.Location = new System.Drawing.Point(382, 45);
            this.URLTextbox.Name = "URLTextbox";
            this.URLTextbox.Size = new System.Drawing.Size(322, 23);
            this.URLTextbox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(382, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(322, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter the URL you were fishing on:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(430, 409);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(226, 139);
            this.button2.TabIndex = 6;
            this.button2.Text = "Show best planets";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AllStatsCheckBox
            // 
            this.AllStatsCheckBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AllStatsCheckBox.Location = new System.Drawing.Point(382, 554);
            this.AllStatsCheckBox.Name = "AllStatsCheckBox";
            this.AllStatsCheckBox.Size = new System.Drawing.Size(322, 33);
            this.AllStatsCheckBox.TabIndex = 7;
            this.AllStatsCheckBox.Text = "Show stats of all planets combined";
            this.AllStatsCheckBox.UseVisualStyleBackColor = true;
            // 
            // UploadDataCheckbox
            // 
            this.UploadDataCheckbox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UploadDataCheckbox.Location = new System.Drawing.Point(400, 323);
            this.UploadDataCheckbox.Name = "UploadDataCheckbox";
            this.UploadDataCheckbox.Size = new System.Drawing.Size(295, 29);
            this.UploadDataCheckbox.TabIndex = 8;
            this.UploadDataCheckbox.Text = "Upload planet data to database";
            this.UploadDataCheckbox.UseVisualStyleBackColor = true;
            // 
            // whitelistFish
            // 
            this.whitelistFish.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whitelistFish.FormattingEnabled = true;
            this.whitelistFish.Location = new System.Drawing.Point(400, 294);
            this.whitelistFish.Name = "whitelistFish";
            this.whitelistFish.Size = new System.Drawing.Size(121, 23);
            this.whitelistFish.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(400, 272);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "Keep an eye on:";
            // 
            // whitelistmultiplier
            // 
            this.whitelistmultiplier.Location = new System.Drawing.Point(574, 294);
            this.whitelistmultiplier.Mask = "1.000";
            this.whitelistmultiplier.Name = "whitelistmultiplier";
            this.whitelistmultiplier.PromptChar = '0';
            this.whitelistmultiplier.Size = new System.Drawing.Size(100, 23);
            this.whitelistmultiplier.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(574, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Choose the multiplier:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 671);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.whitelistmultiplier);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.whitelistFish);
            this.Controls.Add(this.UploadDataCheckbox);
            this.Controls.Add(this.AllStatsCheckBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.URLTextbox);
            this.Controls.Add(this.StatsTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FishingHistoryTextbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "LUX Fishing Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
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
        private ComboBox whitelistFish;
        private Label label3;
        private MaskedTextBox whitelistmultiplier;
        private Label label4;
    }
}