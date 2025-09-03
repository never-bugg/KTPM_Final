using KTPM_Final.Controllers;
using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class frmNhanVien : Form
    {
        NhanVienController bus = new NhanVienController();
        private bool isExiting = false;

        public frmNhanVien()
        {
            InitializeComponent();
            this.FormClosing += frmNhanVien_FormClosing; 
        }

        private void frmNhanVien_FormClosing(object sender, FormClosingEventArgs e)
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

        private void LoadData()
        {
            dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
            dgvNhanVien.Columns["MaNhanVien"].Visible = false;
            dgvNhanVien.Columns["Email"].Visible = false;
            dgvNhanVien.Columns["MaTaiKhoan"].Visible = false;
            ResetForm();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();

            // Cài đặt ComboBox loại nhân viên
            cboLoaiNhanVien.Items.Add("Kho");
            cboLoaiNhanVien.Items.Add("BanHang");

            // Load MaTaiKhoan từ bảng TaiKhoanNguoiDung (chỉ tài khoản chưa gán cho nhân viên)
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT MaTaiKhoan, TenDangNhap FROM TaiKhoanNguoiDung WHERE MaTaiKhoan NOT IN (SELECT MaTaiKhoan FROM NhanVien)", conn);
            DataTable dtTK = new DataTable();
            da.Fill(dtTK);
            cboMaTaiKhoan.DataSource = dtTK;
            cboMaTaiKhoan.DisplayMember = "TenDangNhap";
            cboMaTaiKhoan.ValueMember = "MaTaiKhoan";
            LoadData();
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
            LoadTaiKhoanChuaGanNhanVien();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.Cells["TenNhanVien"].Value != null)
                    autoComplete.Add(row.Cells["TenNhanVien"].Value.ToString());
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoComplete;
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTenNhanVien.Text = dgvNhanVien.Rows[e.RowIndex].Cells["TenNhanVien"].Value.ToString();
                txtSoDienThoai.Text = dgvNhanVien.Rows[e.RowIndex].Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = dgvNhanVien.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                txtChucVu.Text = dgvNhanVien.Rows[e.RowIndex].Cells["ChucVu"].Value.ToString();
                cboLoaiNhanVien.Text = dgvNhanVien.Rows[e.RowIndex].Cells["LoaiNhanVien"].Value.ToString();
                cboMaTaiKhoan.SelectedValue = dgvNhanVien.Rows[e.RowIndex].Cells["MaTaiKhoan"].Value;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!IsValidPhoneNumber(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số!");
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không đúng định dạng!");
                return;
            }

            if (!IsValidName(txtTenNhanVien.Text))
            {
                MessageBox.Show("Tên nhân viên phải chứa ký tự chữ!");
                return;
            }

            NhanVienModel nv = new NhanVienModel
            {
                TenNhanVien = txtTenNhanVien.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                ChucVu = txtChucVu.Text,
                LoaiNhanVien = cboLoaiNhanVien.Text,
                MaTaiKhoan = Convert.ToInt32(cboMaTaiKhoan.SelectedValue)
            };

            if (bus.ThemNhanVien(nv))
            {
                MessageBox.Show("Thêm thành công!");
                dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
                ResetForm();
                LoadData();
            }
            else
                MessageBox.Show("Thêm thất bại!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!IsValidPhoneNumber(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số!");
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không đúng định dạng!");
                return;
            }

            if (!IsValidName(txtTenNhanVien.Text))
            {
                MessageBox.Show("Tên nhân viên phải chứa ký tự chữ!");
                return;
            }

            if (dgvNhanVien.CurrentRow != null)
            {
                int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value);
                NhanVienModel nv = new NhanVienModel
                {
                    MaNhanVien = maNV,
                    TenNhanVien = txtTenNhanVien.Text,
                    SoDienThoai = txtSoDienThoai.Text,
                    Email = txtEmail.Text,
                    ChucVu = txtChucVu.Text,
                    LoaiNhanVien = cboLoaiNhanVien.Text,
                    MaTaiKhoan = Convert.ToInt32(cboMaTaiKhoan.SelectedValue)
                };

                if (bus.CapNhatNhanVien(nv))
                {
                    MessageBox.Show("Sửa thành công!");
                    dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
                    ResetForm();
                    LoadData();
                }
                else
                    MessageBox.Show("Sửa thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value);
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (bus.XoaNhanVien(maNV))
                    {
                        MessageBox.Show("Xóa thành công!");
                        dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
                        ResetForm();
                        LoadData();
                    }
                    else
                        MessageBox.Show("Xóa thất bại!");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
        }

        private void ResetForm()
        {
            txtTenNhanVien.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtChucVu.Clear();
            cboLoaiNhanVien.SelectedIndex = -1;
            cboMaTaiKhoan.SelectedIndex = -1;
            dgvNhanVien.ClearSelection();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }

        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtChucVu.Text = row.Cells["ChucVu"].Value?.ToString();
                cboLoaiNhanVien.Text = row.Cells["LoaiNhanVien"].Value?.ToString();
                cboMaTaiKhoan.SelectedValue = row.Cells["MaTaiKhoan"].Value;
                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void tnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            frmQuickAddAccount frm = new frmQuickAddAccount();

            frm.OnTaiKhoanAdded += (string tenDangNhapMoi) =>
            {
                LoadTaiKhoanChuaGanNhanVien();

                // Tự động chọn lại tài khoản vừa thêm
                for (int i = 0; i < cboMaTaiKhoan.Items.Count; i++)
                {
                    var row = (DataRowView)cboMaTaiKhoan.Items[i];
                    if (row["TenDangNhap"].ToString() == tenDangNhapMoi)
                    {
                        cboMaTaiKhoan.SelectedIndex = i;
                        break;
                    }
                }
            };
            frm.Show(this);
        }

        private void LoadTaiKhoanChuaGanNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                string query = "SELECT MaTaiKhoan, TenDangNhap FROM TaiKhoanNguoiDung WHERE MaTaiKhoan NOT IN (SELECT MaTaiKhoan FROM NhanVien WHERE MaTaiKhoan IS NOT NULL)";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dtTK = new DataTable();
                da.Fill(dtTK);
                cboMaTaiKhoan.DataSource = dtTK;
                cboMaTaiKhoan.DisplayMember = "TenDangNhap";
                cboMaTaiKhoan.ValueMember = "MaTaiKhoan";
                cboMaTaiKhoan.SelectedIndex = -1;
            }
        }

        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach frm = new frmSach();
            frm.Show();
            this.Hide();
        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin();
            frm.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhap phieuNhapForm = new frmXemPhieuNhap();
            phieuNhapForm.Show();
            this.Hide();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan frm = new frmTaiKhoan();
            frm.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon frm = new frmXemHoaDon();
            frm.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                DataTable dt = bus.LayDanhSachNhanVien();
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"TenNhanVien LIKE '%{keyword}%'";
                dgvNhanVien.DataSource = dv.ToTable();
            }
            else
            {
                dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
            }
        }

        private bool IsValidName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, @"[a-zA-ZÀ-ỹà-ỹ\s]+");
        }
    }
}
