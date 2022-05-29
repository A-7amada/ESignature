using System;
using Abp.Notifications;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}