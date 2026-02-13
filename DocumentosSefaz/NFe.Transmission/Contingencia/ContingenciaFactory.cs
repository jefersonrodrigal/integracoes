using NFe.Transmission;
using NFe.Transmission.Contingencia;
using NFe.Transmission.Interfaces;
using NFe.Transmission.Response;

public class ContingenciaFactory : IContingenciaFactory
{
    private readonly ISefazHealthChecker _healthChecker;

    public ContingenciaFactory(ISefazHealthChecker healthChecker)
    {
        _healthChecker = healthChecker;
    }

    public IContingenciaStrategy Criar(SefazResponse response)
    {
        if (_healthChecker.DeveAtivarContingencia(response))
            return new SvcStrategy();

        return new NormalStrategy();
    }
}