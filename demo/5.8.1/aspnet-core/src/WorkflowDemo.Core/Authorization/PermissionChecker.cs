using Abp.Authorization;
using WorkflowDemo.Authorization.Roles;
using WorkflowDemo.Authorization.Users;

namespace WorkflowDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
