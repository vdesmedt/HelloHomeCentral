namespace HelloHome.Central.Common.Configuration
{
    public class HhConfig
    {
        public virtual string ConnectionString { get; set; }
        public virtual SerialConfig SerialConfig { get; set; }
        public virtual EmonCmsConfig EmonCmsConfig { get; set; }
    }
}