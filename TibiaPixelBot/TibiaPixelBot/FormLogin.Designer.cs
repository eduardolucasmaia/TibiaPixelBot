namespace TibiaPixelBot
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.tbEmail = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbPassword = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.cbRememberMe = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.btnLogin = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelLostPassword = new MaterialSkin.Controls.MaterialLabel();
            this.labelRegister = new MaterialSkin.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbEmail
            // 
            this.tbEmail.Depth = 0;
            this.tbEmail.Hint = "E-mail";
            this.tbEmail.Location = new System.Drawing.Point(12, 194);
            this.tbEmail.MouseState = MaterialSkin.MouseState.HOVER;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.PasswordChar = '\0';
            this.tbEmail.SelectedText = "";
            this.tbEmail.SelectionLength = 0;
            this.tbEmail.SelectionStart = 0;
            this.tbEmail.Size = new System.Drawing.Size(310, 23);
            this.tbEmail.TabIndex = 0;
            this.tbEmail.UseSystemPasswordChar = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::TibiaPixelBot.Properties.Resources.icon;
            this.pictureBox1.Location = new System.Drawing.Point(95, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(145, 132);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // tbPassword
            // 
            this.tbPassword.Depth = 0;
            this.tbPassword.Hint = "Password";
            this.tbPassword.Location = new System.Drawing.Point(12, 223);
            this.tbPassword.MouseState = MaterialSkin.MouseState.HOVER;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.SelectedText = "";
            this.tbPassword.SelectionLength = 0;
            this.tbPassword.SelectionStart = 0;
            this.tbPassword.Size = new System.Drawing.Size(310, 23);
            this.tbPassword.TabIndex = 2;
            this.tbPassword.UseSystemPasswordChar = false;
            // 
            // cbRememberMe
            // 
            this.cbRememberMe.AutoSize = true;
            this.cbRememberMe.Depth = 0;
            this.cbRememberMe.Font = new System.Drawing.Font("Roboto", 10F);
            this.cbRememberMe.Location = new System.Drawing.Point(9, 249);
            this.cbRememberMe.Margin = new System.Windows.Forms.Padding(0);
            this.cbRememberMe.MouseLocation = new System.Drawing.Point(-1, -1);
            this.cbRememberMe.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbRememberMe.Name = "cbRememberMe";
            this.cbRememberMe.Ripple = true;
            this.cbRememberMe.Size = new System.Drawing.Size(120, 30);
            this.cbRememberMe.TabIndex = 3;
            this.cbRememberMe.Text = "Remember me";
            this.cbRememberMe.UseVisualStyleBackColor = true;
            this.cbRememberMe.CheckedChanged += new System.EventHandler(this.cbRememberMe_CheckedChanged);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(12, 331);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(310, 1);
            this.materialDivider1.TabIndex = 5;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // btnLogin
            // 
            this.btnLogin.Depth = 0;
            this.btnLogin.Location = new System.Drawing.Point(247, 273);
            this.btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Primary = true;
            this.btnLogin.Size = new System.Drawing.Size(75, 33);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // labelLostPassword
            // 
            this.labelLostPassword.AutoSize = true;
            this.labelLostPassword.Depth = 0;
            this.labelLostPassword.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelLostPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelLostPassword.Location = new System.Drawing.Point(12, 354);
            this.labelLostPassword.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelLostPassword.Name = "labelLostPassword";
            this.labelLostPassword.Size = new System.Drawing.Size(148, 19);
            this.labelLostPassword.TabIndex = 9;
            this.labelLostPassword.Text = "Lost your password?";
            this.labelLostPassword.Click += new System.EventHandler(this.labelLostPassword_Click);
            // 
            // labelRegister
            // 
            this.labelRegister.AutoSize = true;
            this.labelRegister.Depth = 0;
            this.labelRegister.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelRegister.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelRegister.Location = new System.Drawing.Point(258, 354);
            this.labelRegister.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelRegister.Name = "labelRegister";
            this.labelRegister.Size = new System.Drawing.Size(64, 19);
            this.labelRegister.TabIndex = 10;
            this.labelRegister.Text = "Register";
            this.labelRegister.Click += new System.EventHandler(this.labelRegister_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 393);
            this.Controls.Add(this.labelRegister);
            this.Controls.Add(this.labelLostPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.cbRememberMe);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbEmail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.Move += new System.EventHandler(this.FormLogin_Move);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialSingleLineTextField tbEmail;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialSingleLineTextField tbPassword;
        private MaterialSkin.Controls.MaterialCheckBox cbRememberMe;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialRaisedButton btnLogin;
        private MaterialSkin.Controls.MaterialLabel labelLostPassword;
        private MaterialSkin.Controls.MaterialLabel labelRegister;
    }
}