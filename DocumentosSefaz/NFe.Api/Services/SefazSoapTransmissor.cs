using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using NFe.Api.Configuration;
using NFe.PdvIntegration.Contracts;
using NFe.PdvIntegration.Interfaces;

namespace NFe.Api.Services;

public sealed class SefazSoapTransmissor : IPdvSefazTransmissor
{
    private readonly HttpClient _httpClient;
    private readonly SefazTransmissionOptions _options;

    public SefazSoapTransmissor(HttpClient httpClient, IOptions<SefazTransmissionOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<PdvSefazRetorno> EnviarAsync(string xmlAssinado, DocumentoFiscalModelo modelo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(xmlAssinado))
        {
            throw new ArgumentException("XML assinado não informado.", nameof(xmlAssinado));
        }

        var url = modelo == DocumentoFiscalModelo.NFCe
            ? _options.NFCeAutorizacaoUrl
            : _options.NFeAutorizacaoUrl;

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new InvalidOperationException("URL de transmissão SEFAZ não configurada.");
        }

        var soapEnvelope = MontarSoapEnvelope(xmlAssinado);
        using var content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml");
        using var response = await _httpClient.PostAsync(url, content, cancellationToken);
        var xmlRetorno = await response.Content.ReadAsStringAsync(cancellationToken);

        var codigo = ExtrairTag(xmlRetorno, "cStat") ?? ((int)response.StatusCode).ToString();
        var motivo = ExtrairTag(xmlRetorno, "xMotivo") ?? response.ReasonPhrase ?? "Sem retorno da SEFAZ.";

        return new PdvSefazRetorno
        {
            Sucesso = response.IsSuccessStatusCode && EhSucessoSefaz(codigo),
            Codigo = codigo,
            Mensagem = motivo,
            EmContingencia = false,
            XmlRetorno = xmlRetorno
        };
    }

    private static string MontarSoapEnvelope(string xmlAssinado)
    {
        return $@"<soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <nfeDadosMsg xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4"">
      {xmlAssinado}
    </nfeDadosMsg>
  </soap12:Body>
</soap12:Envelope>";
    }

    private static bool EhSucessoSefaz(string codigo)
    {
        return codigo is "100" or "103" or "104";
    }

    private static string? ExtrairTag(string xml, string tag)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return null;
        }

        var match = Regex.Match(xml, $"<{tag}>(.*?)</{tag}>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!match.Success)
        {
            return null;
        }

        return WebUtility.HtmlDecode(match.Groups[1].Value.Trim());
    }
}

