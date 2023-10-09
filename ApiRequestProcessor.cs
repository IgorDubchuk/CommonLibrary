using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommonLibrary
{
    public static class ApiRequestProcessor
    {
        public static IActionResult ProcessApiRequest(Action processApiRequest, ILogger logger)
        {
            try
            {
                processApiRequest();
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while API request processing.");
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500,
                };
            }
        }
        public static ActionResult<ResponseType> ProcessApiRequest<ResponseType>(Func<ResponseType> processApiRequest, ILogger logger)
        {
            try
            {
                var result = processApiRequest();
                return new ObjectResult(result)
                {
                    StatusCode = 200,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while API request processing.");

                var exType = ex.GetType().ToString();
                short statusCode = exType switch
                {
                    "System.ApplicationException" => 500,
                    _ => 500
                };
                return new ObjectResult(ex.Message)
                {
                    StatusCode = statusCode,
                };
            }
        }
    }
}