namespace TibiaPixelBot
{
    partial class FormGetHotkey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetHotkey));
            this.btnOk = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelHotkey = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Depth = 0;
            this.btnOk.Location = new System.Drawing.Point(116, 161);
            this.btnOk.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOk.Name = "btnOk";
            this.btnOk.Primary = true;
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelHotkey
            // 
            this.labelHotkey.AutoSize = true;
            this.labelHotkey.Depth = 0;
            this.labelHotkey.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelHotkey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelHotkey.Location = new System.Drawing.Point(153, 105);
            this.labelHotkey.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelHotkey.Name = "labelHotkey";
            this.labelHotkey.Size = new System.Drawing.Size(0, 19);
            this.labelHotkey.TabIndex = 1;
            this.labelHotkey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormGetHotkey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 196);
            this.Controls.Add(this.labelHotkey);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGetHotkey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGetKotkey_FormClosing);
            this.Load += new System.EventHandler(this.FormGetKotkey_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormGetHotkey_KeyPress);
            this.Move += new System.EventHandler(this.FormGetKotkey_Move);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton btnOk;
        private MaterialSkin.Controls.MaterialLabel labelHotkey;
    }
}