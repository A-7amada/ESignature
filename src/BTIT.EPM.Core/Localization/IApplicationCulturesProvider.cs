using System.Globalization;

namespace BTIT.EPM.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}