using System;
using System.Security.Cryptography.X509Certificates;
using Digst.OioIdws.CommonCore;
using Digst.OioIdws.OioWsTrustCore;

namespace Digst.OioIdws.WscCore.OioWsTrust
{
    /// <summary>
    /// This factory class can be used to generate a <see cref="StsTokenServiceConfiguration"/> configuration based on a <see cref="OioIdwsWcfConfigurationSection"/> configuration.
    /// </summary>
    public static class TokenServiceConfigurationFactory
    {
        /// <summary>
        /// This method creates a new instance of the StsTokenServiceConfiguration class by taking an instance of OioIdwsWcfConfigurationSection as input.
        /// </summary>
        /// <param name="wscConfiguration">An instance of <see cref="OioIdwsWcfConfigurationSection"/></param>
        /// <returns>An instance of <see cref="StsTokenServiceConfiguration"/></returns>
        /// <exception cref="ArgumentNullException">When the input wscConfiguration parameter is null</exception>
        public static StsTokenServiceConfiguration CreateConfiguration(OioIdwsWcfConfigurationSection wscConfiguration)
        {
            if (wscConfiguration == null)
            {
                throw new ArgumentNullException("wscConfiguration");
            }

            X509Certificate2 stsCertificate = CertificateUtil.GetCertificate(wscConfiguration.StsCertificate);
            X509Certificate2 clientCertificate = CertificateUtil.GetCertificate(wscConfiguration.ClientCertificate);

            var stsConfiguration = new StsConfiguration(wscConfiguration.StsEndpointAddress, stsCertificate);
            var wspConfiguration = new WspConfiguration(wscConfiguration.WspEndpointId, clientCertificate);

            var tokenServiceConfiguration = new StsTokenServiceConfiguration(stsConfiguration, wspConfiguration, clientCertificate)
            {
                MaxReceivedMessageSize = wscConfiguration.MaxReceivedMessageSize,
            };

            return tokenServiceConfiguration;
        }

        /// <summary>
        /// This method creates a new instance of the StsTokenServiceConfiguration class using the oioIdwsWcfConfiguration section specified in the application configuration file.
        /// </summary>
        /// <param name="wscConfiguration">An instance of <see cref="OioIdwsWcfConfigurationSection"/></param>
        /// <returns>An instance of <see cref="StsTokenServiceConfiguration"/></returns>
        /// <exception cref="ArgumentNullException">When the input wscConfiguration parameter is null</exception>
        public static StsTokenServiceConfiguration CreateConfiguration()
        {
            var wscConfiguration =
                (OioIdwsWcfConfigurationSection)System.Configuration.ConfigurationManager.GetSection("oioIdwsWcfConfiguration");

            return CreateConfiguration(wscConfiguration);
        }
    }
}