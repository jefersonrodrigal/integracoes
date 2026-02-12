using NFe.Transmission.WebServices;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class NFeWebService : INFeWebService
{
    private readonly HttpClient _http;
    private readonly SefazEndpointResolver _resolver;

    public NFeWebService(X509Certificate2 certificado)
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificado);

        _http = new HttpClient(handler);
        _resolver = new SefazEndpointResolver();
    }

    public async Task<string> EnviarAutorizacaoAsync(string xml)
    {
        var url = _resolver.ObterUrlAutorizacao(35, 2);

        var envelope = SoapEnvelopeBuilder.Build(
            xml,
            "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4");

        var content = new StringContent(envelope, Encoding.UTF8, "application/soap+xml");

        var response = await _http.PostAsync(url, content);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> EnviarEventoAsync(string xml)
    {
        var url = "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeRecepcaoEvento4.asmx";

        var envelope = SoapEnvelopeBuilder.Build(
            xml,
            "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRecepcaoEvento4");

        var content = new StringContent(envelope, Encoding.UTF8, "application/soap+xml");

        var response = await _http.PostAsync(url, content);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> EnviarInutilizacaoAsync(string xml)
    {
        var url = "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeInutilizacao4.asmx";

        var envelope = SoapEnvelopeBuilder.Build(
            xml,
            "http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4");

        var content = new StringContent(envelope, Encoding.UTF8, "application/soap+xml");

        var response = await _http.PostAsync(url, content);

        return await response.Content.ReadAsStringAsync();
    }
}
