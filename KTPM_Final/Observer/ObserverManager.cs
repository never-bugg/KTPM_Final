using System;
using KTPM_Final.Observer.Events;
using KTPM_Final.Observer.Observers;

namespace KTPM_Final.Observer
{
    /// <summary>
    /// Lớp Singleton quản lý toàn bộ hệ thống Observer trong ứng dụng
    /// </summary>
    public sealed class ObserverManager : BaseSubject
    {
        private static readonly Lazy<ObserverManager> _instance = 
            new Lazy<ObserverManager>(() => new ObserverManager());

        public static ObserverManager Instance => _instance.Value;

        private ObserverManager()
        {
            InitializeObservers();
        }

        /// <summary>
        /// Khởi tạo các Observer mặc định
        /// </summary>
        private void InitializeObservers()
        {
            // Đăng ký các observer mặc định
            var notificationObserver = new NotificationObserver(true);
            var logObserver = new LogObserver("logs/system_events.log");
            
            // Lấy connection string từ config hoặc controller
            string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";
            var statisticsObserver = new StatisticsObserver(connectionString);

            Attach(notificationObserver);
            Attach(logObserver);
            Attach(statisticsObserver);
        }

        /// <summary>
        /// Phát sự kiện khi bán sách
        /// </summary>
        public void NotifySachDaBan(string maSach, string tenSach, int soLuongBan, 
            int soLuongConLai, decimal giaBan, int maHoaDon)
        {
            // Tính số lượng sách còn lại sau khi bán
            int soLuongSauBan = soLuongConLai - soLuongBan;
            var eventData = new EventData(EventType.SachDaBan, new SachBanEventData
            {
                MaSach = maSach,
                TenSach = tenSach,
                SoLuongBan = soLuongBan,
                SoLuongConLai = soLuongSauBan,
                GiaBan = giaBan,
                MaHoaDon = maHoaDon
            });

            NotifyObservers(eventData);

            // Kiểm tra và phát cảnh báo tồn kho
            CheckInventoryWarning(maSach, tenSach, soLuongConLai);
        }

        /// <summary>
        /// Phát sự kiện khi nhập sách
        /// </summary>
        public void NotifySachDaNhap(string maSach, string tenSach, int soLuongNhap, 
            int soLuongSauNhap, decimal giaNhap, int maPhieuNhap)
        {
            var eventData = new EventData(EventType.SachDaNhap, new SachNhapEventData
            {
                MaSach = maSach,
                TenSach = tenSach,
                SoLuongNhap = soLuongNhap,
                SoLuongSauNhap = soLuongSauNhap,
                GiaNhap = giaNhap,
                MaPhieuNhap = maPhieuNhap
            });

            NotifyObservers(eventData);

            // Kiểm tra có hàng trở lại
            if (soLuongSauNhap > 0 && (soLuongSauNhap - soLuongNhap) == 0)
            {
                NotifySachCoHangTroLai(maSach, tenSach, soLuongSauNhap);
            }
        }

        /// <summary>
        /// Phát sự kiện khi tạo hóa đơn
        /// </summary>
        public void NotifyHoaDonDaTao(int maHoaDon, decimal tongTien, DateTime thoiGianTao, 
            string tenNhanVien, int soLuongSanPham)
        {
            var eventData = new EventData(EventType.HoaDonDaTao, new HoaDonEventData
            {
                MaHoaDon = maHoaDon,
                TongTien = tongTien,
                ThoiGianTao = thoiGianTao,
                TenNhanVien = tenNhanVien,
                SoLuongSanPham = soLuongSanPham
            });

            NotifyObservers(eventData);
        }

        /// <summary>
        /// Phát sự kiện sách sắp hết hàng
        /// </summary>
        public void NotifySachSapHetHang(string maSach, string tenSach, int soLuongHienTai)
        {
            
            var eventData = new EventData(EventType.SachSapHetHang, new TonKhoEventData
            {
                MaSach = maSach,
                TenSach = tenSach,
                SoLuongHienTai = soLuongHienTai,
                NguyenThan = 10
            });

            NotifyObservers(eventData);
        }

        /// <summary>
        /// Phát sự kiện sách hết hàng
        /// </summary>
        public void NotifySachHetHang(string maSach, string tenSach)
        {
            var eventData = new EventData(EventType.SachHetHang, new TonKhoEventData
            {
                MaSach = maSach,
                TenSach = tenSach,
                SoLuongHienTai = 0,
                NguyenThan = 0
            });

            NotifyObservers(eventData);
        }

        /// <summary>
        /// Phát sự kiện sách có hàng trở lại
        /// </summary>
        public void NotifySachCoHangTroLai(string maSach, string tenSach, int soLuongHienTai)
        {
            var eventData = new EventData(EventType.SachCoHangTroyLai, new TonKhoEventData
            {
                MaSach = maSach,
                TenSach = tenSach,
                SoLuongHienTai = soLuongHienTai
            });

            NotifyObservers(eventData);
        }

        /// <summary>
        /// Kiểm tra và phát cảnh báo tồn kho
        /// </summary>
        private void CheckInventoryWarning(string maSach, string tenSach, int soLuongConLai)
        {
            if (soLuongConLai == 0)
            {
                NotifySachHetHang(maSach, tenSach);
            }
            else if (soLuongConLai < 10)
            {
                NotifySachSapHetHang(maSach, tenSach, soLuongConLai);
            }
        }

        /// <summary>
        /// Thêm observer tùy chỉnh
        /// </summary>
        public void AddCustomObserver(IObserver observer)
        {
            Attach(observer);
        }

        /// <summary>
        /// Xóa observer tùy chỉnh
        /// </summary>
        public void RemoveCustomObserver(IObserver observer)
        {
            Detach(observer);
        }
    }
}
