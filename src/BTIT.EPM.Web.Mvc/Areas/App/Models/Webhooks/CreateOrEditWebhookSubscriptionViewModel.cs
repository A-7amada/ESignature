﻿using Abp.Application.Services.Dto;
using Abp.Webhooks;
using BTIT.EPM.WebHooks.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
