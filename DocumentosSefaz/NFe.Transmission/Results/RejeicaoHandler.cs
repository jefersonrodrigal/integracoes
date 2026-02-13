using NFe.Transmission.Results;
using System.Xml;

public static class RejeicaoHandler
{
    public static SefazResult Processar(string xmlRetorno)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlRetorno);

        var cStatNode = doc.GetElementsByTagName("cStat");
        var cStat = cStatNode.Count > 0 ? cStatNode[0].InnerText : null;
        var xMotivo = doc.GetElementsByTagName("xMotivo")[0]?.InnerText;

        var result = new SefazResult
        {
            Codigo = cStat,
            Mensagem = xMotivo,
            Sucesso = false // define padrão aqui se quiser explícito
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

        return result;
    }
}
