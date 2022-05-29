namespace BTIT.EPM.Url
{
    public interface IAppUrlService
    {
        string CreateEmailActivationUrlFormat(int? tenantId);

        string CreatePasswordResetUrlFormat(int? tenantId);
        string CreateViewAndSignDocumentAsyncUrlFormat(bool hasPin, int? tenantId);

        string CreateEmailActivationUrlFormat(string tenancyName);

        string CreatePasswordResetUrlFormat(string tenancyName);

        string CreateViewAndSignDocumentAsyncUrlFormat(bool hasPin, string tenancyName);
    }
}
