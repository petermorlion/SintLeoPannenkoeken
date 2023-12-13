using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.Filters
{
    /// <summary>
    /// This filter redirects to the same controller and action but with a valid scoutsjaar querystring
    /// parameter, if not already present.
    /// </summary>
    public class ScoutsjaarRedirectionFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _dbContext;

        public ScoutsjaarRedirectionFilter(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Scoutsjaar currentScoutsjaar;
            var controller = context.Controller as Controller;
            var controllerName = controller.RouteData.Values["controller"];
            var actionName = controller.RouteData.Values["action"];
            if (context.ActionArguments.TryGetValue("scoutsjaar", out object? scoutsjaarParameter))
            {
                if (scoutsjaarParameter != null && int.TryParse(scoutsjaarParameter.ToString(), out int scoutsjaar))
                {
                    Scoutsjaar? sj = await _dbContext.Scoutsjaren.SingleOrDefaultAsync(s => s.Begin == scoutsjaar);
                    if (sj == null)
                    {
                        currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                        context.Result = new RedirectResult($"/{controllerName}/{actionName}?scoutsjaar={currentScoutsjaar.Begin}");
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                    context.Result = new RedirectResult($"/{controllerName}/{actionName}?scoutsjaar={currentScoutsjaar.Begin}");
                }
            }
            else
            {
                currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                context.Result = new RedirectResult($"/{controllerName}/{actionName}?scoutsjaar={currentScoutsjaar.Begin}");
            }
        }
    }
}
