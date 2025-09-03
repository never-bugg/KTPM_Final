using System;

namespace KTPM_Final.Observer.Events
{
    /// <summary>
    /// Enum định nghĩa các loại sự kiện trong hệ thống
    /// </summary>
    public enum EventType
    {
        SachDaBan,           // Khi bán sách
        SachDaNhap,          // Khi nhập sách
        HoaDonDaTao,         // Khi tạo hóa đơn
        SachSapHetHang,      // Khi sách sắp hết hàng (< 10)
        SachHetHang,         // Khi sách hết hàng (= 0)
        SachCoHangTroyLai    // Khi sách có hàng trở lại
    }

    /// <summary>
    /// Lớp chứa dữ liệu sự kiện
    /// </summary>
    public class EventData
    {
        public EventType EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public EventData(EventType eventType, object data, string message = "")
        {
            EventType = eventType;
            Data = data;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }

    /// <summary>
    /// Dữ liệu sự kiện cho việc bán sách
    /// </summary>
    public class SachBanEventData
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongBan { get; set; }
        public int SoLuongConLai { get; set; }
        public decimal GiaBan { get; set; }
        public int MaHoaDon { get; set; }
    }

    /// <summary>
    /// Dữ liệu sự kiện cho việc nhập sách
    /// </summary>
    public class SachNhapEventData
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongNhap { get; set; }
        public int SoLuongSauNhap { get; set; }
        public decimal GiaNhap { get; set; }
        public int MaPhieuNhap { get; set; }
    }

    /// <summary>
    /// Dữ liệu sự kiện cho việc tạo hóa đơn
    /// </summary>
    public class HoaDonEventData
    {
        public int MaHoaDon { get; set; }
        public decimal TongTien { get; set; }
        public DateTime ThoiGianTao { get; set; }
        public string TenNhanVien { get; set; }
        public int SoLuongSanPham { get; set; }
    }

    /// <summary>
    /// Dữ liệu sự kiện cho tồn kho
    /// </summary>
    public class TonKhoEventData
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongHienTai { get; set; }
        public int NguyenThan { get; set; } // Nguyên nhân: 10 = sắp hết, 0 = hết hàng
    }
}
