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
    public partial class frmTaiKhoan : Form
    {
        TaiKhoanController bus = new TaiKhoanController();
        public string LoggedInUser { get; set; }
        private bool isExiting = false; // ✅ Dùng để tránh xác nhận lặp

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
            dgvTaiKhoan.Columns["KichHoat"].Visible = false; 
            dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if(Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

        }
        public frmTaiKhoan()
        {
            InitializeComponent();
            this.FormClosing += frmTaiKhoan_FormClosing; 
        }

        private void frmTaiKhoan_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                TaiKhoanModel tk = new TaiKhoanModel(
                    txtMa.Text,
                    txtTen.Text,
                    txtMatKhau.Text,
                    cboVaiTro.SelectedItem.ToString(),
                    chkKichHoat.Checked
                );

                if (bus.ThemTaiKhoan(tk))
                {
                    MessageBox.Show("Thêm thành công");
                    dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TaiKhoanModel tk = new TaiKhoanModel(
                txtMa.Text,
                txtTen.Text,
                txtMatKhau.Text,
                cboVaiTro.SelectedItem.ToString(),
                chkKichHoat.Checked
            );
            if (bus.CapNhatTaiKhoan(tk))
            {
                MessageBox.Show("Cập nhật thành công");
                dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
            }
            else
                MessageBox.Show("Cập nhật thất bại");
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];
                txtMa.Text = row.Cells["MaTaiKhoan"].Value?.ToString();
                txtTen.Text = row.Cells["TenDangNhap"].Value?.ToString();
                txtMatKhau.Text = "******"; // Luôn hiển thị dấu sao
                cboVaiTro.SelectedItem = row.Cells["VaiTro"].Value?.ToString();
                chkKichHoat.Checked = row.Cells["KichHoat"].Value != null && (bool)row.Cells["KichHoat"].Value;
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMa.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa.");
                return;
            }

            if (txtTen.Text == LoggedInUser)
            {
                MessageBox.Show("Bạn không thể xóa tài khoản đang đăng nhập.");
                return;
            }

            if (cboVaiTro.SelectedItem?.ToString() == "Admin" && txtTen.Text == LoggedInUser)
            {
                MessageBox.Show("Admin không thể tự xóa chính mình.");
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?",
                                                "Xác nhận xóa",
                                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (bus.XoaTaiKhoan(txtMa.Text))
                {
                    MessageBox.Show("Xóa thành công.");
                    dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtSearchName.Text.Trim().ToLower();
            string searchRole = cboSearchRole.SelectedItem?.ToString();
            List<TaiKhoanModel> filteredAccounts = bus.TimKiemTaiKhoan(searchName, searchRole);
            dgvTaiKhoan.DataSource = filteredAccounts;
        }

        private void CancelSearch_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            cboSearchRole.SelectedIndex = -1;
            dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMa.Text = string.Empty;
            txtTen.Text = string.Empty;
            txtMatKhau.Text = string.Empty;
            cboVaiTro.SelectedIndex = -1;
            chkKichHoat.Checked = false;
        }

        private void menuSach_Click(object sender, EventArgs e)
        {
            frmSach sach = new frmSach();
            sach.Show();
        }

        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVien = new frmNhanVien();
            nhanVien.Show();
        }

        private void menuTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnTenNhanVien_Click(object sender, EventArgs e)
        {
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin home = new frmAdmin();
            home.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhap phieuNhapForm = new frmXemPhieuNhap();
            phieuNhapForm.Show();
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

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon xemhoadon = new frmXemHoaDon();
            xemhoadon.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần đổi mật khẩu.");
                return;
            }

            string newPassword = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Mật khẩu mới không được để trống.");
                return;
            }

            TaiKhoanModel tk = new TaiKhoanModel(
                txtMa.Text,
                txtTen.Text,
                newPassword,
                cboVaiTro.SelectedItem?.ToString(),
                chkKichHoat.Checked
            );

            if (bus.CapNhatTaiKhoan(tk))
            {
                MessageBox.Show("Đổi mật khẩu thành công.");
                dgvTaiKhoan.DataSource = bus.LayDanhSachTaiKhoan();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại.");
            }
        }

        private void chkKichHoat_CheckedChanged(object sender, EventArgs e)
        {
            // Lấy thông tin tài khoản đang chọn
            string maTaiKhoan = txtMa.Text;
            string vaiTro = cboVaiTro.SelectedItem?.ToString();
            bool isKichHoat = chkKichHoat.Checked;

            // Kiểm tra nếu là admin đang đăng nhập thì không cho phép bỏ kích hoạt
            if (Session.TaiKhoanHienTai != null &&
                Session.TaiKhoanHienTai.MaTaiKhoan == maTaiKhoan &&
                Session.TaiKhoanHienTai.VaiTro == "Admin" &&
                !isKichHoat)
            {
                MessageBox.Show("Không thể hủy kích hoạt tài khoản admin đang đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chkKichHoat.Checked = true; // Đặt lại trạng thái kích hoạt
                return;
            }
        }

    }
}
