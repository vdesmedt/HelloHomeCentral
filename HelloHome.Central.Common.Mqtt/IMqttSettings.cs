namespace HelloHome.Central.Common.Mqtt;

public interface IMqttSettings
{
    string Host { get; set; }
    int Port { get; set; }
    string ClientId { get; set; }
    string? User { get; set; }
    string? Password { get; set; }
    bool Tls { get; set; }
    bool CleanSession { get; set; }
    int KeepAliveSeconds { get; set; }
    bool UseManagedClient { get; set; } 
}