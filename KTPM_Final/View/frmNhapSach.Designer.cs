namespace App
{
    partial class frmNhapSach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNhapSach));
            this.dgvDanhSach = new System.Windows.Forms.DataGridView();
            this.dgvCanhBaoHangTon = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnDoiGia = new System.Windows.Forms.Button();
            this.txtGiaNhap = new System.Windows.Forms.TextBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.cbSach = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTaoPhieuNhap = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnTenNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnNhapSach = new System.Windows.Forms.ToolStripMenuItem();
            this.tHÔNGTINSÁCHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSua = new System.Windows.Forms.Button();
            đĂNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanhBaoHangTon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // dgvDanhSach
            // 
            this.dgvDanhSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSach.Location = new System.Drawing.Point(432, 56);
            this.dgvDanhSach.Name = "dgvDanhSach";
            this.dgvDanhSach.RowHeadersWidth = 51;
            this.dgvDanhSach.RowTemplate.Height = 24;
            this.dgvDanhSach.Size = new System.Drawing.Size(588, 226);
            this.dgvDanhSach.TabIndex = 0;
            this.dgvDanhSach.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSach_CellContentClick);
            this.dgvDanhSach.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSach_CellEndEdit);
            this.dgvDanhSach.SelectionChanged += new System.EventHandler(this.dgvDanhSach_SelectionChanged);
            // 
            // dgvCanhBaoHangTon
            // 
            this.dgvCanhBaoHangTon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCanhBaoHangTon.Location = new System.Drawing.Point(12, 356);
            this.dgvCanhBaoHangTon.Name = "dgvCanhBaoHangTon";
            this.dgvCanhBaoHangTon.RowHeadersWidth = 51;
            this.dgvCanhBaoHangTon.Size = new System.Drawing.Size(1008, 235);
            this.dgvCanhBaoHangTon.TabIndex = 1;
            this.dgvCanhBaoHangTon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCanhBaoHangTon_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnThem);
            this.groupBox1.Controls.Add(this.btnDoiGia);
            this.groupBox1.Controls.Add(this.txtGiaNhap);
            this.groupBox1.Controls.Add(this.txtSoLuong);
            this.groupBox1.Controls.Add(this.cbSach);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 235);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tạo Phiếu Nhập";
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(175, 190);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(101, 39);
            this.btnThem.TabIndex = 7;
            this.btnThem.Text = "THÊM";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnDoiGia
            // 
            this.btnDoiGia.Location = new System.Drawing.Point(297, 145);
            this.btnDoiGia.Name = "btnDoiGia";
            this.btnDoiGia.Size = new System.Drawing.Size(75, 29);
            this.btnDoiGia.TabIndex = 6;
            this.btnDoiGia.Text = "Đổi giá";
            this.btnDoiGia.UseVisualStyleBackColor = true;
            this.btnDoiGia.Click += new System.EventHandler(this.btnDoiGia_Click);
            // 
            // txtGiaNhap
            // 
            this.txtGiaNhap.Location = new System.Drawing.Point(129, 146);
            this.txtGiaNhap.Name = "txtGiaNhap";
            this.txtGiaNhap.Size = new System.Drawing.Size(147, 28);
            this.txtGiaNhap.TabIndex = 5;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(129, 91);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(243, 28);
            this.txtSoLuong.TabIndex = 4;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoLuong_KeyPress);
            this.txtSoLuong.Leave += new System.EventHandler(this.txtSoLuong_Leave);
            // 
            // cbSach
            // 
            this.cbSach.FormattingEnabled = true;
            this.cbSach.Location = new System.Drawing.Point(129, 31);
            this.cbSach.Name = "cbSach";
            this.cbSach.Size = new System.Drawing.Size(243, 29);
            this.cbSach.TabIndex = 3;
            this.cbSach.SelectedIndexChanged += new System.EventHandler(this.cbSach_SelectedIndexChanged);
            this.cbSach.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbSach_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Giá Nhập";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Số lượng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chọn sách";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 21);
            this.label4.TabIndex = 8;
            this.label4.Text = "Cảnh báo tồn kho (SL<10)";
            // 
            // btnTaoPhieuNhap
            // 
            this.btnTaoPhieuNhap.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoPhieuNhap.Location = new System.Drawing.Point(833, 298);
            this.btnTaoPhieuNhap.Name = "btnTaoPhieuNhap";
            this.btnTaoPhieuNhap.Size = new System.Drawing.Size(187, 39);
            this.btnTaoPhieuNhap.TabIndex = 8;
            this.btnTaoPhieuNhap.Text = "TẠO PHIẾU NHẬP";
            this.btnTaoPhieuNhap.UseVisualStyleBackColor = true;
            this.btnTaoPhieuNhap.Click += new System.EventHandler(this.btnTaoPhieuNhap_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(745, 298);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 39);
            this.button4.TabIndex = 9;
            this.button4.Text = "XÓA";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnTenNhanVien,
            this.mnNhapSach,
            this.tHÔNGTINSÁCHToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1032, 29);
            this.menuStrip1.TabIndex = 22;
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
            // mnNhapSach
            // 
            this.mnNhapSach.Name = "mnNhapSach";
            this.mnNhapSach.Size = new System.Drawing.Size(112, 25);
            this.mnNhapSach.Text = "NHẬP SÁCH";
            // 
            // tHÔNGTINSÁCHToolStripMenuItem
            // 
            this.tHÔNGTINSÁCHToolStripMenuItem.Name = "tHÔNGTINSÁCHToolStripMenuItem";
            this.tHÔNGTINSÁCHToolStripMenuItem.Size = new System.Drawing.Size(157, 25);
            this.tHÔNGTINSÁCHToolStripMenuItem.Text = "THÔNG TIN SÁCH";
            this.tHÔNGTINSÁCHToolStripMenuItem.Click += new System.EventHandler(this.tHÔNGTINSÁCHToolStripMenuItem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.Location = new System.Drawing.Point(657, 298);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(82, 39);
            this.btnSua.TabIndex = 23;
            this.btnSua.Text = "SỬA";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // frmNhapSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(1032, 603);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnTaoPhieuNhap);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvCanhBaoHangTon);
            this.Controls.Add(this.dgvDanhSach);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNhapSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập Sách";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNhapSach_FormClosing);
            this.Load += new System.EventHandler(this.frmNhapSach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanhBaoHangTon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDanhSach;
        private System.Windows.Forms.DataGridView dgvCanhBaoHangTon;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnDoiGia;
        private System.Windows.Forms.TextBox txtGiaNhap;
        private System.Windows.Forms.ComboBox cbSach;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTaoPhieuNhap;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnTenNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnNhapSach;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.ToolStripMenuItem tHÔNGTINSÁCHToolStripMenuItem;
    }
}