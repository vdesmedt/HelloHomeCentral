namespace HelloHome.Central.Common.Performance;

public interface IPerformanceStats
{
    Call StartCall();
    void AddHandlerCall(Call call);
    long CallCount { get; }
}