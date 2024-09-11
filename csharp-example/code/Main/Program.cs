using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using Digst.OioIdws.CommonCore.Logging;
using Digst.OioIdws.OioWsTrustCore;
using Digst.OioIdws.WscCore.OioWsTrust;

namespace Main
{

	internal static class Program
	{
		static void Main(string[] args)
		{
			Log4NetLogger.Initialize();
			StsTokenServiceConfiguration stsConfiguration = TokenServiceConfigurationFactory.CreateConfiguration();

			try
			{
				// Get token from STS.
				IStsTokenService stsTokenService = new StsTokenServiceCache(stsConfiguration);
				GenericXmlSecurityToken securityToken;

				try
				{
					securityToken = (GenericXmlSecurityToken)stsTokenService.GetToken();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					Logger.Instance.Error(e.Message, e);
					throw;
				}

				Console.WriteLine("STS Token: " + securityToken.TokenXml.OuterXml);

				// Configure HTTP client to use mTLS
				HttpClientHandler httpClientHandler = new HttpClientHandler();

				httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
				httpClientHandler.ClientCertificates.Add(stsConfiguration.ClientCertificate);
				httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

				using (HttpClient client = new HttpClient(httpClientHandler))
				{
					// Authorize against service
					string assertionString = securityToken.TokenXml.OuterXml;
					StringContent content = new StringContent("saml-token=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(assertionString)));

					HttpResponseMessage authorizeResult = client.PostAsync("https://portalapi.signatur.dk/token", content).Result;

					if (!authorizeResult.IsSuccessStatusCode)
						throw new Exception(authorizeResult.ToString());

					// Parse response
					string authorizeContent = authorizeResult.Content.ReadAsStringAsync().Result;
					WspAccessToken? accessToken = JsonSerializer.Deserialize<WspAccessToken>(authorizeContent);

					// Prepare call service
					StringContent jsonContent = new StringContent(@"{""PeriodFrom"":""2024-06-01T00:00:00"",""PeriodTo"":""2024-09-30T23:59:59""}", Encoding.UTF8, "application/json");
					HttpRequestMessage request = new HttpRequestMessage()
					{
						RequestUri = new Uri("https://portalapi.signatur.dk/recruitment/statistics/1"),
						Method = HttpMethod.Post,
						Content = jsonContent
					};
					request.Headers.Add("Authorization", "Holder-of-key " + accessToken?.AccessToken);
					request.Headers.Add("accept", "application/json");

					// Call service
					HttpResponseMessage serviceResult = client.SendAsync(request).Result;

					if (!serviceResult.IsSuccessStatusCode)
					{
						switch (serviceResult.StatusCode)
						{
							case HttpStatusCode.BadRequest:
								try
								{
									Console.WriteLine(serviceResult.Content.ReadAsStringAsync().Result);
								}
								catch { }

								break;
						}

						throw new Exception(serviceResult.ToString());
					}

					Console.WriteLine(serviceResult.Content.ReadAsStringAsync().Result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Logger.Instance.Error(ex.Message, ex);
				Console.ReadKey();
				return;
			}

			Console.ReadKey();
		}
	}
}
