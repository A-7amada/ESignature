using Abp.DynamicEntityParameters;
using Abp.UI.Inputs;
using BTIT.EPM.Authorization.Users;

namespace BTIT.EPM.DynamicEntityParameters
{
    public class AppDynamicEntityParameterDefinitionProvider : DynamicEntityParameterDefinitionProvider
    {
        public override void SetDynamicEntityParameters(IDynamicEntityParameterDefinitionContext context)
        {
            context.Manager.AddAllowedInputType<SingleLineStringInputType>();
            context.Manager.AddAllowedInputType<ComboboxInputType>();
            context.Manager.AddAllowedInputType<CheckboxInputType>();

            //Add entities here 
            context.Manager.AddEntity<User, long>();
        }
    }
}
