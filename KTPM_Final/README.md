# HỆ THỐNG QUẢN LÝ NHÀ SÁCH VỚI OBSERVER PATTERN

## Tổng quan dự án

Dự án này là hệ thống quản lý nhà sách được phát triển bằng C# .NET Framework 4.8 với Windows Forms, đã được tích hợp **Observer Pattern** để xử lý các sự kiện thời gian thực.

## Cấu trúc dự án

```
KTPM_Final/
├── Controllers/           # Business Logic Layer
├── Model/                # Data Models
├── View/                 # Presentation Layer (Windows Forms)
├── Observer/             # Observer Pattern Implementation
│   ├── IObserver.cs
│   ├── ISubject.cs
│   ├── BaseSubject.cs
│   ├── ObserverManager.cs
│   ├── Events/
│   │   └── EventData.cs
│   └── Observers/
│       ├── NotificationObserver.cs
│       ├── LogObserver.cs
│       ├── StatisticsObserver.cs
│       └── UIUpdateObserver.cs
└── Database/             # SQL Scripts
```

## Observer Pattern được triển khai

### Các sự kiện được quan sát:

1. **SachDaBan** - Khi bán sách
2. **SachDaNhap** - Khi nhập sách
3. **HoaDonDaTao** - Khi tạo hóa đơn
4. **SachSapHetHang** - Khi số lượng < 10
5. **SachHetHang** - Khi hết hàng
6. **SachCoHangTroyLai** - Khi có hàng trở lại

### Các Observer được triển khai:

1. **NotificationObserver** - Hiển thị thông báo popup
2. **LogObserver** - Ghi log vào file
3. **StatisticsObserver** - Cập nhật thống kê database
4. **UIUpdateObserver** - Cập nhật giao diện real-time

## Tính năng chính

### ✅ Quản lý sách
- Thêm, sửa, xóa sách
- Theo dõi tồn kho
- Cảnh báo hết hàng tự động

### ✅ Quản lý bán hàng
- Tạo hóa đơn
- Chi tiết hóa đơn
- Thống kê doanh thu

### ✅ Quản lý nhập hàng
- Tạo phiếu nhập
- Chi tiết phiếu nhập
- Cập nhật tồn kho tự động

### ✅ Quản lý nhân viên
- Thông tin nhân viên
- Phân quyền theo vai trò
- Quản lý tài khoản

### ✅ Observer Pattern Features
- Thông báo thời gian thực
- Ghi log tự động
- Cập nhật thống kê
- Cảnh báo tồn kho thông minh

## Cách chạy dự án

### Yêu cầu hệ thống:
- .NET Framework 4.8
- Visual Studio 2019 hoặc cao hơn
- SQL Server 2019 hoặc SQL Server Express
- Windows 10/11

### Bước 1: Thiết lập Database
1. Mở SQL Server Management Studio
2. Chạy script `NhasachDB.sql` để tạo database
3. Cập nhật connection string trong các Controller

### Bước 2: Build Project
1. Mở `KTPM_Final.sln` trong Visual Studio
2. Restore NuGet packages (BouncyCastle, iTextSharp)
3. Build solution (Ctrl + Shift + B)

### Bước 3: Chạy ứng dụng
1. Set `KTPM_Final` làm startup project
2. Nhấn F5 để chạy
3. Đăng nhập với tài khoản admin

## Demo Observer Pattern

### Chạy Demo:
```csharp
// Trong Program.cs hoặc tạo form riêng
var demoForm = new frmObserverDemo();
demoForm.ShowDialog();
```

### Test Observer Pattern:
```csharp
// Chạy unit tests
ObserverPatternTester.RunTests();
```

## Hướng dẫn sử dụng Observer Pattern

### 1. Sử dụng ObserverManager
```csharp
// Phát sự kiện bán sách
ObserverManager.Instance.NotifySachDaBan(
    maSach: "1",
    tenSach: "Harry Potter",
    soLuongBan: 3,
    soLuongConLai: 7,
    giaBan: 150000,
    maHoaDon: 123
);
```

### 2. Tạo Observer tùy chỉnh
```csharp
public class CustomObserver : IObserver
{
    public void Update(object eventData)
    {
        if (eventData is EventData data)
        {
            // Xử lý sự kiện tùy chỉnh
            ProcessEvent(data);
        }
    }
}

// Đăng ký observer
var customObserver = new CustomObserver();
ObserverManager.Instance.AddCustomObserver(customObserver);
```

### 3. Tích hợp vào Form
```csharp
public partial class YourForm : Form
{
    private UIUpdateObserver _uiObserver;
    
    private void Form_Load(object sender, EventArgs e)
    {
        _uiObserver = new UIUpdateObserver(
            updateStatusBar: (msg) => statusLabel.Text = msg,
            updateInventoryWarning: ShowWarning
        );
        
        ObserverManager.Instance.AddCustomObserver(_uiObserver);
    }
    
    private void Form_FormClosed(object sender, FormClosedEventArgs e)
    {
        ObserverManager.Instance.RemoveCustomObserver(_uiObserver);
    }
}
```

## Lợi ích của Observer Pattern

### 🎯 **Loose Coupling**
- Tách biệt logic business khỏi xử lý sự kiện
- Controllers không cần biết về UI updates

### 🔄 **Real-time Updates**
- Thông báo ngay lập tức khi có sự kiện
- Cập nhật UI tự động

### 📈 **Scalability**
- Dễ dàng thêm Observer mới
- Không cần sửa đổi code cũ

### 🔍 **Monitoring & Logging**
- Ghi log tự động tất cả hoạt động
- Theo dõi hiệu suất hệ thống

### ⚡ **Performance**
- Xử lý bất đồng bộ
- Không block main thread

## Troubleshooting

### Lỗi thường gặp:

1. **Connection string sai**
   - Kiểm tra `connectionString` trong Controllers
   - Đảm bảo SQL Server đang chạy

2. **NuGet packages thiếu**
   ```bash
   Install-Package BouncyCastle.Cryptography
   Install-Package iTextSharp
   ```

3. **Observer không nhận sự kiện**
   - Kiểm tra observer đã được đăng ký chưa
   - Đảm bảo không bị unregister do lỗi

4. **UI không cập nhật**
   - Sử dụng `Invoke` cho thread-safe updates
   - Kiểm tra `UIUpdateObserver` callbacks

## Tác giả & Đóng góp

- **Dự án gốc**: Hệ thống quản lý nhà sách
- **Observer Pattern**: Được tích hợp để cải thiện architecture
- **Ngôn ngữ**: C# .NET Framework 4.8
- **Database**: SQL Server

## License

Dự án này được phát triển cho mục đích học tập và nghiên cứu.

---

📧 **Liên hệ**: Nếu có thắc mắc về Observer Pattern implementation, vui lòng tham khảo file `HUONG_DAN_OBSERVER_PATTERN.md` để biết chi tiết.
