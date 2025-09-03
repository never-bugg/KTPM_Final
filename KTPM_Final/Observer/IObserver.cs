using System;

namespace KTPM_Final.Observer
{
    /// <summary>
    /// Interface cho các observer trong hệ thống
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Phương thức được gọi khi có sự kiện xảy ra
        /// </summary>
        /// <param name="eventData">Dữ liệu của sự kiện</param>
        void Update(object eventData);
    }
}
