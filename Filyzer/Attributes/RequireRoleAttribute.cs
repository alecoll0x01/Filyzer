using Filyzer.Domain.Enums;

namespace Filyzer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequireRoleAttribute : Attribute
    {
        public UserRole[] RequiredRoles { get; }

        public RequireRoleAttribute(params UserRole[] roles) => RequiredRoles = roles;
    }
}
