namespace HostCoreDocker
{
    using System;
    public class ConfigurationException : Exception
    {
        public string ConfigurationKey { get; set; }
        public string ConfigurationDescription { get; set; }
    }
}
