using System.Threading.Tasks;
using BTIT.EPM.Notifications;
using BTIT.EPM.Test.Base;
using Shouldly;
using Xunit;

namespace BTIT.EPM.Tests.Notifications
{
    // ReSharper disable once InconsistentNaming
    public class NotificationAppService_Tests : AppTestBase
    {
        private readonly INotificationAppService _notificationAppService;

        public NotificationAppService_Tests()
        {
            _notificationAppService = Resolve<INotificationAppService>();
        }

        [Fact]
        public async Task Test_ChangeNotificationSettings()
        {
            var settings = await _notificationAppService.GetNotificationSettings();
            settings.ReceiveNotifications.ShouldBe(true);
            settings.Notifications.Count.ShouldBeGreaterThan(0);
        }
    }
}
