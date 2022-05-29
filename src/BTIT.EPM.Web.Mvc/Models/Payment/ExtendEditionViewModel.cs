using System.Collections.Generic;
using BTIT.EPM.Editions.Dto;
using BTIT.EPM.MultiTenancy.Payments;

namespace BTIT.EPM.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}