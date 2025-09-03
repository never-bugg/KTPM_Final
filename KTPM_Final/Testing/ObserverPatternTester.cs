using System;
using KTPM_Final.Observer;
using KTPM_Final.Observer.Events;

namespace KTPM_Final.Testing
{
    /// <summary>
    /// Lớp test đơn giản cho Observer Pattern
    /// </summary>
    public class ObserverPatternTester
    {
        public static void RunTests()
        {
            Console.WriteLine("=== KIỂM TRA OBSERVER PATTERN ===\n");
            
            TestBasicObserverFunctionality();
            TestMultipleObservers();
            TestEventTypes();
            TestObserverManagerSingleton();
            
            Console.WriteLine("\n=== HOÀN THÀNH TẤT CẢ TESTS ===");
        }

        private static void TestBasicObserverFunctionality()
        {
            Console.WriteLine("1. Test: Chức năng cơ bản của Observer");
            
            var testObserver = new TestObserver();
            ObserverManager.Instance.AddCustomObserver(testObserver);
            
            // Test phát sự kiện
            ObserverManager.Instance.NotifySachDaBan("1", "Test Book", 5, 15, 100000, 1);
            
            // Kiểm tra kết quả
            if (testObserver.EventReceived && testObserver.LastEventType == EventType.SachDaBan)
            {
                Console.WriteLine("   ✅ PASS: Observer nhận được sự kiện chính xác");
            }
            else
            {
                Console.WriteLine("   ❌ FAIL: Observer không nhận được sự kiện");
            }
            
            ObserverManager.Instance.RemoveCustomObserver(testObserver);
            Console.WriteLine();
        }

        private static void TestMultipleObservers()
        {
            Console.WriteLine("2. Test: Nhiều Observer cùng lúc");
            
            var observer1 = new TestObserver();
            var observer2 = new TestObserver();
            var observer3 = new TestObserver();
            
            ObserverManager.Instance.AddCustomObserver(observer1);
            ObserverManager.Instance.AddCustomObserver(observer2);
            ObserverManager.Instance.AddCustomObserver(observer3);
            
            // Phát sự kiện
            ObserverManager.Instance.NotifyHoaDonDaTao(123, 500000, DateTime.Now, "Test User", 3);
            
            // Kiểm tra tất cả observer đều nhận được
            bool allReceived = observer1.EventReceived && observer2.EventReceived && observer3.EventReceived;
            
            if (allReceived)
            {
                Console.WriteLine("   ✅ PASS: Tất cả observers đều nhận được sự kiện");
            }
            else
            {
                Console.WriteLine("   ❌ FAIL: Một số observers không nhận được sự kiện");
            }
            
            // Cleanup
            ObserverManager.Instance.RemoveCustomObserver(observer1);
            ObserverManager.Instance.RemoveCustomObserver(observer2);
            ObserverManager.Instance.RemoveCustomObserver(observer3);
            Console.WriteLine();
        }

        private static void TestEventTypes()
        {
            Console.WriteLine("3. Test: Các loại sự kiện khác nhau");
            
            var testObserver = new TestObserver();
            ObserverManager.Instance.AddCustomObserver(testObserver);
            
            // Test từng loại sự kiện
            var eventTypes = new EventType[]
            {
                EventType.SachDaBan,
                EventType.SachDaNhap,
                EventType.HoaDonDaTao,
                EventType.SachSapHetHang,
                EventType.SachHetHang,
                EventType.SachCoHangTroyLai
            };

            int passCount = 0;
            
            foreach (var eventType in eventTypes)
            {
                testObserver.Reset();
                
                switch (eventType)
                {
                    case EventType.SachDaBan:
                        ObserverManager.Instance.NotifySachDaBan("1", "Test", 1, 9, 100000, 1);
                        break;
                    case EventType.SachDaNhap:
                        ObserverManager.Instance.NotifySachDaNhap("1", "Test", 10, 19, 80000, 1);
                        break;
                    case EventType.HoaDonDaTao:
                        ObserverManager.Instance.NotifyHoaDonDaTao(1, 100000, DateTime.Now, "Test", 1);
                        break;
                    case EventType.SachSapHetHang:
                        ObserverManager.Instance.NotifySachSapHetHang("1", "Test", 5);
                        break;
                    case EventType.SachHetHang:
                        ObserverManager.Instance.NotifySachHetHang("1", "Test");
                        break;
                    case EventType.SachCoHangTroyLai:
                        ObserverManager.Instance.NotifySachCoHangTroLai("1", "Test", 10);
                        break;
                }
                
                if (testObserver.EventReceived && testObserver.LastEventType == eventType)
                {
                    Console.WriteLine($"   ✅ {eventType}: PASS");
                    passCount++;
                }
                else
                {
                    Console.WriteLine($"   ❌ {eventType}: FAIL");
                }
            }
            
            Console.WriteLine($"   Kết quả: {passCount}/{eventTypes.Length} sự kiện test thành công");
            
            ObserverManager.Instance.RemoveCustomObserver(testObserver);
            Console.WriteLine();
        }

        private static void TestObserverManagerSingleton()
        {
            Console.WriteLine("4. Test: Singleton pattern của ObserverManager");
            
            var instance1 = ObserverManager.Instance;
            var instance2 = ObserverManager.Instance;
            
            if (ReferenceEquals(instance1, instance2))
            {
                Console.WriteLine("   ✅ PASS: ObserverManager là Singleton");
            }
            else
            {
                Console.WriteLine("   ❌ FAIL: ObserverManager không phải Singleton");
            }
            
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Observer dùng để test
    /// </summary>
    public class TestObserver : IObserver
    {
        public bool EventReceived { get; private set; }
        public EventType LastEventType { get; private set; }
        public object LastEventData { get; private set; }
        public int EventCount { get; private set; }

        public void Update(object eventData)
        {
            EventReceived = true;
            EventCount++;
            
            if (eventData is EventData data)
            {
                LastEventType = data.EventType;
                LastEventData = data.Data;
            }
        }

        public void Reset()
        {
            EventReceived = false;
            LastEventType = default;
            LastEventData = null;
            EventCount = 0;
        }
    }

    /// <summary>
    /// Observer để test performance
    /// </summary>
    public class PerformanceTestObserver : IObserver
    {
        public int UpdateCount { get; private set; }
        public DateTime FirstUpdate { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public void Update(object eventData)
        {
            UpdateCount++;
            
            if (UpdateCount == 1)
            {
                FirstUpdate = DateTime.Now;
            }
            
            LastUpdate = DateTime.Now;
        }

        public TimeSpan GetProcessingTime()
        {
            return LastUpdate - FirstUpdate;
        }

        public void Reset()
        {
            UpdateCount = 0;
            FirstUpdate = default;
            LastUpdate = default;
        }
    }
}
