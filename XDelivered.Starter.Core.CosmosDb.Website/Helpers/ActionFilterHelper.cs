using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public class ActionFilterHelper
    {
        public static Type GetExpectedReturnType(ActionExecutingContext filterContext)
        {
            // Find out what type is expected to be returned
            var actionName = filterContext.ActionDescriptor.RouteValues["action"];
            var controllerType = filterContext.Controller.GetType();
            var actionMethodInfo = default(MethodInfo);
            try
            {
                actionMethodInfo = controllerType.GetMethod(actionName);
            }
            catch (AmbiguousMatchException)
            {
                // Try to find a match using the parameters passed through
                var actionParams = filterContext.ActionArguments;
                var paramTypes = new List<Type>();
                foreach (var p in actionParams)
                    paramTypes.Add(p.Value.GetType());

                actionMethodInfo = controllerType.GetMethod(actionName, paramTypes.ToArray());
            }

            return actionMethodInfo.ReturnType;
        }

        public static object CreateOperationResult(FilterContext context, Type returnType = null)
        {
            var operationResult = typeof(OperationResult<>);
            Type[] payloadTypeArgs = { returnType };
            var makeme = operationResult.MakeGenericType(payloadTypeArgs);
            var result = Activator.CreateInstance(makeme);
            return result;
        }
    }
}