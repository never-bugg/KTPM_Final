using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Configuration;
using KTPM_Final.Controllers;
using KTPM_Final.Model;
namespace App
{
    public partial class frmSach : Form
    {
        SachController sachBUS = new SachController();
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";
        private bool isExiting = false; // Dùng để tránh xác nhận lặp

        public frmSach()
        {
            InitializeComponent();
            this.txtTuKhoa.TextChanged += txtTuKhoa_TextChanged;
            this.FormClosing += frmSach_FormClosing;
        }

        private void frmSach_FormClosing(object sender, FormClosingEventArgs e) // Xác nhận thoát
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
        private void LoadTheLoai() // Tải thể loại sách từ cơ sở dữ liệu
        {
            cboTheLoai.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT TheLoai FROM Sach WHERE TheLoai IS NOT NULL AND TheLoai <> ''";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cboTheLoai.Items.Add(reader["TheLoai"].ToString());
                    }
                }
            }
        }

        private void frmSach_Load(object sender, EventArgs e) // Tải danh sách sách và thể loại
        {
            DataTable dt = sachBUS.LayDanhSachSach();
            dgvSach.DataSource = dt;
            LoadTheLoai();

            var dsSach = sachBUS.LayDanhSachSach()
            .AsEnumerable()
            .Where(row => !row.IsNull("NgungKinhDoanh") && !Convert.ToBoolean(row["NgungKinhDoanh"]));

            DataTable dtKinhDoanh;
            if (dsSach.Any())
            {
                dtKinhDoanh = dsSach.CopyToDataTable();
            }
            else
            {
                dtKinhDoanh = sachBUS.LayDanhSachSach().Clone();
            }

            foreach (DataGridViewColumn column in dgvSach.Columns)
            {
                column.Visible = false;
            }
            if (dgvSach.Columns.Contains("MaSach")) dgvSach.Columns["MaSach"].Visible = true;
            if (dgvSach.Columns.Contains("TenSach")) dgvSach.Columns["TenSach"].Visible = true;

            if (!dgvSach.Columns.Contains("TrangThai"))
            {
                dgvSach.Columns.Add("TrangThai", "Trạng Thái");
            }

            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row == null || row.IsNewRow) continue;

                bool ngungKinhDoanh = false;
                if (dt.Columns.Contains("NgungKinhDoanh") && row.Cells["NgungKinhDoanh"].Value != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(row.Cells["NgungKinhDoanh"].Value);
                }
                if (ngungKinhDoanh)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.Cells["TrangThai"].Value = "Ngừng kinh doanh";
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells["TrangThai"].Value = "Đang kinh doanh";
                }
            }

            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = -1;
            nudSoLuong.Value = 0;

            dgvSach.ClearSelection();
            txtTuKhoa.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTuKhoa.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var autoComplete = new AutoCompleteStringCollection();
            foreach (DataRow dataRow in sachBUS.LayDanhSachSach().Rows)
            {
                autoComplete.Add(dataRow["TenSach"].ToString());
            }
            txtTuKhoa.AutoCompleteCustomSource = autoComplete;
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (cboTheLoai.Items.Count > 0)
                cboTheLoai.SelectedIndex = 0;

            nudSoLuong.Minimum = 0;
            nudSoLuong.Maximum = 1000000;

            pictureBoxAnh.SizeMode = PictureBoxSizeMode.StretchImage;


            dgvSach.SelectionChanged -= dgvSach_SelectionChanged;
            dgvSach.ClearSelection();
            dgvSach.SelectionChanged += dgvSach_SelectionChanged;

            txtGiaBan.ReadOnly = true;
            txtGiaNhap.ReadOnly = true;
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;

            btnKinhDoanhLai.Enabled = false;
            btnNgungKinhDoanh.Enabled = false;

            if (dgvSach.CurrentRow != null && dgvSach.CurrentRow.Index >= 0)
            {
                bool ngungKinhDoanh = false;
                var cellValue = dgvSach.CurrentRow.Cells["NgungKinhDoanh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(cellValue);
                }

                btnKinhDoanhLai.Enabled = ngungKinhDoanh;
                btnNgungKinhDoanh.Enabled = !ngungKinhDoanh;
            }
        }


        private void LoadSach()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh FROM Sach";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSach.DataSource = dt;
            }
        }

        private void ClearForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtGiaBan.ReadOnly = true;
            cboTheLoai.SelectedIndex = -1;
            nudSoLuong.Value = 0;
            txtAnh.Clear();
            txtNamXuatBan.Clear();
            txtNhaXuatBan.Clear();
            txtMoTa.Clear();
            pictureBoxAnh.Image = null;
        }


        private void LoadDanhSachSachVaTrangThai() // Tải danh sách sách và trạng thái từ cơ sở dữ liệu
        {
            DataTable dt = sachBUS.LayDanhSachSach();
            dgvSach.DataSource = dt;
            CapNhatTrangThaiSach();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(cboTheLoai.Text) ||
                string.IsNullOrWhiteSpace(txtTacGia.Text) ||    
                string.IsNullOrWhiteSpace(txtAnh.Text) ||   
                string.IsNullOrWhiteSpace(txtNamXuatBan.Text) ||
                string.IsNullOrWhiteSpace(txtNhaXuatBan.Text) ||
                string.IsNullOrWhiteSpace(txtMoTa.Text))
                    {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả thông tin!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(cboTheLoai.Text))
            {
                MessageBox.Show("Vui lòng chọn thể loại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal giaNhap = 0, giaBan = 0;
            int namXuatBan = 0, soLuong = (int)nudSoLuong.Value;
            if (!string.IsNullOrWhiteSpace(txtGiaNhap.Text) && !decimal.TryParse(txtGiaNhap.Text, out giaNhap))
            {
                MessageBox.Show("Giá nhập không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtGiaBan.Text) && !decimal.TryParse(txtGiaBan.Text, out giaBan))
            {
                MessageBox.Show("Giá bán không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtNamXuatBan.Text) && !int.TryParse(txtNamXuatBan.Text, out namXuatBan))
            {
                MessageBox.Show("Năm xuất bản không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SachModel sach = new SachModel
            {
                TenSach = txtTenSach.Text.Trim(),
                TacGia = txtTacGia.Text.Trim(),
                TheLoai = cboTheLoai.Text.Trim(),
                SoLuong = soLuong,
                GiaNhap = giaNhap,
                GiaBan = giaBan,
                Anh = txtAnh.Text,
                NamXuatBan = namXuatBan,
                NhaXuatBan = txtNhaXuatBan.Text.Trim(),
                MoTa = txtMoTa.Text.Trim()
            };

            if (sachBUS.ThemSach(sach))
            {
                MessageBox.Show("Thêm sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSach();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Thêm sách thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnThem.Enabled = false;
            LoadTheLoai();
            LoadDanhSachSachVaTrangThai();
        }

        private void btnThemTheLoai_Click(object sender, EventArgs e)
        {
            string theLoaiMoi = cboTheLoai.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(theLoaiMoi) && !cboTheLoai.Items.Contains(theLoaiMoi))
            {
                cboTheLoai.Items.Add(theLoaiMoi);
                cboTheLoai.SelectedItem = theLoaiMoi;
            }
            LoadTheLoai();

        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value.ToString();
                txtTenSach.Text = row.Cells["TenSach"].Value.ToString();
                txtTacGia.Text = row.Cells["TacGia"].Value.ToString();
                string theLoai = row.Cells["TheLoai"].Value?.ToString();
                if (!string.IsNullOrEmpty(theLoai))
                {
                    int idx = cboTheLoai.Items.IndexOf(theLoai);
                    if (idx >= 0)
                        cboTheLoai.SelectedIndex = idx;
                    else
                    {
                        cboTheLoai.Items.Add(theLoai);
                        cboTheLoai.SelectedItem = theLoai;
                    }
                }
                else
                {
                    cboTheLoai.SelectedIndex = -1;
                }

                nudSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuong"].Value);
                txtGiaNhap.Text = row.Cells["GiaNhap"].Value.ToString();
                txtGiaBan.Text = row.Cells["GiaBan"].Value.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSach.Text))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal giaNhap = 0, giaBan = 0;
            int namXuatBan = 0, soLuong = (int)nudSoLuong.Value;
            if (!string.IsNullOrWhiteSpace(txtGiaNhap.Text) && !decimal.TryParse(txtGiaNhap.Text, out giaNhap))
            {
                MessageBox.Show("Giá nhập không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtGiaBan.Text) && !decimal.TryParse(txtGiaBan.Text, out giaBan))
            {
                MessageBox.Show("Giá bán không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtNamXuatBan.Text) && !int.TryParse(txtNamXuatBan.Text, out namXuatBan))
            {
                MessageBox.Show("Năm xuất bản không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SachModel s = new SachModel
            {
                MaSach = txtMaSach.Text,
                TenSach = txtTenSach.Text,
                TacGia = txtTacGia.Text,
                TheLoai = cboTheLoai.SelectedItem?.ToString(),
                GiaNhap = giaNhap,
                GiaBan = giaBan,
                SoLuong = soLuong,
                Anh = txtAnh.Text,
                NamXuatBan = namXuatBan,
                NhaXuatBan = txtNhaXuatBan.Text.Trim(),
                MoTa = txtMoTa.Text.Trim()
            };

            if (sachBUS.SuaSach(s))
            {
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvSach.DataSource = sachBUS.LayDanhSachSach();
                // Hiển thị lại thông tin mới
                var sachMoi = sachBUS.LaySachTheoMa(s.MaSach);
                if (sachMoi != null)
                {
                    txtMaSach.Text = sachMoi.MaSach;
                    txtTenSach.Text = sachMoi.TenSach;
                    txtTacGia.Text = sachMoi.TacGia;
                    cboTheLoai.SelectedItem = sachMoi.TheLoai;
                    nudSoLuong.Value = sachMoi.SoLuong;
                    txtGiaNhap.Text = sachMoi.GiaNhap.ToString();
                    txtGiaBan.Text = sachMoi.GiaBan.ToString();
                    txtAnh.Text = sachMoi.Anh;
                    txtNamXuatBan.Text = sachMoi.NamXuatBan.ToString();
                    txtNhaXuatBan.Text = sachMoi.NhaXuatBan;
                    txtMoTa.Text = sachMoi.MoTa;
                    if (!string.IsNullOrEmpty(sachMoi.Anh) && System.IO.File.Exists(sachMoi.Anh))
                        pictureBoxAnh.Image = Image.FromFile(sachMoi.Anh);
                    else
                        pictureBoxAnh.Image = null;
                }
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtGiaBan.ReadOnly = true;
            txtGiaBan.BackColor = SystemColors.Control;
            LoadTheLoai();
            LoadDanhSachSachVaTrangThai();
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSach = txtMaSach.Text;
            if (string.IsNullOrEmpty(maSach))
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin sách
            var sach = sachBUS.LaySachTheoMa(maSach);
            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy thông tin sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra điều kiện: chỉ xóa nếu chưa từng nhập hàng (chưa có giá nhập)
            if (sach.GiaNhap > 0)
            {
                MessageBox.Show("Không thể xóa sách này vì đã có thông tin nhập hàng (đã có giá nhập).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (sachBUS.XoaSach(maSach))
                {
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvSach.DataSource = sachBUS.LayDanhSachSach();
                    ClearForm();
                    LoadDanhSachSachVaTrangThai();
                }
                else
                {
                    MessageBox.Show("Xóa sách thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;

            DataGridViewRow row = dgvSach.CurrentRow;
            txtMaSach.Text = row.Cells["MaSach"].Value?.ToString();
            txtTenSach.Text = row.Cells["TenSach"].Value?.ToString();
            txtTacGia.Text = row.Cells["TacGia"].Value?.ToString();
            string theLoai = row.Cells["TheLoai"].Value?.ToString();
            if (!string.IsNullOrEmpty(theLoai))
            {
                int idx = cboTheLoai.Items.IndexOf(theLoai);
                if (idx >= 0)
                    cboTheLoai.SelectedIndex = idx;
                else
                {
                    cboTheLoai.Items.Add(theLoai);
                    cboTheLoai.SelectedItem = theLoai;
                }
            }
            else
            {
                cboTheLoai.SelectedIndex = -1;
            }

            nudSoLuong.Value = row.Cells["SoLuong"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["SoLuong"].Value);
            txtGiaNhap.Text = row.Cells["GiaNhap"].Value?.ToString();
            txtGiaBan.Text = row.Cells["GiaBan"].Value?.ToString();

            // Kiểm tra cột "Anh" có tồn tại không trước khi truy cập
            if (dgvSach.Columns.Contains("Anh"))
                txtAnh.Text = row.Cells["Anh"].Value?.ToString();
            else
                txtAnh.Text = "";

            txtNamXuatBan.Text = dgvSach.Columns.Contains("NamXuatBan") ? row.Cells["NamXuatBan"].Value?.ToString() : "";
            txtNhaXuatBan.Text = dgvSach.Columns.Contains("NhaXuatBan") ? row.Cells["NhaXuatBan"].Value?.ToString() : "";
            txtMoTa.Text = dgvSach.Columns.Contains("MoTa") ? row.Cells["MoTa"].Value?.ToString() : "";

            if (!string.IsNullOrEmpty(txtAnh.Text) && System.IO.File.Exists(txtAnh.Text))
                pictureBoxAnh.Image = Image.FromFile(txtAnh.Text);
            else
                pictureBoxAnh.Image = null;

            btnKinhDoanhLai.Enabled = false;
            btnNgungKinhDoanh.Enabled = false;
            bool ngungKinhDoanh = false;
            if (dgvSach.Columns.Contains("NgungKinhDoanh"))
            {
                var cellValue = dgvSach.CurrentRow.Cells["NgungKinhDoanh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(cellValue);
                }
            }
            btnKinhDoanhLai.Enabled = ngungKinhDoanh;
            btnNgungKinhDoanh.Enabled = !ngungKinhDoanh;
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            ClearForm();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text;
            dgvSach.DataSource = sachBUS.TimKiemSach(tuKhoa);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Text = string.Empty;

            dgvSach.DataSource = sachBUS.LayDanhSachSach();
            ClearFormTimkiem();
            LoadDanhSachSachVaTrangThai();

        }
        private void ClearFormTimkiem()
        {
            txtTuKhoa.Clear();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            txtGiaBan.ReadOnly = false;
            txtGiaBan.BackColor = Color.White; 
            txtGiaBan.Focus();
        }


        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin adminForm = new frmAdmin();
            adminForm.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
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
            frmTaiKhoan frmTaiKhoan = new frmTaiKhoan();
            frmTaiKhoan.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon hoaDonForm = new frmXemHoaDon();
            hoaDonForm.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            
        }

        private void btnKinhDoanhLai_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;
            var rowView = dgvSach.CurrentRow.DataBoundItem as DataRowView;
            if (rowView != null && Convert.ToBoolean(rowView["NgungKinhDoanh"]))
            {
                string maSach = rowView["MaSach"].ToString();
                if (sachBUS.CapNhatTrangThaiNgungKinhDoanh(maSach,false))
                {
                    MessageBox.Show("Đã chuyển sách về trạng thái kinh doanh!");
                    DataTable dt = sachBUS.LayDanhSachSach();
                    dgvSach.DataSource = dt;

                    CapNhatTrangThaiSach();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            }
        }

        private void CapNhatTrangThaiSach()
        {
            if (!dgvSach.Columns.Contains("TrangThai"))
            {
                dgvSach.Columns.Add("TrangThai", "Trạng Thái");
            }

            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row.IsNewRow) continue;


                if (row.Cells["NgungKinhDoanh"].Value != null && row.Cells["NgungKinhDoanh"].Value != DBNull.Value)
                {
                    bool ngung = Convert.ToBoolean(row.Cells["NgungKinhDoanh"].Value);
                    row.DefaultCellStyle.BackColor = ngung ? Color.LightGray : Color.White;
                    row.Cells["TrangThai"].Value = ngung ? "Ngừng kinh doanh" : "Đang kinh doanh";
                }
            }
        }


        private void btnNgungKinhDoanh_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;
            var rowView = dgvSach.CurrentRow.DataBoundItem as DataRowView;
            if (rowView != null && !Convert.ToBoolean(rowView["NgungKinhDoanh"]))
            {
                string maSach = rowView["MaSach"].ToString();
                if (sachBUS.CapNhatTrangThaiNgungKinhDoanh(maSach, true))
                {
                    MessageBox.Show("Đã chuyển sách sang trạng thái ngừng kinh doanh!");
                    DataTable dt = sachBUS.LayDanhSachSach();
                    dgvSach.DataSource = dt;
                    CapNhatTrangThaiSach();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            }
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                dgvSach.DataSource = sachBUS.TimKiemSach(tuKhoa);
                CapNhatTrangThaiSach();

                foreach (DataGridViewColumn col in dgvSach.Columns)
                    col.Visible = false;
                if (dgvSach.Columns.Contains("MaSach")) dgvSach.Columns["MaSach"].Visible = true;
                if (dgvSach.Columns.Contains("TenSach")) dgvSach.Columns["TenSach"].Visible = true;
                if (dgvSach.Columns.Contains("TrangThai")) dgvSach.Columns["TrangThai"].Visible = true;
            }
            else
            {
                LoadDanhSachSachVaTrangThai();
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh sách";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxAnh.Image = Image.FromFile(ofd.FileName);
                    txtAnh.Text = ofd.FileName;
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}


