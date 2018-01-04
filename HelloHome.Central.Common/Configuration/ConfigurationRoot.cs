namespace HelloHome.Central.Common.Configuration
{
    public class ConfigurationRoot
    {
        public virtual string ConnectionString { get; set; }
        public virtual EmonCmsConfig EmonCmsConfig { get; set; }
    }

    public class EmonCmsConfig
    {
        public virtual string ApiKey { get; set; }
    }
}