# HƯỚNG DẪN NHÚNG OBSERVER PATTERN VÀO HỆ THỐNG QUẢN LÝ NHÀ SÁCH

## Tổng quan Observer Pattern

Observer Pattern là một design pattern thuộc nhóm Behavioral Patterns. Pattern này định nghĩa mối quan hệ phụ thuộc one-to-many giữa các đối tượng, khi một đối tượng thay đổi trạng thái, tất cả các đối tượng phụ thuộc sẽ được thông báo và tự động cập nhật.

### Lợi ích của Observer Pattern:
1. **Loose Coupling**: Giảm sự phụ thuộc chặt chẽ giữa các thành phần
2. **Tính mở rộng**: Dễ dàng thêm observer mới mà không cần sửa đổi code cũ
3. **Real-time Updates**: Cập nhật thời gian thực khi có sự kiện xảy ra
4. **Separation of Concerns**: Tách biệt logic xử lý sự kiện khỏi business logic

## Cấu trúc Observer Pattern trong dự án

### 1. Interfaces chính
- **IObserver**: Interface cho các observer
- **ISubject**: Interface cho các subject (đối tượng được quan sát)

### 2. Classes triển khai
- **BaseSubject**: Lớp cơ sở triển khai ISubject
- **ObserverManager**: Singleton quản lý toàn bộ hệ thống Observer

### 3. Event Data
- **EventData**: Lớp chứa thông tin sự kiện
- **SachBanEventData**: Dữ liệu sự kiện bán sách
- **SachNhapEventData**: Dữ liệu sự kiện nhập sách
- **HoaDonEventData**: Dữ liệu sự kiện tạo hóa đơn
- **TonKhoEventData**: Dữ liệu sự kiện tồn kho

### 4. Concrete Observers
- **NotificationObserver**: Hiển thị thông báo popup
- **LogObserver**: Ghi log vào file
- **StatisticsObserver**: Cập nhật thống kê database
- **UIUpdateObserver**: Cập nhật giao diện người dùng

## Các sự kiện được quan sát

### 1. Sự kiện bán sách (SachDaBan)
**Khi nào phát sinh**: Khi thêm chi tiết hóa đơn
**Dữ liệu**: Mã sách, tên sách, số lượng bán, số lượng còn lại, giá bán, mã hóa đơn
**Observer xử lý**: Tất cả observers

### 2. Sự kiện nhập sách (SachDaNhap)
**Khi nào phát sinh**: Khi thêm chi tiết phiếu nhập
**Dữ liệu**: Mã sách, tên sách, số lượng nhập, số lượng sau nhập, giá nhập, mã phiếu nhập
**Observer xử lý**: Tất cả observers

### 3. Sự kiện tạo hóa đơn (HoaDonDaTao)
**Khi nào phát sinh**: Khi tạo hóa đơn mới
**Dữ liệu**: Mã hóa đơn, tổng tiền, thời gian tạo, tên nhân viên, số lượng sản phẩm
**Observer xử lý**: Tất cả observers

### 4. Sự kiện cảnh báo tồn kho
- **SachSapHetHang**: Khi số lượng < 10
- **SachHetHang**: Khi số lượng = 0
- **SachCoHangTroLai**: Khi từ hết hàng chuyển thành có hàng

## Cách tích hợp vào Controllers

### SachController
```csharp
// Thêm using
using KTPM_Final.Observer;

// Trong method CapNhatSoLuong
private void CheckAndNotifyInventoryChange(string maSach, string tenSach, int soLuongCu, int soLuongMoi)
{
    var observerManager = ObserverManager.Instance;
    
    if (soLuongCu == 0 && soLuongMoi > 0)
    {
        observerManager.NotifySachCoHangTroLai(maSach, tenSach, soLuongMoi);
    }
    else if (soLuongCu > 0 && soLuongMoi == 0)
    {
        observerManager.NotifySachHetHang(maSach, tenSach);
    }
    else if (soLuongMoi < soLuongCu && soLuongMoi > 0 && soLuongMoi < 10)
    {
        observerManager.NotifySachSapHetHang(maSach, tenSach, soLuongMoi);
    }
}
```

### HoaDonController
```csharp
// Trong method ThemHoaDon
if (result)
{
    ObserverManager.Instance.NotifyHoaDonDaTao(
        hoaDon.MaHoaDon,
        hoaDon.TongTien,
        hoaDon.ThoiGianTao,
        hoaDon.TenNhanVien,
        soLuongSanPham
    );
}
```

### ChiTietHoaDonController
```csharp
// Trong method ThemChiTietHoaDon
if (result)
{
    ObserverManager.Instance.NotifySachDaBan(
        chiTiet.MaSach,
        sach.TenSach,
        chiTiet.SoLuong,
        sach.SoLuong,
        chiTiet.GiaBan,
        chiTiet.MaHoaDon
    );
}
```

## Cách sử dụng trong Form

### Thêm Observer tùy chỉnh cho Form
```csharp
public partial class frmHoaDon : Form
{
    private UIUpdateObserver _uiObserver;
    
    private void frmHoaDon_Load(object sender, EventArgs e)
    {
        // Tạo UI Observer với callback functions
        _uiObserver = new UIUpdateObserver(
            updateStatusBar: (message) => {
                statusLabel.Text = message;
            },
            updateInventoryWarning: (tenSach, warning) => {
                // Hiển thị cảnh báo trên UI
                ShowInventoryWarning(tenSach, warning);
            }
        );
        
        // Đăng ký observer
        ObserverManager.Instance.AddCustomObserver(_uiObserver);
    }
    
    private void frmHoaDon_FormClosed(object sender, FormClosedEventArgs e)
    {
        // Hủy đăng ký observer khi đóng form
        ObserverManager.Instance.RemoveCustomObserver(_uiObserver);
    }
}
```

## Tạo Observer tùy chỉnh

### Ví dụ: Email Notification Observer
```csharp
public class EmailNotificationObserver : IObserver
{
    public void Update(object eventData)
    {
        if (eventData is EventData data)
        {
            switch (data.EventType)
            {
                case EventType.SachHetHang:
                    if (data.Data is TonKhoEventData tonKho)
                    {
                        SendLowStockEmail(tonKho.TenSach);
                    }
                    break;
                    
                case EventType.HoaDonDaTao:
                    if (data.Data is HoaDonEventData hoaDon)
                    {
                        SendSalesReportEmail(hoaDon);
                    }
                    break;
            }
        }
    }
    
    private void SendLowStockEmail(string tenSach)
    {
        // Logic gửi email cảnh báo hết hàng
    }
    
    private void SendSalesReportEmail(HoaDonEventData hoaDon)
    {
        // Logic gửi email báo cáo bán hàng
    }
}

// Đăng ký observer
var emailObserver = new EmailNotificationObserver();
ObserverManager.Instance.AddCustomObserver(emailObserver);
```

## Testing Observer Pattern

### Test cơ bản
```csharp
[Test]
public void TestObserverPattern()
{
    // Arrange
    var testObserver = new TestObserver();
    ObserverManager.Instance.AddCustomObserver(testObserver);
    
    // Act
    ObserverManager.Instance.NotifySachDaBan("1", "Test Book", 5, 15, 100000, 1);
    
    // Assert
    Assert.IsTrue(testObserver.EventReceived);
    Assert.AreEqual(EventType.SachDaBan, testObserver.LastEventType);
}

public class TestObserver : IObserver
{
    public bool EventReceived { get; private set; }
    public EventType LastEventType { get; private set; }
    
    public void Update(object eventData)
    {
        if (eventData is EventData data)
        {
            EventReceived = true;
            LastEventType = data.EventType;
        }
    }
}
```

## Cấu hình và Tùy chỉnh

### Tắt/Bật Observer
```csharp
// Tắt notification popup
var notificationObserver = new NotificationObserver(showPopup: false);

// Tùy chỉnh đường dẫn log file
var logObserver = new LogObserver("logs/custom_events.log");

// Thay connection string cho StatisticsObserver
var statsObserver = new StatisticsObserver("your_connection_string");
```

### Xóa tất cả Observer
```csharp
ObserverManager.Instance.ClearObservers();
```

## Best Practices

### 1. Error Handling
- Mỗi observer nên có try-catch riêng
- Lỗi ở một observer không được ảnh hưởng đến observer khác
- Log lỗi để debug

### 2. Performance
- Không thực hiện các thao tác nặng trong observer
- Sử dụng async/await cho các thao tác I/O
- Cân nhắc sử dụng background thread cho các observer phức tạp

### 3. Memory Management
- Luôn hủy đăng ký observer khi không cần thiết
- Tránh memory leak bằng cách sử dụng weak reference nếu cần

### 4. Testing
- Tạo test observer để kiểm tra logic
- Mock các dependencies trong observer
- Test các edge cases

## Lưu ý khi triển khai

1. **Thread Safety**: ObserverManager được thiết kế thread-safe
2. **Singleton Pattern**: ObserverManager sử dụng Lazy Singleton
3. **Event Data**: Luôn kiểm tra kiểu dữ liệu trước khi cast
4. **Exception Handling**: Observer không được throw exception ra ngoài
5. **Lifecycle Management**: Đăng ký/hủy đăng ký observer đúng thời điểm

## Kết luận

Observer Pattern đã được tích hợp thành công vào hệ thống quản lý nhà sách với các tính năng:

- ✅ Thông báo real-time khi có sự kiện
- ✅ Ghi log tự động các hoạt động
- ✅ Cập nhật thống kê tự động
- ✅ Cảnh báo tồn kho thông minh
- ✅ Tính mở rộng cao
- ✅ Loose coupling giữa các component

Pattern này giúp hệ thống trở nên linh hoạt, dễ bảo trì và mở rộng trong tương lai.
