# Projeto NF-e/NFC-e (.NET 10)

## Vis?o Geral
Este projeto implementa uma solu??o completa para emiss?o, assinatura, valida??o e transmiss?o de NF-e (modelo 55) e NFC-e (modelo 65) utilizando .NET 10, seguindo as melhores pr?ticas fiscais e t?cnicas.

---

## Integra??o com PDV

O projeto agora possui uma camada dedicada `NFe.PdvIntegration`, pronta para ser consumida por API/ERP/PDV.

### Recursos para PDV
- Contratos de entrada/sa?da (`PdvEmissaoRequest` / `PdvEmissaoResponse`)
- Servi?o orquestrador ?nico (`IPdvNFeService`)
- Gera??o de XML de NF-e e NFC-e com Zeus
- Assinatura digital opcional (com `IXmlSignatureService`)
- Transmiss?o opcional para SEFAZ via interface plug?vel (`IPdvSefazTransmissor`)
- Correla??o de requisi??o (`CorrelationId`) para rastreabilidade no PDV

### Exemplo de registro no DI

```csharp
using Microsoft.Extensions.DependencyInjection;
using NFe.PdvIntegration.Extensions;
using NFe.PdvIntegration.Interfaces;
using NFe.PdvIntegration.Services;

services.AddNFePdvIntegration(options =>
{
    options.AssinarXmlPorPadrao = true;
    options.EnviarParaSefazPorPadrao = false;
});

services.AddSingleton<ICertificadoProvider>(sp =>
    new FixedCertificadoProvider(certificadoDigital));

// Opcional: transmissor simulado para homologa??o de integra??o com PDV
services.AddSingleton<IPdvSefazTransmissor, FakeSefazTransmissor>();
```

### Exemplo de emiss?o consumida pelo PDV

```csharp
using NFe.PdvIntegration.Contracts;
using NFe.PdvIntegration.Interfaces;

var service = provider.GetRequiredService<IPdvNFeService>();

var resposta = await service.EmitirAsync(new PdvEmissaoRequest
{
    CorrelationId = "pdv-001-000123",
    EmpresaId = "loja-001",
    DocumentoZeus = documentoZeus,
    Modelo = DocumentoFiscalModelo.NFCe,
    AssinarXml = true,
    EnviarParaSefaz = false
});

// resposta.XmlGerado / resposta.XmlAssinado / resposta.RetornoSefaz
```

---

## Exemplo r?pido com Zeus.Net.NFe.NFCe

### Gerar XML de NF-e

```csharp
using NFe.Builders.Examples;

var xmlNFe = ZeusXmlExamples.GerarXmlNFeExemplo();
```

### Gerar XML de NFC-e

```csharp
using NFe.Builders.Examples;

var xmlNFCe = ZeusXmlExamples.GerarXmlNFCeExemplo();
```

### Usando seus pr?prios objetos Zeus

```csharp
using NFe.Builders;
using NFe.Classes;

var documentoZeus = new NFe.Classes.NFe();

var nfeBuilder = new NFeBuilder();
var xmlNFe = nfeBuilder.GerarXml(documentoZeus);

var nfceBuilder = new NFCeBuilder();
var xmlNFCe = nfceBuilder.GerarXml(documentoZeus);
```

---

## O que foi implementado
- **Estrutura modular**: Separa??o em projetos para dom?nio, infraestrutura, assinatura, valida??o, serializa??o, transmiss?o e builders.
- **Camada PDV**: Novo projeto `NFe.PdvIntegration` com contratos, servi?o de emiss?o e extens?o de DI.
- **Enum de ambiente**: `Environment` para Homologa??o e Produ??o.
- **Carregamento de certificado digital**: Classe `CertificateLoader` para carregar certificados A1 (arquivo PFX) e do reposit?rio do Windows (por subject name ou thumbprint).
- **Assinatura digital de XML**: Classe `NFeSigner` e helper `XmlSignatureHelper` para assinar XMLs fiscais com certificado digital.
- **Serializa??o XML**: Helper para serializar objetos para XML conforme layout da SEFAZ.
- **Integra??o Zeus.Net.NFe.NFCe**: Implementa??o para serializar objetos Zeus em XML de NF-e e NFC-e.
- **Prepara??o para integra??o com schemas oficiais**: Orienta??es e automa??o para uso dos XSDs oficiais da SEFAZ (NF-e 4.00).
- **Cria??o autom?tica do wrapper XSD**: `NFe_Geracao.xsd` para facilitar a gera??o das classes.
- **Remo??o do projeto de console**: O projeto `NFe.ConsoleApp` foi removido da solu??o.
- **Revis?o das depend?ncias**: Todas as depend?ncias entre projetos foram revisadas e mantidas apenas as necess?rias.
- **Build limpo**: Todos os projetos compilam com sucesso.

## O que N?O foi implementado
- **Gera??o autom?tica das classes C# a partir dos XSDs**: O comando `xsd.exe` deve ser executado manualmente pelo desenvolvedor.
- **Integra??o das classes geradas dos XSDs ao dom?nio**: As classes ainda n?o foram integradas ao projeto.
- **Transmiss?o real e consulta de NF-e/NFC-e**: A camada de transmiss?o para PDV est? pronta por interface, mas o cliente real da SEFAZ depende da implementa??o de `IPdvSefazTransmissor` no seu ambiente.
- **Testes automatizados**: N?o h? testes automatizados implementados.

---

### Sem certificado (modo dev)

Se voce ainda nao possui certificado A1, pode habilitar o modo sem certificado para testar o fluxo no MAUI.

Em `NFe.Api\appsettings.Development.json`:

```json
"Certificado": {
  "PermitirSemCertificado": true
}
```

Neste modo:
- `/api/transmissao/enviar` e `/api/transmissao/assinar-e-enviar` respondem com sucesso simulado.
- O endpoint `/health` indica `modoSemCertificado=true`.

Para homologacao real, configure o certificado A1 e defina `PermitirSemCertificado=false`.
