using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Exceptions
{
    public class HandleUserExceptions : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            Debug.WriteLine(context.Exception);

            
            if (exception is UserMessageException userException)
            {
                context.Result = new OkObjectResult(new OperationResult()
                {
                    IsSuccess = false,
                    Message = userException.Message
                });
            }
            else
                base.OnException(context);
        }
    }
}