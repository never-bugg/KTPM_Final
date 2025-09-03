using System;
using System.Windows.Forms;
using KTPM_Final.Observer;
using KTPM_Final.Observer.Events;
using KTPM_Final.Observer.Observers;

namespace KTPM_Final.Demo
{
    /// <summary>
    /// Form demo Observer Pattern
    /// </summary>
    public partial class frmObserverDemo : Form
    {
        private UIUpdateObserver _uiObserver;
        private LogObserver _customLogObserver;

        public frmObserverDemo()
        {
            InitializeComponent();
            InitializeObservers();
        }

        private void InitializeObservers()
        {
            // Tạo UI Observer với callback cho form này
            _uiObserver = new UIUpdateObserver(
                updateStatusBar: UpdateStatusBar,
                updateInventoryWarning: ShowInventoryWarning
            );

            // Tạo log observer riêng cho demo
            _customLogObserver = new LogObserver("logs/demo_events.log");

            // Đăng ký observers
            ObserverManager.Instance.AddCustomObserver(_uiObserver);
            ObserverManager.Instance.AddCustomObserver(_customLogObserver);
        }

        private void UpdateStatusBar(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatusBar), message);
                return;
            }
            
            lblStatus.Text = $"[{DateTime.Now:HH:mm:ss}] {message}";
            lstEvents.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
            
            // Giữ tối đa 50 items
            while (lstEvents.Items.Count > 50)
            {
                lstEvents.Items.RemoveAt(lstEvents.Items.Count - 1);
            }
        }

        private void ShowInventoryWarning(string tenSach, string warning)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(ShowInventoryWarning), tenSach, warning);
                return;
            }

            string message = $"⚠️ CẢNH BÁO: {tenSach} - {warning}";
            lstWarnings.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
            
            // Highlight warning
            if (lstWarnings.Items.Count > 0)
            {
                lstWarnings.BackColor = System.Drawing.Color.LightYellow;
                lstWarnings.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnTestSachDaBan_Click(object sender, EventArgs e)
        {
            // Test sự kiện bán sách
            ObserverManager.Instance.NotifySachDaBan(
                maSach: "1",
                tenSach: "Harry Potter và Hòn đá Phù thủy",
                soLuongBan: 3,
                soLuongConLai: 7,
                giaBan: 150000,
                maHoaDon: 123
            );
        }

        private void btnTestSachDaNhap_Click(object sender, EventArgs e)
        {
            // Test sự kiện nhập sách
            ObserverManager.Instance.NotifySachDaNhap(
                maSach: "2",
                tenSach: "Dạy con làm giàu",
                soLuongNhap: 20,
                soLuongSauNhap: 45,
                giaNhap: 80000,
                maPhieuNhap: 456
            );
        }

        private void btnTestHoaDonDaTao_Click(object sender, EventArgs e)
        {
            // Test sự kiện tạo hóa đơn
            ObserverManager.Instance.NotifyHoaDonDaTao(
                maHoaDon: 789,
                tongTien: 450000,
                thoiGianTao: DateTime.Now,
                tenNhanVien: "Nguyễn Văn A",
                soLuongSanPham: 3
            );
        }

        private void btnTestSachSapHet_Click(object sender, EventArgs e)
        {
            // Test cảnh báo sách sắp hết
            ObserverManager.Instance.NotifySachSapHetHang(
                maSach: "3",
                tenSach: "Nhà giả kim",
                soLuongHienTai: 5
            );
        }

        private void btnTestSachHetHang_Click(object sender, EventArgs e)
        {
            // Test cảnh báo hết hàng
            ObserverManager.Instance.NotifySachHetHang(
                maSach: "4",
                tenSach: "Đắc nhân tâm"
            );
        }

        private void btnTestSachCoHang_Click(object sender, EventArgs e)
        {
            // Test sách có hàng trở lại
            ObserverManager.Instance.NotifySachCoHangTroLai(
                maSach: "4",
                tenSach: "Đắc nhân tâm",
                soLuongHienTai: 15
            );
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            lstEvents.Items.Clear();
            lstWarnings.Items.Clear();
            lstWarnings.BackColor = System.Drawing.SystemColors.Window;
            lstWarnings.ForeColor = System.Drawing.SystemColors.WindowText;
            lblStatus.Text = "Đã xóa tất cả log";
        }

        private void btnShowObserverCount_Click(object sender, EventArgs e)
        {
            int count = ObserverManager.Instance.ObserverCount;
            MessageBox.Show($"Hiện tại có {count} observers đã đăng ký.", 
                          "Thông tin Observer", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Information);
        }

        private void frmObserverDemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Hủy đăng ký observers khi đóng form
            ObserverManager.Instance.RemoveCustomObserver(_uiObserver);
            ObserverManager.Instance.RemoveCustomObserver(_customLogObserver);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.grpEvents = new GroupBox();
            this.btnTestSachDaBan = new Button();
            this.btnTestSachDaNhap = new Button();
            this.btnTestHoaDonDaTao = new Button();
            this.btnTestSachSapHet = new Button();
            this.btnTestSachHetHang = new Button();
            this.btnTestSachCoHang = new Button();
            this.grpLogs = new GroupBox();
            this.lstEvents = new ListBox();
            this.grpWarnings = new GroupBox();
            this.lstWarnings = new ListBox();
            this.lblStatus = new Label();
            this.btnClearLogs = new Button();
            this.btnShowObserverCount = new Button();

            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Size = new System.Drawing.Size(760, 30);
            this.lblTitle.Text = "DEMO OBSERVER PATTERN - HỆ THỐNG QUẢN LÝ NHÀ SÁCH";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // grpEvents
            this.grpEvents.Location = new System.Drawing.Point(12, 50);
            this.grpEvents.Size = new System.Drawing.Size(200, 300);
            this.grpEvents.Text = "Test Events";
            this.grpEvents.Controls.AddRange(new Control[] {
                this.btnTestSachDaBan, this.btnTestSachDaNhap, this.btnTestHoaDonDaTao,
                this.btnTestSachSapHet, this.btnTestSachHetHang, this.btnTestSachCoHang
            });

            // Buttons
            int buttonY = 25;
            this.btnTestSachDaBan.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestSachDaBan.Size = new System.Drawing.Size(180, 30);
            this.btnTestSachDaBan.Text = "Test: Bán sách";
            this.btnTestSachDaBan.Click += btnTestSachDaBan_Click;

            buttonY += 40;
            this.btnTestSachDaNhap.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestSachDaNhap.Size = new System.Drawing.Size(180, 30);
            this.btnTestSachDaNhap.Text = "Test: Nhập sách";
            this.btnTestSachDaNhap.Click += btnTestSachDaNhap_Click;

            buttonY += 40;
            this.btnTestHoaDonDaTao.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestHoaDonDaTao.Size = new System.Drawing.Size(180, 30);
            this.btnTestHoaDonDaTao.Text = "Test: Tạo hóa đơn";
            this.btnTestHoaDonDaTao.Click += btnTestHoaDonDaTao_Click;

            buttonY += 40;
            this.btnTestSachSapHet.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestSachSapHet.Size = new System.Drawing.Size(180, 30);
            this.btnTestSachSapHet.Text = "Test: Sách sắp hết";
            this.btnTestSachSapHet.Click += btnTestSachSapHet_Click;

            buttonY += 40;
            this.btnTestSachHetHang.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestSachHetHang.Size = new System.Drawing.Size(180, 30);
            this.btnTestSachHetHang.Text = "Test: Sách hết hàng";
            this.btnTestSachHetHang.Click += btnTestSachHetHang_Click;

            buttonY += 40;
            this.btnTestSachCoHang.Location = new System.Drawing.Point(10, buttonY);
            this.btnTestSachCoHang.Size = new System.Drawing.Size(180, 30);
            this.btnTestSachCoHang.Text = "Test: Có hàng trở lại";
            this.btnTestSachCoHang.Click += btnTestSachCoHang_Click;

            // grpLogs
            this.grpLogs.Location = new System.Drawing.Point(230, 50);
            this.grpLogs.Size = new System.Drawing.Size(280, 300);
            this.grpLogs.Text = "Event Logs";
            this.lstEvents.Location = new System.Drawing.Point(10, 25);
            this.lstEvents.Size = new System.Drawing.Size(260, 265);
            this.grpLogs.Controls.Add(this.lstEvents);

            // grpWarnings
            this.grpWarnings.Location = new System.Drawing.Point(530, 50);
            this.grpWarnings.Size = new System.Drawing.Size(280, 300);
            this.grpWarnings.Text = "Cảnh báo tồn kho";
            this.lstWarnings.Location = new System.Drawing.Point(10, 25);
            this.lstWarnings.Size = new System.Drawing.Size(260, 265);
            this.grpWarnings.Controls.Add(this.lstWarnings);

            // Status bar
            this.lblStatus.Location = new System.Drawing.Point(12, 370);
            this.lblStatus.Size = new System.Drawing.Size(500, 25);
            this.lblStatus.Text = "Sẵn sàng...";
            this.lblStatus.BorderStyle = BorderStyle.Fixed3D;

            // Control buttons
            this.btnClearLogs.Location = new System.Drawing.Point(530, 370);
            this.btnClearLogs.Size = new System.Drawing.Size(100, 30);
            this.btnClearLogs.Text = "Xóa Logs";
            this.btnClearLogs.Click += btnClearLogs_Click;

            this.btnShowObserverCount.Location = new System.Drawing.Point(640, 370);
            this.btnShowObserverCount.Size = new System.Drawing.Size(120, 30);
            this.btnShowObserverCount.Text = "Số Observer";
            this.btnShowObserverCount.Click += btnShowObserverCount_Click;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 420);
            this.Controls.AddRange(new Control[] {
                this.lblTitle, this.grpEvents, this.grpLogs, this.grpWarnings,
                this.lblStatus, this.btnClearLogs, this.btnShowObserverCount
            });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Text = "Observer Pattern Demo";
            this.FormClosed += frmObserverDemo_FormClosed;

            this.ResumeLayout(false);
        }

        private Label lblTitle;
        private GroupBox grpEvents;
        private Button btnTestSachDaBan;
        private Button btnTestSachDaNhap;
        private Button btnTestHoaDonDaTao;
        private Button btnTestSachSapHet;
        private Button btnTestSachHetHang;
        private Button btnTestSachCoHang;
        private GroupBox grpLogs;
        private ListBox lstEvents;
        private GroupBox grpWarnings;
        private ListBox lstWarnings;
        private Label lblStatus;
        private Button btnClearLogs;
        private Button btnShowObserverCount;
        #endregion
    }
}
