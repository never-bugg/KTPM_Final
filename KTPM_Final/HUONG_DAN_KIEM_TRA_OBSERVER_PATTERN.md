# HƯỚNG DẪN KIỂM TRA OBSERVER PATTERN KHI CHẠY CHƯƠNG TRÌNH

## Mục đích

Tài liệu này hướng dẫn cách kiểm tra và xác minh Observer Pattern đã được tích hợp thành công vào hệ thống quản lý nhà sách. Bạn sẽ học cách:

1. Kiểm tra các sự kiện Observer được phát ra đúng cách
2. Xác minh các Observer nhận và xử lý sự kiện
3. Theo dõi log và thông báo real-time
4. Test các tình huống thực tế

---

## Chuẩn bị trước khi kiểm tra

### 1. Đảm bảo hệ thống hoạt động
- ✅ Database đã được tạo và có dữ liệu mẫu
- ✅ Connection string đã được cấu hình đúng
- ✅ Ứng dụng build thành công không có lỗi
- ✅ Có thể đăng nhập vào hệ thống

### 2. Tạo dữ liệu test
```sql
-- Thêm một số sách mẫu để test
INSERT INTO Sach (TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh)
VALUES 
('Test Book 1', 'Test Author', 'Test Category', 15, 50000, 80000, 0),
('Test Book 2', 'Test Author', 'Test Category', 8, 60000, 90000, 0),
('Test Book 3', 'Test Author', 'Test Category', 0, 70000, 100000, 0);
```

### 3. Bật chế độ Debug
- Mở Visual Studio
- Set breakpoint tại `ObserverManager.Instance.NotifyObservers()`
- Chạy ứng dụng trong Debug mode (F5)

---

## PHẦN 1: KIỂM TRA SỰ KIỆN BÁN SÁCH

### Bước 1: Mở form Hóa đơn
1. Đăng nhập vào hệ thống
2. Mở form **Hóa đơn** (frmHoaDon)
3. Quan sát các thành phần UI sẵn sàng

### Bước 2: Thực hiện bán sách
1. **Chọn sách từ ComboBox**
   - Chọn một cuốn sách có số lượng > 10
   - Quan sát: Giá bán và số lượng hiện tại được hiển thị

2. **Nhập số lượng bán**
   - Nhập số lượng: 3
   - Nhấn **"Thêm vào giỏ"**

3. **Tạo hóa đơn**
   - Nhấn **"Tạo hóa đơn"**
   - **Quan sát các sự kiện Observer:**

### Kết quả mong đợi:
```
[Observer Events được phát]
✅ SachDaBan Event:
   - MaSach: [ID sách]
   - TenSach: [Tên sách]
   - SoLuongBan: 3
   - SoLuongConLai: [Số lượng cũ - 3]
   - GiaBan: [Giá bán]
   - MaHoaDon: [ID hóa đơn mới]

✅ HoaDonDaTao Event:
   - MaHoaDon: [ID hóa đơn]
   - TongTien: [Tổng tiền]
   - TenNhanVien: [Tên nhân viên]
   - SoLuongSanPham: 1
```

### Xác minh:
- [ ] Thông báo popup xuất hiện: "Tạo hóa đơn thành công!"
- [ ] File log được tạo trong thư mục `logs/`
- [ ] Debug Console hiển thị log messages
- [ ] Số lượng sách trong database đã giảm

---

## PHẦN 2: KIỂM TRA CẢNH BÁO TỒN KHO

### Scenario 1: Sách sắp hết hàng (< 10 cuốn)

1. **Chọn sách có số lượng = 12**
2. **Bán 8 cuốn** (còn lại 4 cuốn)
3. **Tạo hóa đơn**

### Kết quả mong đợi:
```
⚠️ CẢNH BÁO: Observer Events
✅ SachDaBan Event (bình thường)
✅ SachSapHetHang Event:
   - MaSach: [ID sách]
   - TenSach: [Tên sách]
   - SoLuongHienTai: 4
   - NguyenThan: 10
```

### Xác minh:
- [ ] Popup cảnh báo: "CẢNH BÁO: Sách '[Tên sách]' sắp hết hàng! Còn 4 cuốn"
- [ ] Log file ghi nhận sự kiện SachSapHetHang
- [ ] UI cập nhật với màu cảnh báo (nếu có UIUpdateObserver)

### Scenario 2: Sách hết hàng (= 0 cuốn)

1. **Chọn sách có số lượng = 5**
2. **Bán hết 5 cuốn**
3. **Tạo hóa đơn**

### Kết quả mong đợi:
```
🚨 CẢNH BÁO NGHIÊM TRỌNG: Observer Events
✅ SachDaBan Event (bình thường)
✅ SachHetHang Event:
   - MaSach: [ID sách]
   - TenSach: [Tên sách]
   - SoLuongHienTai: 0
   - NguyenThan: 0
```

### Xác minh:
- [ ] Popup cảnh báo nghiêm trọng: "CẢNH BÁO: Sách '[Tên sách]' đã hết hàng!"
- [ ] Log file ghi nhận sự kiện SachHetHang
- [ ] MessageBox hiển thị với icon Warning

---

## PHẦN 3: KIỂM TRA SỰ KIỆN NHẬP SÁCH

### Bước 1: Mở form Nhập sách
1. Mở form **Nhập sách** (frmNhapSach)
2. Tạo phiếu nhập mới

### Bước 2: Thực hiện nhập sách
1. **Chọn sách đã hết hàng** (từ test trước)
2. **Nhập số lượng: 20**
3. **Thêm vào phiếu nhập**
4. **Lưu phiếu nhập**

### Kết quả mong đợi:
```
📦 NHẬP HÀNG: Observer Events
✅ SachDaNhap Event:
   - MaSach: [ID sách]
   - TenSach: [Tên sách]
   - SoLuongNhap: 20
   - SoLuongSauNhap: 20
   - GiaNhap: [Giá nhập]
   - MaPhieuNhap: [ID phiếu nhập]

✅ SachCoHangTroyLai Event:
   - MaSach: [ID sách]
   - TenSach: [Tên sách]
   - SoLuongHienTai: 20
```

### Xác minh:
- [ ] Log ghi nhận sự kiện SachDaNhap
- [ ] Log ghi nhận sự kiện SachCoHangTroyLai
- [ ] Số lượng trong database đã được cập nhật
- [ ] Thông báo: "Sách '[Tên sách]' đã có hàng trở lại! Số lượng: 20"

---

## PHẦN 4: KIỂM TRA LOG VÀ THỐNG KÊ

### Kiểm tra Log Files

1. **Mở thư mục `logs/` trong project**
2. **Kiểm tra file `system_events.log`**

### Format log mong đợi:
```
[2025-08-31 14:30:15] [SachDaBan] MaSach: 1, TenSach: Test Book 1, SoLuongBan: 3, ConLai: 7, GiaBan: 80000, MaHoaDon: 5
[2025-08-31 14:30:15] [SachSapHetHang] MaSach: 1, TenSach: Test Book 1, SoLuong: 7
[2025-08-31 14:30:15] [HoaDonDaTao] MaHoaDon: 5, TongTien: 240000, NhanVien: Nguyễn Văn A, SoSP: 1
[2025-08-31 14:32:10] [SachDaNhap] MaSach: 3, TenSach: Test Book 3, SoLuongNhap: 20, TongSau: 20, GiaNhap: 70000, MaPhieuNhap: 2
[2025-08-31 14:32:10] [SachCoHangTroyLai] MaSach: 3, TenSach: Test Book 3, SoLuong: 20
```

### Xác minh:
- [ ] File log tồn tại và có thể đọc được
- [ ] Timestamp chính xác
- [ ] Dữ liệu sự kiện đầy đủ và đúng format
- [ ] Không có lỗi encoding (tiếng Việt hiển thị đúng)

---

## PHẦN 5: KIỂM TRA OBSERVER PATTERN VỚI DEMO FORM

### Chạy Demo Form (Nếu có)

1. **Tạo và chạy frmObserverDemo**
```csharp
var demoForm = new frmObserverDemo();
demoForm.ShowDialog();
```

2. **Test từng loại sự kiện:**

#### Test 1: Sự kiện bán sách
- Nhấn **"Test: Bán sách"**
- Quan sát:
  - [ ] Event log hiển thị ngay lập tức
  - [ ] Status bar cập nhật
  - [ ] Timestamp chính xác

#### Test 2: Sự kiện nhập sách
- Nhấn **"Test: Nhập sách"**
- Quan sát:
  - [ ] Event log hiển thị thông tin nhập hàng
  - [ ] Status bar cập nhật

#### Test 3: Sự kiện tạo hóa đơn
- Nhấn **"Test: Tạo hóa đơn"**
- Quan sát:
  - [ ] Event log hiển thị thông tin hóa đơn
  - [ ] Status bar hiển thị tổng tiền

#### Test 4: Cảnh báo tồn kho
- Nhấn **"Test: Sách sắp hết"**
- Nhấn **"Test: Sách hết hàng"**
- Quan sát:
  - [ ] Cảnh báo hiển thị trong phần riêng
  - [ ] Màu sắc thay đổi (vàng/đỏ)
  - [ ] Icon cảnh báo hiển thị

#### Test 5: Có hàng trở lại
- Nhấn **"Test: Có hàng trở lại"**
- Quan sát:
  - [ ] Thông báo tích cực
  - [ ] Màu sắc quay về bình thường

---

## PHẦN 6: KIỂM TRA PERFORMANCE VÀ MEMORY

### Test Performance

1. **Chạy nhiều sự kiện liên tiếp:**
```csharp
// Test 100 sự kiện
for (int i = 0; i < 100; i++)
{
    ObserverManager.Instance.NotifySachDaBan($"{i}", $"Book {i}", 1, 10, 100000, i);
}
```

### Xác minh:
- [ ] Ứng dụng không bị lag
- [ ] Memory usage không tăng bất thường
- [ ] Tất cả events được xử lý
- [ ] Không có memory leak

### Test Observer Count

1. **Kiểm tra số lượng Observer:**
```csharp
int count = ObserverManager.Instance.ObserverCount;
Console.WriteLine($"Số Observer: {count}");
```

### Xác minh:
- [ ] Số Observer mặc định = 3 (Notification + Log + Statistics)
- [ ] Số Observer tăng khi thêm custom observer
- [ ] Số Observer giảm khi remove observer

---

## PHẦN 7: KIỂM TRA ERROR HANDLING

### Test Exception Handling

1. **Tạo Observer lỗi:**
```csharp
public class BuggyObserver : IObserver
{
    public void Update(object eventData)
    {
        throw new Exception("Test exception");
    }
}

// Thêm observer lỗi
ObserverManager.Instance.AddCustomObserver(new BuggyObserver());
```

2. **Phát sự kiện:**
```csharp
ObserverManager.Instance.NotifySachDaBan("1", "Test", 1, 9, 100000, 1);
```

### Xác minh:
- [ ] Ứng dụng không crash
- [ ] Các observer khác vẫn hoạt động bình thường
- [ ] Lỗi được log trong Debug Console
- [ ] Thông báo lỗi không hiển thị cho user

---

## PHẦN 8: CHECKLIST TỔNG HỢP

### ✅ Functional Tests
- [ ] Sự kiện bán sách hoạt động
- [ ] Sự kiện nhập sách hoạt động  
- [ ] Sự kiện tạo hóa đơn hoạt động
- [ ] Cảnh báo tồn kho hoạt động
- [ ] Thông báo có hàng trở lại hoạt động

### ✅ Observer Tests
- [ ] NotificationObserver hiển thị popup đúng
- [ ] LogObserver ghi file log chính xác
- [ ] StatisticsObserver cập nhật database (nếu có)
- [ ] UIUpdateObserver cập nhật giao diện

### ✅ Integration Tests
- [ ] Observer Pattern tích hợp với Controllers
- [ ] Sự kiện phát từ View layer
- [ ] Database được cập nhật đồng bộ
- [ ] UI phản hồi real-time

### ✅ Error & Performance Tests
- [ ] Exception handling hoạt động
- [ ] Memory usage ổn định
- [ ] Performance chấp nhận được
- [ ] Thread safety đảm bảo

### ✅ Configuration Tests
- [ ] Observer có thể được thêm/xóa
- [ ] Configuration tùy chỉnh hoạt động
- [ ] Singleton pattern đúng
- [ ] Lifecycle management đúng

---

## TROUBLESHOOTING - KHẮC PHỤC SỰ CỐ

### Sự kiện không được phát
**Nguyên nhân:** Observer chưa được đăng ký
**Giải pháp:**
```csharp
// Kiểm tra Observer đã được đăng ký
int count = ObserverManager.Instance.ObserverCount;
if (count == 0)
{
    // Đăng ký lại observers mặc định
    ObserverManager.Instance.InitializeObservers();
}
```

### Popup không hiển thị
**Nguyên nhân:** NotificationObserver bị tắt popup
**Giải pháp:**
```csharp
// Tạo observer với popup enabled
var notificationObserver = new NotificationObserver(showPopup: true);
ObserverManager.Instance.AddCustomObserver(notificationObserver);
```

### Log file không được tạo
**Nguyên nhân:** Không có quyền ghi file hoặc đường dẫn sai
**Giải pháp:**
```csharp
// Kiểm tra và tạo thư mục logs
string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
if (!Directory.Exists(logDir))
{
    Directory.CreateDirectory(logDir);
}
```

### Observer bị memory leak
**Nguyên nhân:** Không hủy đăng ký observer khi đóng form
**Giải pháp:**
```csharp
private void Form_FormClosed(object sender, FormClosedEventArgs e)
{
    // Luôn hủy đăng ký observer
    ObserverManager.Instance.RemoveCustomObserver(_customObserver);
}
```

---

## KẾT LUẬN

Sau khi hoàn thành tất cả các bước kiểm tra trên, bạn sẽ có thể:

1. **Xác nhận Observer Pattern hoạt động đúng** trong hệ thống
2. **Hiểu cách các sự kiện được phát và xử lý** 
3. **Biết cách debug và troubleshoot** các vấn đề Observer
4. **Tự tin rằng pattern được tích hợp thành công** và sẵn sàng sử dụng

### Tiêu chí thành công:
- ✅ **100% functional tests PASS**
- ✅ **Không có lỗi runtime**
- ✅ **Performance ổn định**
- ✅ **Log files đầy đủ và chính xác**
- ✅ **UI responsive và user-friendly**

**🎉 Chúc mừng! Observer Pattern đã được tích hợp thành công vào hệ thống quản lý nhà sách!**
