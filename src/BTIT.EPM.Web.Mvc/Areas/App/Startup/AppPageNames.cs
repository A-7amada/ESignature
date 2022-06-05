namespace BTIT.EPM.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class Common
        {
            public const string FileSignatures = "ESignatureDemo.FileSignatures";
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            public const string DemoUiComponents = "Administration.DemoUiComponents";
            public const string UiCustomization = "Administration.UiCustomization";
            public const string WebhookSubscriptions = "Administration.WebhookSubscriptions";
            public const string DynamicEntityParameters = "Administration.DynamicEntityParameters";
            public const string DynamicParameters = "Administration.DynamicParameters";
            public const string EntityDynamicParameters = "Administration.EntityDynamicParameters";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            public const string Dashboard = "Dashboard";
        }

        public static class Tenant
        {
            public const string Contacts = "DigitalSignature.Contacts";
            public const string Recipients = "DigitalSignature.Recipients";
            public const string Documents = "Documents.Documents";
            public const string DocumentRequestAuditTrails = "DigitalSignature.DocumentRequestAuditTrails";
            public const string Dashboard = "Dashboard.Tenant";
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
            public const string DocumentRequests = "DigitalSignature.DocumentRequests";
            public const string ViewAndSignDocument = "DigitalSignature.ViewAndSignDocument";

        }
    }
}