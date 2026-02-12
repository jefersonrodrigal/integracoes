using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NFe.Transmission;

public class NFeClient
{
    private readonly HttpClient _http;

    public NFeClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> EnviarAsync(string xmlAssinado, string url)
    {
        var soapEnvelope = $@"
<soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <nfeDadosMsg xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4"">
      {xmlAssinado}
    </nfeDadosMsg>
  </soap12:Body>
</soap12:Envelope>";

        var content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml");

        var response = await _http.PostAsync(url, content);

        return await response.Content.ReadAsStringAsync();
    }
}
