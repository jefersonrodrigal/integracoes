using NFe.Transmission.Services.Enuns;
using System;
using System.Xml.Serialization;

namespace NFe.Domain.Models;

public class Ide
{
    [XmlElement("cUF")]
    public int CUF { get; set; }

    [XmlElement("cNF")]
    public string CNF { get; set; }

    [XmlElement("natOp")]
    public string NatOp { get; set; }

    [XmlElement("mod")]
    public int Mod { get; set; }

    [XmlElement("serie")]
    public int Serie { get; set; }

    [XmlElement("nNF")]
    public int NNF { get; set; }

    [XmlElement("dhEmi")]
    public DateTimeOffset DhEmi { get; set; }

    [XmlElement("tpNF")]
    public int TpNF { get; set; }

    [XmlElement("idDest")]
    public int IdDest { get; set; }

    [XmlElement("cMunFG")]
    public int CMunFG { get; set; }

    [XmlElement("tpImp")]
    public int TpImp { get; set; }

    [XmlElement("tpEmis")]
    public TipoEmissao TpEmis { get; set; }

    [XmlElement("tpAmb")]
    public int TpAmb { get; set; }

    [XmlElement("finNFe")]
    public int FinNFe { get; set; }

    [XmlElement("indFinal")]
    public int IndFinal { get; set; }

    [XmlElement("indPres")]
    public int IndPres { get; set; }

    [XmlElement("procEmi")]
    public int ProcEmi { get; set; }

    [XmlElement("verProc")]
    public string VerProc { get; set; }

    // Campos de contingência
    [XmlElement("dhCont")]
    public DateTimeOffset? DhCont { get; set; }

    [XmlElement("xJust")]
    public string XJust { get; set; }
}
