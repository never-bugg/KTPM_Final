using KTPM_Final.Controllers;
using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace App
{
    public partial class frmXemPhieuNhap : Form
    {
        public frmXemPhieuNhap()
        {
            InitializeComponent();
        }

        private void frmXemPhieuNhap_Load(object sender, EventArgs e)
        {
            LoadLichSuNhapHang();
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void LoadLichSuNhapHang()
        {
            NhapSachController nhapBUS = new NhapSachController();
            NhanVienController nvBUS = new NhanVienController();
            List<NhapSachModel> dsPhieuNhap = nhapBUS.LayDanhSachPhieuNhap();

            // Convert List<NhapSachDTO> to DataTable
            DataTable dtPhieuNhap = new DataTable();
            dtPhieuNhap.Columns.Add("MaPhieuNhap", typeof(int));
            dtPhieuNhap.Columns.Add("MaNhanVien", typeof(int));
            dtPhieuNhap.Columns.Add("NgayNhap", typeof(DateTime));
            dtPhieuNhap.Columns.Add("TenNhanVien", typeof(string));
            dtPhieuNhap.Columns.Add("TongTien", typeof(decimal)); // Thêm cột Tổng Tiền

            foreach (var item in dsPhieuNhap)
            {
                DataRow row = dtPhieuNhap.NewRow();
                row["MaPhieuNhap"] = item.MaPhieuNhap;
                row["MaNhanVien"] = item.MaNhanVien;
                row["NgayNhap"] = item.NgayNhap;
                row["TenNhanVien"] = nvBUS.LayTenNhanVien(item.MaNhanVien);
                row["TongTien"] = item.TongTien; // Gán giá trị Tổng Tiền
                dtPhieuNhap.Rows.Add(row);
            }

            dgvLichSuNhapHang.DataSource = dtPhieuNhap;
            dgvLichSuNhapHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvLichSuNhapHang.Columns["MaNhanVien"] != null)
                dgvLichSuNhapHang.Columns["MaNhanVien"].Visible = false;
            if (dgvLichSuNhapHang.Columns["TenNhanVien"] != null)
                dgvLichSuNhapHang.Columns["TenNhanVien"].HeaderText = "Tên nhân viên";
            if (dgvLichSuNhapHang.Columns["TongTien"] != null)
                dgvLichSuNhapHang.Columns["TongTien"].HeaderText = "Tổng tiền";
        }



        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin admin = new frmAdmin();
            admin.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach sach = new frmSach();
            sach.Show();
            this.Hide();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVien = new frmNhanVien();
            nhanVien.Show();
            this.Hide();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon hoaDon = new frmXemHoaDon();
            hoaDon.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvLichSuNhapHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý khi click vào cell nếu cần
        }

        private void dgvLichSuNhapHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLichSuNhapHang.Rows[e.RowIndex];
            int maPhieuNhap = Convert.ToInt32(row.Cells["MaPhieuNhap"].Value);
            var frm = new ChiTietPhieuNhap(maPhieuNhap);
            frm.ShowDialog();
        }

        private void dgvLichSuNhapHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
