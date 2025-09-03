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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace App
{
    public partial class frmAdmin : Form
    {
        private NhanVienModel _nhanVienDTO = new NhanVienModel();
        private bool isExiting = false; 

        public frmAdmin()
        {
            InitializeComponent();

            this.FormClosing += frmAdmin_FormClosing; // Gắn sự kiện xác nhận
        }
       

        private void Button_MouseEnter(object sender, EventArgs e) //hàm để khi chuột di chuyển vào button
        {
            Button btn = sender as Button; 
            btn.BackColor = Color.LightSkyBlue; 
        }

        private void Button_MouseLeave(object sender, EventArgs e) //hàm để khi chuột rời khỏi button
        {
            Button btn = sender as Button; 
            btn.BackColor = SystemColors.Control;
        }

        private void frmAdmin_Load(object sender, EventArgs e) 
        {
            btnSach.MouseEnter += Button_MouseEnter;
            btnSach.MouseLeave += Button_MouseLeave;
            btnNhap.MouseEnter += Button_MouseEnter;
            btnNhap.MouseLeave += Button_MouseLeave;
            btnNhanVien.MouseEnter += Button_MouseEnter;
            btnNhanVien.MouseLeave += Button_MouseLeave;
            btnTaiKhoan.MouseEnter += Button_MouseEnter;
            btnTaiKhoan.MouseLeave += Button_MouseLeave;

            btnHoaDon.MouseEnter += Button_MouseEnter;
            btnHoaDon.MouseLeave += Button_MouseLeave;

            if (Session.NhanVienHienTai != null)
            {
                mn1.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void btnSach_Click(object sender, EventArgs e)
        {
            frmSach sachForm = new frmSach();
            sachForm.Show();
            this.Hide();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
          frmXemPhieuNhap phieuNhapForm = new frmXemPhieuNhap();
            phieuNhapForm.Show();
            this.Hide();    
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVienForm = new frmNhanVien();
            nhanVienForm.Show();
            this.Hide();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoanForm = new frmTaiKhoan();
            taiKhoanForm.Show();
            this.Hide();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon xemHoaDonForm = new frmXemHoaDon();
            xemHoaDonForm.Show();
            this.Hide();
        }

        private void btnDoanhthu_Click(object sender, EventArgs e)
        {
            
        }

        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e) // Xác nhận thoát
        {

            if (!isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình?",
                    "Xác nhận thoát",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

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

        private void đĂNGXUẤTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            this.Hide();
        }
    }
}

