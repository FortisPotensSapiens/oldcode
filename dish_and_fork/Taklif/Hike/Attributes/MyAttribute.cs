using System.Data;
using System.Security.Claims;
using Hike.Ef;
using Hike.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Hike.Attributes
{
    public class MyAttribute : TypeFilterAttribute
    {
        public const string ADMIN = "admin";
        public const string SELLER = "seller";

        public MyAttribute(params string[] ids) : base(typeof(MyAttributeImpl))
        {
            Arguments = new object[] { ids };
        }

        private class MyAttributeImpl : IActionFilter
        {
            private readonly string[] _ids;

            public MyAttributeImpl(string[] ids)
            {
                _ids = ids;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var userId = context?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return;
                var roles = context.HttpContext.User.Claims.ToList();
                //    var ur = db.UserRoles.Where(x => x.UserId == userId).AsNoTracking().ToList();
                //    var r = db.Roles.Where(x => ur.Select(r => r.RoleId).Contains(x.Id)).AsNoTracking().ToList();
                foreach (var id in _ids)
                {
                    if (roles.Any(x => x.Value?.GetNormalizedName()?.Contains(id.GetNormalizedName()) == true))
                        continue;
                    context.Result = new ForbidResult();
                    return;
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
