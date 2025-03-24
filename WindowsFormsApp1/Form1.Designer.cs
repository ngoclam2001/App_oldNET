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
            this.txt_areaID = new System.Windows.Forms.TextBox();
            this.lb_check = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            // txt_areaID
            // 
            this.txt_areaID.Location = new System.Drawing.Point(216, 34);
            this.txt_areaID.Name = "txt_areaID";
            this.txt_areaID.Size = new System.Drawing.Size(159, 22);
            this.txt_areaID.TabIndex = 2;
            // 
            // lb_check
            // 
            this.lb_check.AutoSize = true;
            this.lb_check.Location = new System.Drawing.Point(22, 88);
            this.lb_check.Name = "lb_check";
            this.lb_check.Size = new System.Drawing.Size(0, 16);
            this.lb_check.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nhập mã giấy chứng nhận";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 780);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_check);
            this.Controls.Add(this.txt_areaID);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.web_1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.web_1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 web_1;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.TextBox txt_areaID;
        private System.Windows.Forms.Label lb_check;
        private System.Windows.Forms.Label label1;
    }
}

