namespace WindowsFormsApp1
{
    partial class Form1
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
            this.web_1 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.btn_load = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.web_1)).BeginInit();
            this.SuspendLayout();
            // 
            // web_1
            // 
            this.web_1.AllowExternalDrop = true;
            this.web_1.CreationProperties = null;
            this.web_1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.web_1.Location = new System.Drawing.Point(12, 75);
            this.web_1.Name = "web_1";
            this.web_1.Size = new System.Drawing.Size(1330, 693);
            this.web_1.TabIndex = 0;
            this.web_1.ZoomFactor = 1D;
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(12, 12);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(177, 44);
            this.btn_load.TabIndex = 1;
            this.btn_load.Text = "Load/Reload";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 780);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.web_1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.web_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 web_1;
        private System.Windows.Forms.Button btn_load;
    }
}

