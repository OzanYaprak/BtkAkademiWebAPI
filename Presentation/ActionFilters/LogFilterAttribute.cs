using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        #region Constructor

        private readonly ILoggerService _loggerService;

        public LogFilterAttribute(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        #endregion Constructor

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _loggerService.LogInfo(Log("OnActionExecuting", context.RouteData));
        }

        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"]
            };

            if (routeData.Values.Count >= 3)
            {
                logDetails.Id = routeData.Values["Id"];
            }

            return logDetails.ToString();
        }
    }
}