using NFe.Transmission.Contingencia;

public static class ContingenciaFactory
{
    public static IContingenciaStrategy Criar(string codigoSefaz)
    {
        if (SefazHealthChecker.DeveAtivarContingencia(codigoSefaz))
            return new SvcStrategy();

        return new NormalStrategy();
    }
}
