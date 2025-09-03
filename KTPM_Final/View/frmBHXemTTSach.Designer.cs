namespace App
{
    partial class frmBHXemTTSach
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
            System.Windows.Forms.ToolStripMenuItem đĂNGToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBHXemTTSach));
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnHuyTimKiem = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnTenNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSach = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvSach = new System.Windows.Forms.DataGridView();
            đĂNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSach)).BeginInit();
            this.SuspendLayout();
            // 
            // đĂNGToolStripMenuItem
            // 
            đĂNGToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("đĂNGToolStripMenuItem.Image")));
            đĂNGToolStripMenuItem.Name = "đĂNGToolStripMenuItem";
            đĂNGToolStripMenuItem.Size = new System.Drawing.Size(186, 26);
            đĂNGToolStripMenuItem.Text = "ĐĂNG XUẤT";
            đĂNGToolStripMenuItem.Click += new System.EventHandler(this.đĂNGToolStripMenuItem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimKiem.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(533, 48);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(309, 28);
            this.txtTimKiem.TabIndex = 27;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // btnHuyTimKiem
            // 
            this.btnHuyTimKiem.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuyTimKiem.Location = new System.Drawing.Point(848, 48);
            this.btnHuyTimKiem.Name = "btnHuyTimKiem";
            this.btnHuyTimKiem.Size = new System.Drawing.Size(172, 28);
            this.btnHuyTimKiem.TabIndex = 26;
            this.btnHuyTimKiem.Text = "Hủy Tìm Kiếm";
            this.btnHuyTimKiem.UseVisualStyleBackColor = true;
            this.btnHuyTimKiem.Click += new System.EventHandler(this.btnHuyTimKiem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnTenNhanVien,
            this.mnHoaDon,
            this.mnSach});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1032, 29);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnTenNhanVien
            // 
            this.mnTenNhanVien.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            đĂNGToolStripMenuItem});
            this.mnTenNhanVien.Name = "mnTenNhanVien";
            this.mnTenNhanVien.Size = new System.Drawing.Size(33, 25);
            this.mnTenNhanVien.Text = "1";
            // 
            // mnHoaDon
            // 
            this.mnHoaDon.Name = "mnHoaDon";
            this.mnHoaDon.Size = new System.Drawing.Size(142, 25);
            this.mnHoaDon.Text = " TẠO HÓA ĐƠN";
            this.mnHoaDon.Click += new System.EventHandler(this.mnHoaDon_Click);
            // 
            // mnSach
            // 
            this.mnSach.Name = "mnSach";
            this.mnSach.Size = new System.Drawing.Size(162, 25);
            this.mnSach.Text = " THÔNG TIN SÁCH";
            // 
            // dgvSach
            // 
            this.dgvSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSach.Location = new System.Drawing.Point(12, 82);
            this.dgvSach.Name = "dgvSach";
            this.dgvSach.RowHeadersWidth = 51;
            this.dgvSach.RowTemplate.Height = 24;
            this.dgvSach.Size = new System.Drawing.Size(1008, 509);
            this.dgvSach.TabIndex = 29;
            // 
            // frmBHXemTTSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(1032, 603);
            this.Controls.Add(this.dgvSach);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.btnHuyTimKiem);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBHXemTTSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmThongTinSach";
            this.Load += new System.EventHandler(this.frmThongTinSach_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnHuyTimKiem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnTenNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnHoaDon;
        private System.Windows.Forms.ToolStripMenuItem mnSach;
        private System.Windows.Forms.DataGridView dgvSach;
    }
}