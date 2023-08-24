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

           var userId =  resultContext.HttpContext.User.GetUserId();

           var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepostory.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}
