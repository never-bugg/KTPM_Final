using System;
using System.Collections.Generic;
using System.Linq;

namespace KTPM_Final.Observer
{
    /// <summary>
    /// Lớp cơ sở triển khai ISubject
    /// </summary>
    public abstract class BaseSubject : ISubject
    {
        private readonly List<IObserver> _observers;

        protected BaseSubject()
        {
            _observers = new List<IObserver>();
        }

        /// <summary>
        /// Đăng ký một observer
        /// </summary>
        public virtual void Attach(IObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        /// <summary>
        /// Hủy đăng ký một observer
        /// </summary>
        public virtual void Detach(IObserver observer)
        {
            if (observer != null && _observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        /// <summary>
        /// Thông báo cho tất cả observer
        /// </summary>
        public virtual void NotifyObservers(object eventData)
        {
            // Tạo bản copy để tránh lỗi khi có thay đổi danh sách observer trong quá trình notify
            var observersCopy = _observers.ToList();
            
            foreach (var observer in observersCopy)
            {
                try
                {
                    observer.Update(eventData);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không dừng việc thông báo cho các observer khác
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi thông báo cho observer: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Lấy số lượng observer hiện tại
        /// </summary>
        public int ObserverCount => _observers.Count;

        /// <summary>
        /// Xóa tất cả observer
        /// </summary>
        public virtual void ClearObservers()
        {
            _observers.Clear();
        }
    }
}
