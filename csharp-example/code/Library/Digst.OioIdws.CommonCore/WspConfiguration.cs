using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Digst.OioIdws.CommonCore
{
    public class WspConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WspConfiguration"/> class.
        /// </summary>
        public WspConfiguration(string wspEndpointId, X509Certificate2 wspCertificate)
        {
            EndpointId = wspEndpointId;
            ServiceCertificate = wspCertificate ?? throw new ArgumentNullException(nameof(wspCertificate));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WspConfiguration"/> class.
        /// </summary>
        public WspConfiguration(WspConfiguration other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            EndpointId = other.EndpointId;
            ServiceCertificate = other.ServiceCertificate;
        }

        /// <summary>
        /// Endpoint of WSP. E.g. https://wsp.dk/service/query/2
        /// </summary>
        public string EndpointId { get; }

        /// <summary>
        /// Represents the service certificate containing only the public key.
        /// </summary>
        public X509Certificate2 ServiceCertificate { get; }
    }
}
