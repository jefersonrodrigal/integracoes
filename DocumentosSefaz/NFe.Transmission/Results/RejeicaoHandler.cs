using System.Xml;

namespace NFe.Transmission.Results;

public static class RejeicaoHandler
{
    public static SefazResult Processar(string xmlRetorno)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlRetorno);

        var cStat = doc.GetElementsByTagName("cStat")[0]?.InnerText;
        var xMotivo = doc.GetElementsByTagName("xMotivo")[0]?.InnerText;

        var result = new SefazResult
        {
            Codigo = cStat,
            Mensagem = xMotivo
        };

        switch (cStat)
        {
            case "100":
            case "135":
            case "102":
                result.Sucesso = true;
                break;

            case "204":
                result.AcaoSugerida = "Consultar protocolo e gerar procNFe.";
                break;

            case "539":
                result.AcaoSugerida = "Revisar conteúdo da NFe (diferença na duplicidade).";
                break;

            case "215":
                result.AcaoSugerida = "Revalidar XML contra XSD.";
                break;

            case "225":
                result.AcaoSugerida = "Verificar assinatura digital.";
                break;

            default:
                result.AcaoSugerida = "Analisar retorno manualmente.";
                break;
        }

        result.Sucesso ??= false;

        return result;
    }
}
