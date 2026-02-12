using System.Xml.Serialization;

namespace NFe.Transmission.Inutilizacao;

public class InfInut
{
    [XmlAttribute("Id")]
    public string Id { get; set; }

    public int tpAmb { get; set; }
    public int xServ { get; set; } = 0; // Não usar (removido no 4.00)
    public int cUF { get; set; }
    public string ano { get; set; }
    public string CNPJ { get; set; }
    public int mod { get; set; } = 55;
    public int serie { get; set; }
    public int nNFIni { get; set; }
    public int nNFFin { get; set; }
    public string xJust { get; set; }
}
