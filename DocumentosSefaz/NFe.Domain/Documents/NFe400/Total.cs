namespace NFe.Domain.Documents.NFe400;

public class Total
{
    public ICMSTot ICMSTot { get; set; }
}

public class ICMSTot
{
    public decimal vBC { get; set; }
    public decimal vICMS { get; set; }
    public decimal vICMSDeson { get; set; }
    public decimal vFCP { get; set; }
    public decimal vBCST { get; set; }
    public decimal vST { get; set; }
    public decimal vProd { get; set; }
    public decimal vFrete { get; set; }
    public decimal vSeg { get; set; }
    public decimal vDesc { get; set; }
    public decimal vII { get; set; }
    public decimal vIPI { get; set; }
    public decimal vPIS { get; set; }
    public decimal vCOFINS { get; set; }
    public decimal vOutro { get; set; }
    public decimal vNF { get; set; }
}
