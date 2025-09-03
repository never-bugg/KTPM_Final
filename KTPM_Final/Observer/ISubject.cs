using System;
using System.Collections.Generic;

namespace KTPM_Final.Observer
{
    /// <summary>
    /// Interface cho các subject (đối tượng được quan sát)
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Đăng ký một observer
        /// </summary>
        /// <param name="observer">Observer cần đăng ký</param>
        void Attach(IObserver observer);

        /// <summary>
        /// Hủy đăng ký một observer
        /// </summary>
        /// <param name="observer">Observer cần hủy đăng ký</param>
        void Detach(IObserver observer);

        /// <summary>
        /// Thông báo cho tất cả observer về sự kiện
        /// </summary>
        /// <param name="eventData">Dữ liệu sự kiện</param>
        void NotifyObservers(object eventData);
    }
}
