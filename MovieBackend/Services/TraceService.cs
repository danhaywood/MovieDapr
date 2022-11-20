using zipkin4net;

namespace RazorPagesMovie.Services
{
    public class TraceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TraceService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<T> Execute<T> (Task<T> task)
        {
            
            var request = _httpContextAccessor.HttpContext!.Request;
            
            if (request.Headers.ContainsKey("traceparent"))
            {
                var value = request.Headers["traceparent"][0];
                Console.Out.WriteLine($"Trace start: {value}");
                var parentTrace = Trace.Create();
                
                if (parentTrace == null)
                {
                    parentTrace = Trace.CreateFromId(new TraceContext(value));
                }

                var child = parentTrace.Child();
                child.Record(Annotations.LocalOperationStart("database"));
                child.Record(Annotations.ServiceName("expense"));
                child.Record(Annotations.Tag("request", "list-expenses"));
                child.Record(Annotations.Rpc("SELECT"));

                task.GetAwaiter().OnCompleted(continuation: () =>
                {
                    child.Record(Annotations.LocalOperationStop());
                    Console.Out.WriteLine($"Trace end: {value}");
                });
            }
            return await task;
        }
    }
}