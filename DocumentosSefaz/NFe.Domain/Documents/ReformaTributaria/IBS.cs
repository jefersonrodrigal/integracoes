namespace NFe.Domain.Documents.ReformaTributaria;

public class IBS
{
    public decimal vBC { get; set; }        // Base de cálculo
    public decimal pIBS { get; set; }       // Alíquota
    public decimal vIBS { get; set; }       // Valor do imposto

    public decimal vCredito { get; set; }   // Crédito aproveitado
    public decimal vDebito { get; set; }    // Débito gerado

    public string cClassTrib { get; set; }  // Código classificação tributária
    public string CST { get; set; }         // Situação tributária

    public IBSPartilha Partilha { get; set; }
}
