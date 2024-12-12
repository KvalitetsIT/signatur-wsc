Example code C#
The C# example code can be downloaded at https://github.com/KvalitetsIT/signatur-
wsc/CSharp.
Do the following in order to get the C# example up and running:
1. Download the Kithosting STS certificate: https://github.com/KvalitetsIT/signatur-wsc/tree/main/stsCerts/sts.cer. The certificate is used together with your own certificate
to request a token on the STS server.
2. Install both Kithosting STS and your own certificate in &quot;Local Computer -&gt; Trusted Root
Certificates”.
3. Open the C# solution through the main solution file: Main -&gt; Main.sln.
4. Edit the Main -&gt; App.config file:
a. Edit configuration/oioIdwsWcfConfiguration/clientCertificate so that it contains the
thumbprint on your certificate.
b. The configuration/oioIdwsWcfConfiguration/stsCertificate configuration is already
set to Kithosting STS certificate.
5. Edit the Main -&gt; Program.cs file:
a. In line 64, setup the input parameter for the query. For example:
StringContent jsonContent = new StringContent(@&quot;{&quot;&quot;PeriodFrom&quot;&quot;:&quot;&quot;2024-06-
01T00:00:00&quot;&quot;,&quot;&quot;PeriodTo&quot;&quot;:&quot;&quot;2024-09- 30T23:59:59&quot;&quot;}&quot;, Encoding.UTF8,
&quot;application/json&quot;);
b. In line 67, setup the URL to the API endpoint. For example:
RequestUri = new Uri(&quot;https://portalapi.signatur.dk/recruitment/statistics/1&quot;)

6. In the Main -&gt; Program.cs file, notice the following important lines:
a.
7. The example is now ready to be executed.
a. Note that in the example only very basic error handling is included. Something
more advanced is probably needed.

Når ovenstående er klaret, er i klar til at lave en forespørgsel. I skal i første omgang forholde
jer til Main -&gt; Program.cs, da her laves både forespørgsel på token, forespørgsel på API,
samt håndtering af svar fra API.
1. In line 30, the token is retrieved from the STS server:
securityToken = (GenericXmlSecurityToken)stsTokenService.GetToken();
2. In line 75, the API request is executed:
HttpResponseMessage serviceResult = client.SendAsync(request).Result;
3. In line 94, the response for the API request is displayed:
Console.WriteLine(serviceResult.Content.ReadAsStringAsync().Result);X
