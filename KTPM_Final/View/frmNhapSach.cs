using KTPM_Final.Controllers;
using KTPM_Final.Model;
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

namespace App
{
    public partial class frmNhapSach : Form
    {
        private List<SachModel> canhBaoList = new List<SachModel>();
        private bool isExiting = false;
      

        public frmNhapSach()
        {
            InitializeComponent();
            dgvDanhSach.SelectionChanged += dgvDanhSach_SelectionChanged;
            dgvDanhSach.CellEndEdit += dgvDanhSach_CellEndEdit;
            cbSach.SelectedIndexChanged += cbSach_SelectedIndexChanged;
            txtSoLuong.KeyPress += txtSoLuong_KeyPress;
            txtSoLuong.Leave += txtSoLuong_Leave;


        }

        private void frmNhapSach_Load(object sender, EventArgs e)
        {
            // Add columns if not already present
            if (dgvDanhSach.Columns.Count == 0)
            {
                dgvDanhSach.Columns.Add("MaSach", "Mã Sách");
                dgvDanhSach.Columns.Add("TenSach", "Tên Sách");
                dgvDanhSach.Columns.Add("SoLuong", "Số Lượng");
                dgvDanhSach.Columns.Add("GiaNhap", "Giá Nhập");
            }

            SachController sachBUS = new SachController(); // First instance of sachBUS
            DataTable dt = sachBUS.LayDanhSachSachDangKinhDoanh();
            cbSach.DataSource = dt;
            cbSach.DisplayMember = "TenSach";
            cbSach.ValueMember = "MaSach";

            // Không chọn gì khi mới vào
            cbSach.SelectedIndex = -1;
            cbSach.Text = "";

            // Thiết lập AutoComplete
            cbSach.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbSach.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            foreach (DataRow row in dt.Rows)
            {
                auto.Add(row["TenSach"].ToString());
            }
            cbSach.AutoCompleteCustomSource = auto;

            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

            // Reuse the existing sachBUS instance instead of redeclaring it
            var canhBaoList = sachBUS.LaySachTonKhoDuoi10();
            dgvCanhBaoHangTon.DataSource = canhBaoList
                .Select(s => new { s.MaSach, s.TenSach, s.SoLuong })
                .ToList();

            dgvCanhBaoHangTon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDanhSach.ReadOnly = false;
            dgvDanhSach.Columns["MaSach"].ReadOnly = true;
            dgvDanhSach.Columns["TenSach"].ReadOnly = true;
        }


        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSach.SelectedValue != null && cbSach.SelectedIndex != -1)
            {
                txtSoLuong.Text = "1"; // Chỉ hiện 1 khi chọn sách
            }
            else
            {
                txtSoLuong.Text = ""; // Không chọn thì để trống
            }

            if (cbSach.SelectedValue == null || cbSach.SelectedIndex == -1)
            {
                txtGiaNhap.Text = "";
                txtGiaNhap.Enabled = false;
                return;
            }

            string maSach = cbSach.SelectedValue.ToString();
            var sach = new SachController().LaySachTheoMa(maSach);
            if (sach != null)
            {
                txtGiaNhap.Text = sach.GiaNhap > 0 ? sach.GiaNhap.ToString() : "";
                txtGiaNhap.Enabled = sach.GiaNhap == 0;
            }
        }


        private void btnDoiGia_Click(object sender, EventArgs e)
        {
            // Chỉ bật cho phép nhập giá, không xử lý cập nhật ở đây
            txtGiaNhap.Enabled = true;
            txtGiaNhap.Focus();
        }

        // Thêm sự kiện xử lý khi người dùng nhập xong giá mới (ví dụ khi nhấn Enter hoặc rời khỏi ô nhập giá)
        private void txtGiaNhap_Leave(object sender, EventArgs e)
        {
            if (!txtGiaNhap.Enabled) return;

            if (cbSach.SelectedValue == null)
                return;

            string maSach = cbSach.SelectedValue.ToString();

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhapMoi))
                return;

            decimal giaBanMoi = Math.Round(giaNhapMoi * 1.5m, 0);

            SachController sachBUS = new SachController();
            var sach = sachBUS.LaySachTheoMa(maSach);
            if (sach == null)
                return;

            sach.GiaNhap = giaNhapMoi;
            sach.GiaBan = giaBanMoi;

            sachBUS.SuaSach(sach);
            txtGiaNhap.Enabled = false;
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSach = cbSach.SelectedValue.ToString();
            string tenSach = ((DataRowView)cbSach.SelectedItem)["TenSach"].ToString();

            if (!int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap))
            {
                MessageBox.Show("Giá nhập không hợp lệ.");
                return;
            }

            // Kiểm tra sản phẩm đã có trong dgvDanhSach chưa
            bool found = false;
            foreach (DataGridViewRow row in dgvDanhSach.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["MaSach"].Value?.ToString() == maSach)
                {
                    // Nếu đã có, cộng dồn số lượng và cập nhật giá nhập mới nhất
                    int oldSoLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    row.Cells["SoLuong"].Value = oldSoLuong + soLuong;
                    row.Cells["GiaNhap"].Value = giaNhap; // Lấy giá nhập lần sau
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dgvDanhSach.Rows.Add(maSach, tenSach, soLuong, giaNhap);
            }

            cbSach.SelectedIndex = -1;
            cbSach.Text = "";
            txtSoLuong.Text = ""; 
            txtGiaNhap.Text = "";
            txtGiaNhap.Enabled = false;

        }


        private void btnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.Rows.Count == 0 || dgvDanhSach.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sách để tạo phiếu nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ======= XÁC NHẬN DANH SÁCH SẢN PHẨM =======
            StringBuilder confirmMsg = new StringBuilder();
            confirmMsg.AppendLine("Xác nhận nhập các sản phẩm sau:\n");
            foreach (DataGridViewRow row in dgvDanhSach.Rows)
            {
                if (row.IsNewRow) continue;
                string tenSach = row.Cells["TenSach"].Value?.ToString();
                string soLuong = row.Cells["SoLuong"].Value?.ToString();
                string giaNhap = row.Cells["GiaNhap"].Value?.ToString();
                confirmMsg.AppendLine($"- {tenSach} | SL: {soLuong} | Giá nhập: {giaNhap}");
            }
            confirmMsg.AppendLine("\nBạn có chắc chắn muốn tạo phiếu nhập không?");

            DialogResult result = MessageBox.Show(
                confirmMsg.ToString(),
                "Xác nhận tạo phiếu nhập",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.OK)
                return;
            // ======= KẾT THÚC XÁC NHẬN =======

            NhapSachController nhapBUS = new NhapSachController();
            ChiTietNhapSachController ctBUS = new ChiTietNhapSachController();
            SachController sachBUS = new SachController(); // Reuse this instance instead of redeclaring later

            decimal tongTien = 0;

            foreach (DataGridViewRow row in dgvDanhSach.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["SoLuong"].Value == null || row.Cells["GiaNhap"].Value == null)
                {
                    MessageBox.Show("Một hoặc nhiều dòng có ô trống. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                tongTien += Convert.ToInt32(row.Cells["SoLuong"].Value) * Convert.ToDecimal(row.Cells["GiaNhap"].Value);
            }

            if (Session.NhanVienHienTai == null)
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NhapSachModel phieu = new NhapSachModel
            {
                NgayNhap = DateTime.Now,
                MaNhanVien = Session.NhanVienHienTai.MaNhanVien,
                TongTien = tongTien
            };

            int maPhieu = nhapBUS.ThemPhieuNhap(phieu);

            foreach (DataGridViewRow row in dgvDanhSach.Rows)
            {
                if (row.IsNewRow) continue;

                string maSach = row.Cells["MaSach"].Value?.ToString();
                if (string.IsNullOrEmpty(maSach)) continue;

                decimal giaNhapMoi = Convert.ToDecimal(row.Cells["GiaNhap"].Value);

                // Lấy sách hiện tại từ DB
                var sach = sachBUS.LaySachTheoMa(maSach);
                if (sach != null && sach.GiaNhap != giaNhapMoi)
                {
                    sach.GiaNhap = giaNhapMoi;
                    sach.GiaBan = Math.Round(giaNhapMoi * 1.5m, 0);
                    sachBUS.SuaSach(sach);
                }

                ChiTietNhapSachModel ct = new ChiTietNhapSachModel
                {
                    MaPhieuNhap = maPhieu,
                    MaSach = Convert.ToInt32(maSach),
                    SoLuong = Convert.ToInt32(row.Cells["SoLuong"].Value),
                    GiaNhap = giaNhapMoi
                };

                ctBUS.ThemChiTiet(ct);

                // Cập nhật tồn kho
                sachBUS.CapNhatSoLuong(maSach, sach.SoLuong + ct.SoLuong);
            }

            MessageBox.Show("Tạo phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvDanhSach.Rows.Clear();
            // Load lại cảnh báo tồn kho sau khi nhập thành công
            var canhBaoList = sachBUS.LaySachTonKhoDuoi10();
            dgvCanhBaoHangTon.DataSource = canhBaoList
                .Select(s => new { s.MaSach, s.TenSach, s.SoLuong })
                .ToList();
        }



        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach sachForm = new frmSach();
            sachForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow != null && !dgvDanhSach.CurrentRow.IsNewRow)
            {
                dgvDanhSach.Rows.RemoveAt(dgvDanhSach.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmNhapSach_FormClosing(object sender, FormClosingEventArgs e)
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


        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            this.Hide();
        }

        private void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
        {
            btnSua.Enabled = dgvDanhSach.CurrentRow != null && !dgvDanhSach.CurrentRow.IsNewRow;

        }

        private void dgvDanhSach_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvDanhSach.Rows[e.RowIndex];
            if (!int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out _) ||
                !decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out _))
            {
                MessageBox.Show("Số lượng hoặc giá nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Có thể reset lại giá trị cũ nếu muốn
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.CurrentRow != null && !dgvDanhSach.CurrentRow.IsNewRow)
            {
                dgvDanhSach.BeginEdit(true);
            }
        }

        private void dgvCanhBaoHangTon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tHÔNGTINSÁCHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKhoXemTTSach khoForm = new frmKhoXemTTSach();
            khoForm.Show();
            this.Hide();
        }

        private void cbSach_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSoLuong_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoLuong.Text, out int value) || value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Text = "1";
                txtSoLuong.Focus();
            }
        }
    }
}
