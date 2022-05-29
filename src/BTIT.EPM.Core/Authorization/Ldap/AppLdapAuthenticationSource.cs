using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using BTIT.EPM.Authorization.Users;
using BTIT.EPM.MultiTenancy;

namespace BTIT.EPM.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}