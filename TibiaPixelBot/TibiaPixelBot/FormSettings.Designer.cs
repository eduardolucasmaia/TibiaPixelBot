namespace TibiaPixelBot
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.cbChangeLanguage = new System.Windows.Forms.ComboBox();
            this.btnApply = new MaterialSkin.Controls.MaterialFlatButton();
            this.cbCloseToTray = new MaterialSkin.Controls.MaterialCheckBox();
            this.cbMinimizeToTray = new MaterialSkin.Controls.MaterialCheckBox();
            this.nudAutoScan = new System.Windows.Forms.NumericUpDown();
            this.nudRule = new System.Windows.Forms.NumericUpDown();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRule)).BeginInit();
            this.SuspendLayout();
            // 
            // cbChangeLanguage
            // 
            this.cbChangeLanguage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cbChangeLanguage.ForeColor = System.Drawing.Color.White;
            this.cbChangeLanguage.FormattingEnabled = true;
            this.cbChangeLanguage.Items.AddRange(new object[] {
            "English",
            "Português"});
            this.cbChangeLanguage.Location = new System.Drawing.Point(109, 69);
            this.cbChangeLanguage.Name = "cbChangeLanguage";
            this.cbChangeLanguage.Size = new System.Drawing.Size(165, 21);
            this.cbChangeLanguage.TabIndex = 73;
            this.cbChangeLanguage.Text = "English";
            // 
            // btnApply
            // 
            this.btnApply.AutoSize = true;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Depth = 0;
            this.btnApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApply.Location = new System.Drawing.Point(551, 307);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnApply.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnApply.Name = "btnApply";
            this.btnApply.Primary = false;
            this.btnApply.Size = new System.Drawing.Size(54, 36);
            this.btnApply.TabIndex = 113;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbCloseToTray
            // 
            this.cbCloseToTray.AutoSize = true;
            this.cbCloseToTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cbCloseToTray.Depth = 0;
            this.cbCloseToTray.Font = new System.Drawing.Font("Roboto", 10F);
            this.cbCloseToTray.ForeColor = System.Drawing.Color.White;
            this.cbCloseToTray.Location = new System.Drawing.Point(16, 123);
            this.cbCloseToTray.Margin = new System.Windows.Forms.Padding(0);
            this.cbCloseToTray.MouseLocation = new System.Drawing.Point(-1, -1);
            this.cbCloseToTray.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbCloseToTray.Name = "cbCloseToTray";
            this.cbCloseToTray.Ripple = true;
            this.cbCloseToTray.Size = new System.Drawing.Size(385, 30);
            this.cbCloseToTray.TabIndex = 114;
            this.cbCloseToTray.Text = "Close button should minimize the Pixel window to the tray";
            this.cbCloseToTray.UseVisualStyleBackColor = false;
            // 
            // cbMinimizeToTray
            // 
            this.cbMinimizeToTray.AutoSize = true;
            this.cbMinimizeToTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cbMinimizeToTray.Depth = 0;
            this.cbMinimizeToTray.Font = new System.Drawing.Font("Roboto", 10F);
            this.cbMinimizeToTray.ForeColor = System.Drawing.Color.White;
            this.cbMinimizeToTray.Location = new System.Drawing.Point(16, 93);
            this.cbMinimizeToTray.Margin = new System.Windows.Forms.Padding(0);
            this.cbMinimizeToTray.MouseLocation = new System.Drawing.Point(-1, -1);
            this.cbMinimizeToTray.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbMinimizeToTray.Name = "cbMinimizeToTray";
            this.cbMinimizeToTray.Ripple = true;
            this.cbMinimizeToTray.Size = new System.Drawing.Size(407, 30);
            this.cbMinimizeToTray.TabIndex = 115;
            this.cbMinimizeToTray.Text = "Minimize button should minimize the Pixel window to the tray";
            this.cbMinimizeToTray.UseVisualStyleBackColor = false;
            // 
            // nudAutoScan
            // 
            this.nudAutoScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.nudAutoScan.ForeColor = System.Drawing.Color.White;
            this.nudAutoScan.Location = new System.Drawing.Point(16, 159);
            this.nudAutoScan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAutoScan.Name = "nudAutoScan";
            this.nudAutoScan.Size = new System.Drawing.Size(94, 20);
            this.nudAutoScan.TabIndex = 116;
            this.nudAutoScan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAutoScan.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudRule
            // 
            this.nudRule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.nudRule.ForeColor = System.Drawing.Color.White;
            this.nudRule.Location = new System.Drawing.Point(16, 187);
            this.nudRule.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRule.Name = "nudRule";
            this.nudRule.Size = new System.Drawing.Size(94, 20);
            this.nudRule.TabIndex = 118;
            this.nudRule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRule.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(116, 159);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(299, 19);
            this.materialLabel1.TabIndex = 119;
            this.materialLabel1.Text = "Number of Auto Scan attempts. (Default: 3)";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(116, 188);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(492, 19);
            this.materialLabel2.TabIndex = 120;
            this.materialLabel2.Text = "Maximum number of consecutive attempts to stop the rule. (Default: 30)";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(12, 69);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(91, 19);
            this.materialLabel3.TabIndex = 121;
            this.materialLabel3.Text = "LANGUAGE:";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 358);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.cbChangeLanguage);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.nudRule);
            this.Controls.Add(this.cbMinimizeToTray);
            this.Controls.Add(this.cbCloseToTray);
            this.Controls.Add(this.nudAutoScan);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Move += new System.EventHandler(this.FormSettings_Move);
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRule)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbChangeLanguage;
        private MaterialSkin.Controls.MaterialFlatButton btnApply;
        private MaterialSkin.Controls.MaterialCheckBox cbCloseToTray;
        private MaterialSkin.Controls.MaterialCheckBox cbMinimizeToTray;
        private System.Windows.Forms.NumericUpDown nudAutoScan;
        private System.Windows.Forms.NumericUpDown nudRule;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
    }
}