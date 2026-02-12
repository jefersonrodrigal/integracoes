namespace NFe.Domain.Models;

public class ICMS30
{
    public int orig { get; set; }
    public string CST { get; set; }

    public int modBCST { get; set; }
    public decimal pMVAST { get; set; }
    public decimal vBCST { get; set; }
    public decimal pICMSST { get; set; }
    public decimal vICMSST { get; set; }
}
