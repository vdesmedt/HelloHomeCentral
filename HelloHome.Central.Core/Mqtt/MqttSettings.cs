namespace HelloHome.Central.Core.Mqtt;

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

public sealed class MqttSettings : IMqttSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1883;
    public string ClientId { get; set; } = "my-app";
    public string? User { get; set; }
    public string? Password { get; set; }
    public bool Tls { get; set; }
    public bool CleanSession { get; set; } = true;
    public int KeepAliveSeconds { get; set; } = 60;
    public bool UseManagedClient { get; set; } = true; // recommended
}
