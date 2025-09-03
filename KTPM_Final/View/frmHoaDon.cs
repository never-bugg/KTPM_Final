using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KTPM_Final.Controllers;
using KTPM_Final.Model;

namespace App
{
    public partial class frmHoaDon : Form
    {
        private List<ChiTietHoaDonModel> gioHang = new List<ChiTietHoaDonModel>();
        private HoaDonController hoaDonBUS = new HoaDonController();
        private SachController sachBUS = new SachController();
        private NhanVienModel _nhanVienDTO = new NhanVienModel();
        private List<HoaDonModel> hoaDonHomNay = new List<HoaDonModel>();
        private bool isExiting = false; // ✅ Biến kiểm soát xác nhận thoát

        public frmHoaDon()
        {
            InitializeComponent();
            this.FormClosing += frmHoaDon_FormClosing; // ✅ Gắn sự kiện FormClosing
        }

        private void frmHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn thoát chương trình?",
                    "Xác nhận",
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

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadLichSuHoaDon();

            List<SachModel> dsSach = sachBUS.LayDanhSachSachDangKinhDoanh()
                .AsEnumerable()
                .Select(row => new SachModel
                {
                    MaSach = row["MaSach"].ToString(),
                    TenSach = row["TenSach"].ToString(),
                    TacGia = row["TacGia"].ToString(),
                    TheLoai = row["TheLoai"].ToString(),
                    GiaNhap = row.Table.Columns.Contains("GiaNhap") && row["GiaNhap"] != DBNull.Value ? Convert.ToDecimal(row["GiaNhap"]) : 0,
                    GiaBan = row.Table.Columns.Contains("GiaBan") && row["GiaBan"] != DBNull.Value ? Convert.ToDecimal(row["GiaBan"]) : 0,
                    SoLuong = row.Table.Columns.Contains("SoLuong") && row["SoLuong"] != DBNull.Value ? Convert.ToInt32(row["SoLuong"]) : 0,
                    NgungKinhDoanh = row.Table.Columns.Contains("NgungKinhDoanh") && row["NgungKinhDoanh"] != DBNull.Value ? Convert.ToBoolean(row["NgungKinhDoanh"]) : false
                })
                .Where(s => s.SoLuong > 0) // Chỉ lấy sách có số lượng > 0
                .ToList();

            cmbSach.DataSource = dsSach;
            cmbSach.DisplayMember = "TenSach";
            cmbSach.ValueMember = "MaSach";

            // Thiết lập AutoComplete cho ComboBox
            cmbSach.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSach.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            foreach (var sach in dsSach)
            {
                autoCompleteCollection.Add(sach.TenSach);
            }
            cmbSach.AutoCompleteCustomSource = autoCompleteCollection;

            cmbSach.SelectedIndex = -1;
            cmbSach.Text = "";

            cmbSach.SelectedIndexChanged += cmbSach_SelectedIndexChanged;
            txtSoLuong.TextChanged += txtSoLuong_TextChanged;

            // Load MaTaiKhoan từ bảng TaiKhoanNguoiDung (chỉ tài khoản chưa gán cho nhân viên)
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT MaTaiKhoan, TenDangNhap FROM TaiKhoanNguoiDung WHERE MaTaiKhoan NOT IN (SELECT MaTaiKhoan FROM NhanVien)", conn);
            DataTable dtTK = new DataTable();
            da.Fill(dtTK);
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

            dgvLichSuHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGioHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thiết lập AutoComplete cho txtTimKiem
            var danhSachHoaDon = hoaDonBUS.LayTatCaHoaDon()
                .Where(hd => hd.ThoiGianTao.Date == DateTime.Today)
                .ToList();
            hoaDonHomNay = danhSachHoaDon;

            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
            foreach (var hd in danhSachHoaDon)
            {
                autoComplete.Add(hd.MaHoaDon.ToString());
                if (!string.IsNullOrEmpty(hd.TenNhanVien))
                    autoComplete.Add(hd.TenNhanVien);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoComplete;

            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
        }
        private void LoadLichSuHoaDon()
        {
            var danhSachHoaDon = hoaDonBUS.LayTatCaHoaDon();

            // Lọc chỉ lấy hóa đơn tạo trong ngày hôm nay
            var today = DateTime.Today;
            var hoaDonHomNay = danhSachHoaDon
                .Where(hd => hd.ThoiGianTao.Date == today)
                .ToList();

            dgvLichSuHoaDon.DataSource = null;
            dgvLichSuHoaDon.DataSource = hoaDonHomNay;

            if (dgvLichSuHoaDon.Columns["MaNhanVien"] != null)
                dgvLichSuHoaDon.Columns["MaNhanVien"].Visible = false;

            if (dgvLichSuHoaDon.Columns["ThoiGianTao"] != null)
                dgvLichSuHoaDon.Columns["ThoiGianTao"].DefaultCellStyle.Format = "dd/MM/yyyy";
        
        }

        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            if (cmbSach.SelectedValue == null || cmbSach.SelectedValue is DataRowView)
                return;

            string maSach = cmbSach.SelectedValue.ToString();
            var sach = sachBUS.LaySachTheoMa(maSach);

            if (sach != null && int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                if (soLuong > sach.SoLuong)
                {
                    MessageBox.Show("Số lượng nhập không được vượt quá số lượng hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra sản phẩm đã có trong giỏ chưa
                var chiTiet = gioHang.FirstOrDefault(ct => ct.MaSach == maSach && ct.GiaBan == sach.GiaBan);
                if (chiTiet != null)
                {
                    // Nếu đã có thì cộng dồn số lượng
                    if (chiTiet.SoLuong + soLuong > sach.SoLuong)
                    {
                        MessageBox.Show("Tổng số lượng vượt quá số lượng hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    chiTiet.SoLuong += soLuong;
                }
                else
                {
                    gioHang.Add(new ChiTietHoaDonModel
                    {
                        MaSach = maSach,
                        SoLuong = soLuong,
                        GiaBan = sach.GiaBan
                    });
                }
                CapNhatGioHang();
            }
        }

        private void CapNhatGioHang()
        {
            var data = gioHang.Select(ct => new
            {
                TenSach = sachBUS.LaySachTheoMa(ct.MaSach)?.TenSach ?? ct.MaSach,
                SoLuong = ct.SoLuong,
                GiaBan = ct.GiaBan,
                ThanhTien = ct.SoLuong * ct.GiaBan
            }).ToList();

            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = data;
        }


        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null || dgvGioHang.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowIndex = dgvGioHang.CurrentRow.Index;
            if (rowIndex >= 0 && rowIndex < gioHang.Count)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    gioHang.RemoveAt(rowIndex);
                    CapNhatGioHang();
                }
            }
        }

        private void btnTaoHoaDon_Click(object sender, EventArgs e)
        {
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hoaDon = new HoaDonModel
            {
                TongTien = gioHang.Sum(ct => ct.SoLuong * ct.GiaBan),
                ThoiGianTao = DateTime.Now,
                TenNhanVien = Session.NhanVienHienTai.TenNhanVien,
                MaNhanVien = Session.NhanVienHienTai.MaNhanVien,
            };

            if (hoaDonBUS.ThemHoaDon(hoaDon))
            {
                foreach (var chiTiet in gioHang)
                {
                    chiTiet.MaHoaDon = hoaDon.MaHoaDon;
                    
                    // Lấy thông tin sách trước khi cập nhật
                    var sach = sachBUS.LaySachTheoMa(chiTiet.MaSach);
                    if (sach != null)
                    {
                        int soLuongMoi = sach.SoLuong - chiTiet.SoLuong;
                        if (soLuongMoi < 0)
                        {
                            MessageBox.Show($"Số lượng sản phẩm {sach.TenSach} không đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        // Thêm chi tiết hóa đơn (sẽ phát sự kiện bán sách)
                        new ChiTietHoaDonController().ThemChiTietHoaDon(chiTiet);
                        
                        // Cập nhật số lượng sách (sẽ phát sự kiện thay đổi tồn kho)
                        sachBUS.CapNhatSoLuong(chiTiet.MaSach, soLuongMoi);
                    }
                }

                MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XuatHoaDonPDF(hoaDon, gioHang);
                gioHang.Clear();
                CapNhatGioHang();

                // Reload toàn bộ form
                this.Controls.Clear();
                this.InitializeComponent();
                frmHoaDon_Load(null, null);
            }
            else
            {
                MessageBox.Show("Tạo hóa đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cmbSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSach.SelectedValue == null || cmbSach.SelectedValue is DataRowView)
                return;

            string maSach = cmbSach.SelectedValue.ToString();
            var sach = sachBUS.LaySachTheoMa(maSach);

            if (sach != null)
            {
                txtGiaBan.Text = sach.GiaBan.ToString("N0");
                lblSoLuongHienTai.Text = $"Số lượng hiện tại: {sach.SoLuong}";
                txtSoLuong.Text = "1"; // Chỉ gán khi chọn sách hợp lệ
            }
        }
        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (cmbSach.SelectedValue == null || cmbSach.SelectedValue is DataRowView)
                return;

            string maSach = cmbSach.SelectedValue.ToString();
            var sach = sachBUS.LaySachTheoMa(maSach);

            if (sach != null)
            {
                if (int.TryParse(txtSoLuong.Text, out int soLuongNhap))
                {
                    // Kiểm tra số lượng <= 0
                    if (soLuongNhap <= 0)
                    {
                        MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Text = "1";
                        txtSoLuong.SelectionStart = txtSoLuong.Text.Length;
                        return;
                    }
                    if (soLuongNhap > sach.SoLuong)
                    {
                        MessageBox.Show("Số lượng nhập không được vượt quá số lượng hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Text = sach.SoLuong.ToString();
                        txtSoLuong.SelectionStart = txtSoLuong.Text.Length;
                    }
                    else
                    {
                        decimal giaBan = sach.GiaBan;
                        decimal tongTien = soLuongNhap * giaBan;
                        txtTongTien.Text = tongTien.ToString("N0");
                    }
                }
                else
                {
                    txtTongTien.Text = "0";
                }
            }
        }

        private void XuatHoaDonPDF(HoaDonModel hoaDon, List<ChiTietHoaDonModel> chiTietList)
        {
            // Đường dẫn thư mục cố định
            string folderPath = Path.Combine(Application.StartupPath, "HoaDonPDF");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"HoaDon_{hoaDon.MaHoaDon}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(folderPath, fileName);

            // Có thể dùng font mặc định vì không còn ký tự đặc biệt
            var font = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL);
            var fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, iTextSharp.text.Font.BOLD);

            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // Tiêu đề
            Paragraph title = new Paragraph(RemoveDiacritics("HOA DON BAN SACH"), fontBold);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);
            doc.Add(new Paragraph(RemoveDiacritics($"Ma hoa don: {hoaDon.MaHoaDon}"), font));
            doc.Add(new Paragraph(RemoveDiacritics($"Ngay lap: {hoaDon.ThoiGianTao:dd/MM/yyyy HH:mm}"), font));
            doc.Add(new Paragraph(RemoveDiacritics($"Nhan vien: {hoaDon.TenNhanVien}"), font));
            doc.Add(new Paragraph(RemoveDiacritics($"Tong tien: {hoaDon.TongTien:N0} VND"), font));
            doc.Add(new Paragraph(" ", font));

            // Bảng chi tiết
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.AddCell(new PdfPCell(new Phrase(RemoveDiacritics("Ten sach"), fontBold)));
            table.AddCell(new PdfPCell(new Phrase(RemoveDiacritics("So luong"), fontBold)));
            table.AddCell(new PdfPCell(new Phrase(RemoveDiacritics("Don gia"), fontBold)));
            table.AddCell(new PdfPCell(new Phrase(RemoveDiacritics("Thanh tien"), fontBold)));

            foreach (var ct in chiTietList)
            {
                var sach = sachBUS.LaySachTheoMa(ct.MaSach);
                table.AddCell(new Phrase(RemoveDiacritics(sach?.TenSach ?? ct.MaSach), font));
                table.AddCell(new Phrase(ct.SoLuong.ToString(), font));
                table.AddCell(new Phrase(ct.GiaBan.ToString("N0"), font));
                table.AddCell(new Phrase((ct.SoLuong * ct.GiaBan).ToString("N0"), font));
            }
            doc.Add(table);

            doc.Close();
            MessageBox.Show("Xuất hóa đơn PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Tự động mở file PDF sau khi xuất
            System.Diagnostics.Process.Start(filePath);
        }




        private string RemoveDiacritics(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void dgvLichSuHoaDon_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLichSuHoaDon.Rows[e.RowIndex];
                int maHoaDon = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
                frmChiTietHoaDon frm = new frmChiTietHoaDon(maHoaDon);
                frm.ShowDialog();
            }
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đã trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                gioHang.Clear();
                CapNhatGioHang();
            }
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }
        private int selectedGioHangIndex = -1;

        private void dgvGioHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gioHang.Count)
            {
                selectedGioHangIndex = e.RowIndex;
                var chiTiet = gioHang[selectedGioHangIndex];
                // Hiển thị lên các textbox
                cmbSach.SelectedValue = chiTiet.MaSach;
                txtSoLuong.Text = chiTiet.SoLuong.ToString();
                txtGiaBan.Text = chiTiet.GiaBan.ToString("N0");
                btnSua.Enabled = true;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedGioHangIndex >= 0 && selectedGioHangIndex < gioHang.Count)
            {
                var chiTiet = gioHang[selectedGioHangIndex];
                // Cập nhật lại thông tin
                chiTiet.MaSach = cmbSach.SelectedValue.ToString();
                int soLuong;
                decimal giaBan;
                if (int.TryParse(txtSoLuong.Text, out soLuong))
                    chiTiet.SoLuong = soLuong;
                if (decimal.TryParse(txtGiaBan.Text, out giaBan))
                    chiTiet.GiaBan = giaBan;
                CapNhatGioHang();
                selectedGioHangIndex = -1;
            }
        }
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimKiemHoaDon();
            }
        }
        private void TimKiemHoaDon()
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            var ketQua = hoaDonHomNay.Where(hd =>
                hd.MaHoaDon.ToString().Contains(tuKhoa) ||
                (!string.IsNullOrEmpty(hd.TenNhanVien) && hd.TenNhanVien.ToLower().Contains(tuKhoa)) ||
                hd.TongTien.ToString("N0").Replace(",", "").Contains(tuKhoa.Replace(",", ""))
            ).ToList();

            dgvLichSuHoaDon.DataSource = null;
            dgvLichSuHoaDon.DataSource = ketQua;

            if (dgvLichSuHoaDon.Columns["MaNhanVien"] != null)
                dgvLichSuHoaDon.Columns["MaNhanVien"].Visible = false;
            if (dgvLichSuHoaDon.Columns["ThoiGianTao"] != null)
                dgvLichSuHoaDon.Columns["ThoiGianTao"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }


        private void mnSach_Click(object sender, EventArgs e)
        {
            frmBHXemTTSach frm = new frmBHXemTTSach();
            frm.Show();
            this.Hide();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            LoadLichSuHoaDon();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}