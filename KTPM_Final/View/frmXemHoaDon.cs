using KTPM_Final.Controllers;
using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class frmXemHoaDon : Form
    {
        private HoaDonController hoaDonBUS = new HoaDonController();
        private BindingList<HoaDonModel> _hoaDonGoc;
        private bool isExiting = false;


        public frmXemHoaDon()
        {
            InitializeComponent();
            this.FormClosing += frmXemHoaDon_FormClosing;

            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            btnHuyTimKiem.Click += btnHuyTimKiem_Click;
            dtpNgayTao.ValueChanged += dtpNgayTao_ValueChanged;
        }

        private void frmXemHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn thoát chương trình?",
                    "Xác nhận",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    isExiting = true;
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void frmXemHoaDon_Load(object sender, EventArgs e)
        {
            var list = hoaDonBUS.LayTatCaHoaDon();
            _hoaDonGoc = new BindingList<HoaDonModel>(list);
            dgvLichSuHoaDon.DataSource = _hoaDonGoc;
            dgvLichSuHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Autocomplete
            var autoSource = new AutoCompleteStringCollection();
            autoSource.AddRange(list
                .SelectMany(x => new string[]
                {
                x.MaHoaDon.ToString(),
                x.TongTien.ToString(),
                x.ThoiGianTao.ToString("dd/MM/yyyy"),
                x.TenNhanVien
                })
                .Distinct()
                .ToArray());

            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoSource;

            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LocVaTimKiem();
        }

        private void dtpNgayTao_ValueChanged(object sender, EventArgs e)
        {
            LocVaTimKiem();
        }

        private void btnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";

            if (_hoaDonGoc != null && _hoaDonGoc.Count > 0)
            {
                dtpNgayTao.Value = _hoaDonGoc.Min(x => x.ThoiGianTao).Date;
            }
            else
            {
                dtpNgayTao.Value = DateTime.Now;
            }

            LocVaTimKiem();
        }

        private void LocVaTimKiem()
        {
            if (_hoaDonGoc == null) return;

            string keyword = txtTimKiem.Text.Trim().ToLower();
            DateTime selectedDate = dtpNgayTao.Value.Date;
            DateTime minDate = _hoaDonGoc.Min(x => x.ThoiGianTao).Date;

            var filtered = _hoaDonGoc.Where(x =>
                (string.IsNullOrEmpty(keyword) ||
                    x.MaHoaDon.ToString().Contains(keyword) ||
                    x.TongTien.ToString().Contains(keyword) ||
                    x.ThoiGianTao.ToString("dd/MM/yyyy").Contains(keyword) ||
                    (x.TenNhanVien != null && x.TenNhanVien.ToLower().Contains(keyword))
                )
                && (selectedDate == minDate || x.ThoiGianTao.Date == selectedDate)
            ).ToList();

            dgvLichSuHoaDon.DataSource = new BindingList<HoaDonModel>(filtered);
        }

        private void dgvLichSuHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var row = dgvLichSuHoaDon.Rows[e.RowIndex];
                    int maHoaDon = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
                    frmChiTietHoaDon frm = new frmChiTietHoaDon(maHoaDon);
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            frmSach sachForm = new frmSach();
            sachForm.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhap phieuNhapForm = new frmXemPhieuNhap();
            phieuNhapForm.Show();
            this.Hide();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVienForm = new frmNhanVien();
            nhanVienForm.Show();
            this.Hide();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoanForm = new frmTaiKhoan();
            taiKhoanForm.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            
        }
    }
}
