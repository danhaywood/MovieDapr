using zipkin4net;

namespace RazorPagesMovie.Services;

public class TraceContext : ITraceContext
{
    public TraceContext(string traceparent)
    {
        
    }

    public bool? Sampled { get; set;  }
    public bool Debug { get; set; }
    public long TraceIdHigh { get; set; }
    public long TraceId { get; set;  }
    public long? ParentSpanId { get; set; }
    public long SpanId { get; set; }
    public List<object> Extra { get; set; }
}