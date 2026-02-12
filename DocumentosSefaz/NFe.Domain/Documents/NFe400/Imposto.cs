using NFe.Domain.Documents.ReformaTributaria;

namespace NFe.Domain.Documents.NFe400;

public class Imposto
{
    public ICMS ICMS { get; set; }
    public IPI IPI { get; set; }
    public PIS PIS { get; set; }
    public COFINS COFINS { get; set; }

    // Grupo futuro – reforma tributária
    public ImpostoReforma ImpostoReforma { get; set; }
}
