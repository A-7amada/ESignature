using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using BTIT.EPM.Queries.Container;

namespace BTIT.EPM.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}