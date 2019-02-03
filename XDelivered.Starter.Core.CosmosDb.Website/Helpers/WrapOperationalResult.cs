using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{

    /// <summary>
    ///     Will transform successful Controller actions into <see cref="OperationResult{T}" /> objects that are rich Data
    ///     Transfer objects containing meta data around the result, such as <see cref="OperationResult.IsSuccess" /> and
    ///     <see cref="OperationResult.Messages" />
    /// </summary>
    public class WrapOperationalResult : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (context.Cancel) return;

            if (IsAlreadyPayload(context))
            {
                base.OnResultExecuting(context);
                return;
            }

            //pass through any model/mvc level errors
            if (context.Result is ObjectResult badRequest && (badRequest.StatusCode.HasValue && badRequest.StatusCode != 200))
            {
                var payloadType = badRequest.Value.GetType();

                WrapAndDeliveryOperationalResult(context, payloadType, badRequest, error:true);
            }
            //handle IActionResult return type
            else if (context.Result is OkObjectResult okPayload)
            {
                var payloadType = okPayload.Value.GetType();

                WrapAndDeliveryOperationalResult(context, payloadType, okPayload);
            }
            else if (context.Result is JsonResult json)
            {
                var payloadType = json.Value.GetType();

                WrapAndDeliveryOperationalResult(context, payloadType, new ObjectResult(json.Value));
            }

            //Handle POCO return type
            else if (context.Result is ObjectResult payload)
            {
                var payloadType = payload.Value.GetType();

                WrapAndDeliveryOperationalResult(context, payloadType, payload);
            }

            //Handle void return type
            if (context.Result is EmptyResult emptyResult)
            {
                context.Result = new OkObjectResult(new OperationResult
                {
                    IsSuccess = true
                });
            }

            if (context.Result is OkResult okResult)
            {
                context.Result = new OkObjectResult(new OperationResult
                {
                    IsSuccess = true
                });
            }

        }

        private bool IsAlreadyPayload(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult payload)
                return payload.Value is OperationResult;
            return false;
        }

        private static void WrapAndDeliveryOperationalResult(ResultExecutingContext context, Type payloadType, ObjectResult payload, bool error = false)
        {
            //create operational result (with generics)
            var operationResult = ActionFilterHelper.CreateOperationResult(
                context,
                payloadType);

            //set data object
            operationResult.GetType().GetProperty("Data").SetValue(operationResult, payload.Value);

            if (operationResult is OperationResult success && !error)
                success.IsSuccess = true;

            context.Result = new OkObjectResult(operationResult);
        }
    }
}
