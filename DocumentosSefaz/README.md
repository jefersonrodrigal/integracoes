# Projeto NF-e/NFC-e (.NET 10)

## Visão Geral
Este projeto implementa uma solução completa para emissão, assinatura, validação e transmissão de NF-e (modelo 55) e NFC-e (modelo 65) utilizando .NET 10, seguindo as melhores práticas fiscais e técnicas.

---

## Integração com PDV

O projeto agora possui uma camada dedicada `NFe.PdvIntegration`, pronta para ser consumida por API/ERP/PDV.

### Recursos para PDV
- Contratos de entrada/saída (`PdvEmissaoRequest` / `PdvEmissaoResponse`)
- Serviço orquestrador único (`IPdvNFeService`)
- Geração de XML de NF-e e NFC-e com Zeus
- Assinatura digital opcional (com `IXmlSignatureService`)
- Transmissão opcional para SEFAZ via interface plugável (`IPdvSefazTransmissor`)
- Correlação de requisição (`CorrelationId`) para rastreabilidade no PDV

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

// Opcional: transmissor simulado para homologação de integração com PDV
services.AddSingleton<IPdvSefazTransmissor, FakeSefazTransmissor>();
```

### Exemplo de emissão consumida pelo PDV

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

## Exemplo rápido com Zeus.Net.NFe.NFCe

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

### Usando seus próprios objetos Zeus

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
- **Estrutura modular**: Separação em projetos para domínio, infraestrutura, assinatura, validação, serialização, transmissão e builders.
- **Camada PDV**: Novo projeto `NFe.PdvIntegration` com contratos, serviço de emissão e extensão de DI.
- **Enum de ambiente**: `Environment` para Homologação e Produção.
- **Carregamento de certificado digital**: Classe `CertificateLoader` para carregar certificados A1 (arquivo PFX) e do repositório do Windows (por subject name ou thumbprint).
- **Assinatura digital de XML**: Classe `NFeSigner` e helper `XmlSignatureHelper` para assinar XMLs fiscais com certificado digital.
- **Serialização XML**: Helper para serializar objetos para XML conforme layout da SEFAZ.
- **Integração Zeus.Net.NFe.NFCe**: Implementação para serializar objetos Zeus em XML de NF-e e NFC-e.
- **Preparação para integração com schemas oficiais**: Orientações e automação para uso dos XSDs oficiais da SEFAZ (NF-e 4.00).
- **Criação automática do wrapper XSD**: `NFe_Geracao.xsd` para facilitar a geração das classes.
- **Remoção do projeto de console**: O projeto `NFe.ConsoleApp` foi removido da solução.
- **Revisão das dependências**: Todas as dependências entre projetos foram revisadas e mantidas apenas as necessárias.
- **Build limpo**: Todos os projetos compilam com sucesso.

## O que NÃO foi implementado
- **Geração automática das classes C# a partir dos XSDs**: O comando `xsd.exe` deve ser executado manualmente pelo desenvolvedor.
- **Integração das classes geradas dos XSDs ao domínio**: As classes ainda não foram integradas ao projeto.
- **Transmissão real e consulta de NF-e/NFC-e**: A camada de transmissão para PDV está pronta por interface, mas o cliente real da SEFAZ depende da implementação de `IPdvSefazTransmissor` no seu ambiente.
- **Testes automatizados**: Não há testes automatizados implementados.

---
