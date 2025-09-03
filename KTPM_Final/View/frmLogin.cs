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
    public partial class frmLogin : Form
    {
        private TaiKhoanController _taiKhoanBUS;
        private NhanVienController _nhanVienBUS = new NhanVienController();
        private NhanVienModel _nhanVienDTO = new NhanVienModel();
        private bool isExiting = false;

        public frmLogin()
        {
            InitializeComponent();
            _taiKhoanBUS = new TaiKhoanController();
            this.FormClosing += frmLogin_FormClosing;
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            var user = _taiKhoanBUS.DangNhap(username, password);

            if (user != null)
            {
                if (!user.KichHoat)
                {
                    lblError.Text = "Tài khoản này chưa được kích hoạt!";
                    return;
                }

                Session.TaiKhoanHienTai = user;

                if (int.TryParse(user.MaTaiKhoan, out int maTaiKhoan))
                {
                    Session.NhanVienHienTai = _nhanVienBUS.LayTheoMaTaiKhoan(maTaiKhoan);
                }
                else
                {
                    MessageBox.Show("Lỗi: MaTaiKhoan không hợp lệ!");
                    return;
                }

                Form nextForm = null;

                if (user.VaiTro == "Admin")
                    nextForm = new frmAdmin();
                else if (user.VaiTro == "Kho")
                    nextForm = new frmNhapSach();
                else if (user.VaiTro == "BanHang")
                    nextForm = new frmHoaDon();

                if (nextForm != null)
                {
                    nextForm.Show();
                    this.Hide();
                }
            }
            else
            {
                lblError.Text = "Sai tên đăng nhập hoặc mật khẩu!";
            }
        }
 

        private void frmLogin_Load(object sender, EventArgs e)
        {
            ptbAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            txtUsername.KeyDown += txtUsername_KeyDown;
            txtPassword.KeyDown += txtPassword_KeyDown;
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void ptbAnh_Click(object sender, EventArgs e)
        {
        }
    }
}
