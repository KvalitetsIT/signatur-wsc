using System.Configuration;

namespace Digst.OioIdws.WscCore.OioWsTrust
{
    /// <summary>
    /// XML Configuration object which reads data from the oioIdwsWcfConfiguration configuration section in app.config
    /// </summary>
    public class OioIdwsWcfConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("stsEndpointAddress", IsRequired = true)]
        public string StsEndpointAddress
        {
            get
            {
                return (string)this["stsEndpointAddress"];
            }
            set
            {
                this["stsEndpointAddress"] = value;
            }
        }

        [ConfigurationProperty("wspEndpointId", IsRequired = true)]
        public string WspEndpointId
		{
            get
            {
                return (string)this["wspEndpointId"];
            }
            set
            {
                this["wspEndpointId"] = value;
            }
        }

        [ConfigurationProperty("maxReceivedMessageSize", IsRequired = false)]
        public int? MaxReceivedMessageSize
        {
            get
            {
                return (int?)this["maxReceivedMessageSize"];
            }
            set
            {
                this["maxReceivedMessageSize"] = value;
            }
        }

        [ConfigurationProperty("clientCertificate", IsRequired = true)]
        public Certificate ClientCertificate
        {
            get
            {
                return (Certificate) this["clientCertificate"];
            }
            set
            {
                this["clientCertificate"] = value;
            }
        }

        [ConfigurationProperty("stsCertificate", IsRequired = true)]
        public Certificate StsCertificate
        {
            get
            {
                return (Certificate)this["stsCertificate"];
            }
            set
            {
                this["stsCertificate"] = value;
            }
        }
    }
}
