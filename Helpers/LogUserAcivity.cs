using datingAppreal.Extensions;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datingAppreal.Helpers
{
    public class LogUserAcivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var resultContext = await next();

           if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

           var username =  resultContext.HttpContext.User.GetUsername();

           var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepostory>();

            var user = await repo.GetUserByNameAsync(username);
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
