using Abp.Configuration;

namespace BTIT.EPM.Timing.Dto
{
    public class GetTimezoneComboboxItemsInput
    {
        public SettingScopes DefaultTimezoneScope;

        public string SelectedTimezoneId { get; set; }
    }
}
