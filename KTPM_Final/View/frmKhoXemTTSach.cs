using KTPM_Final.Controllers;
using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace App
{
    public partial class frmKhoXemTTSach : Form
    {
        private SachController sachBUS = new SachController();
        private List<SachModel> danhSachSach = new List<SachModel>();

        public frmKhoXemTTSach()
        {
            InitializeComponent();
            this.Load += frmKhoXemTTSach_Load;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            btnHuyTimKiem.Click += btnHuyTimKiem_Click;
        }

        private void frmKhoXemTTSach_Load(object sender, EventArgs e)
        {
            LoadDanhSachSach();
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thiết lập AutoComplete cho TextBox tìm kiếm
            var autoComplete = new AutoCompleteStringCollection();
            foreach (var s in danhSachSach)
            {
                if (!string.IsNullOrEmpty(s.MaSach))
                    autoComplete.Add(s.MaSach);
                if (!string.IsNullOrEmpty(s.TenSach))
                    autoComplete.Add(s.TenSach);
                autoComplete.Add(s.GiaNhap.ToString("N0"));
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoComplete;
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void LoadDanhSachSach()
        {
            var dt = sachBUS.LayDanhSachSachDangKinhDoanh();
            danhSachSach = new List<SachModel>();
            foreach (DataRow row in dt.Rows)
            {
                danhSachSach.Add(new SachModel
                {
                    MaSach = row["MaSach"].ToString(),
                    TenSach = row["TenSach"].ToString(),
                    SoLuong = row["SoLuong"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLuong"]),
                    GiaNhap = row["GiaNhap"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaNhap"])
                });
            }

            dgvSach.DataSource = null;
            dgvSach.DataSource = danhSachSach.Select(s => new
            {
                s.MaSach,
                s.TenSach,
                s.SoLuong,
                GiaNhap = s.GiaNhap.ToString("N0")
            }).ToList();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            var ketQua = danhSachSach.Where(s =>
                (!string.IsNullOrEmpty(s.MaSach) && s.MaSach.ToLower().Contains(tuKhoa)) ||
                (!string.IsNullOrEmpty(s.TenSach) && s.TenSach.ToLower().Contains(tuKhoa)) ||
                s.GiaNhap.ToString("N0").Replace(",", "").Contains(tuKhoa.Replace(",", ""))
            ).Select(s => new
            {
                s.MaSach,
                s.TenSach,
                s.SoLuong,
                GiaNhap = s.GiaNhap.ToString("N0")
            }).ToList();

            dgvSach.DataSource = null;
            dgvSach.DataSource = ketQua;
        }

        private void btnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            LoadDanhSachSach();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmNhapSach frm = new frmNhapSach();
            frm.Show();
            this.Hide();
        }

        private void tHÔNGTINSÁCHToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
