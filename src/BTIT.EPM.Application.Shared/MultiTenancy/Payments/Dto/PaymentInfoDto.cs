using BTIT.EPM.Editions.Dto;

namespace BTIT.EPM.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < EPMConsts.MinimumUpgradePaymentAmount;
        }
    }
}
