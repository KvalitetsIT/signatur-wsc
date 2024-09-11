using System;
using System.Security.Cryptography.X509Certificates;

namespace Digst.OioIdws.CommonCore
{
    /// <summary>
    /// Contains all settings needed to call an STS
    /// </summary>
    public class StsConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StsConfiguration"/> class.
        /// </summary>
        public StsConfiguration(string stsEndpointAddress, X509Certificate2 stsCertificate)
        {
            if (string.IsNullOrEmpty(stsEndpointAddress))
                throw new ArgumentNullException(nameof(stsEndpointAddress));

            EndpointAddress = stsEndpointAddress;
            Certificate = stsCertificate ?? throw new ArgumentNullException(nameof(stsCertificate));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StsConfiguration"/> class.
        /// </summary>
        public StsConfiguration(StsConfiguration other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            EndpointAddress = other.EndpointAddress;
            Certificate = other.Certificate;
        }

        /// <summary>
        /// Endpoint address of STS. E.g. https://adgangsstyring.eksterntest-stoettesystemerne.dk/runtime/services/kombittrust/14/certificatemixed
        /// </summary>
        public string EndpointAddress { get; }

        /// <summary>
        /// Represents the STS certificate containing only the public key. This should be a FOCES certificate.
        /// </summary>
        public X509Certificate2 Certificate { get; }

    }
}
