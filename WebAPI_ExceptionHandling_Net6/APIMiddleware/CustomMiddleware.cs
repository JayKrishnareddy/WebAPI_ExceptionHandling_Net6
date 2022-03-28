namespace WebAPI_ExceptionHandling_Net6.APIMiddleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        public CustomMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }
        /// <summary>
        /// Function to Store the Exceptions Logs in the Database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, LoggingAPIContext db)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var trace = new StackTrace(contextFeature.Error, true);
                    var frame = trace.GetFrames().FirstOrDefault();
                    var lineNumber = frame.GetFileLineNumber();
                    var fileName = frame.GetFileName();
                    Log logs = new();
                    logs.Source = contextFeature.Error.Source;
                    logs.FilePath = fileName;
                    logs.LineNumber = Convert.ToInt32(lineNumber);
                    logs.ExceptionMessage = contextFeature.Error.Message;
                    logs.CretedDate = DateTime.UtcNow;
                    db.Logs.Add(logs);
                    await db.SaveChangesAsync();
                    // await SaveLogsToDatabase($"File Name : {fileName}, Line number : {lineNumber}, Exception : {contextFeature.Error.Message}", contextFeature.Error.Source);
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Exception = $"File Name : {fileName}, Line number : {lineNumber}, Exception : {contextFeature.Error.Message}"
                    }.ToString());
                }
            }
        }
    }
    /// <summary>
    /// Model to store the values of Exception Fields.
    /// </summary>
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Exception { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    /// <summary>
    /// Custom Middleware to catch the exceptions if at all there is an any issue found while running the app.
    /// </summary>
    public static class MiddlewareRegistrationExtension
    {
        public static void UseAppException(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
