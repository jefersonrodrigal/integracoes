
# Projeto NF-e/NFC-e (.NET 10)

## Visão Geral
Este projeto implementa uma solução completa para emissão, assinatura, validação e transmissão de NF-e (modelo 55) e NFC-e (modelo 65) utilizando .NET 10, seguindo as melhores práticas fiscais e técnicas.

---


## O que foi implementado
- **Estrutura modular**: Separação em projetos para domínio, infraestrutura, assinatura, validação, serialização, transmissão e builders.
- **Enum de ambiente**: `Environment` para Homologação e Produção.
- **Carregamento de certificado digital**: Classe `CertificateLoader` para carregar certificados A1 (arquivo PFX) e do repositório do Windows (por subject name ou thumbprint).
- **Assinatura digital de XML**: Classe `NFeSigner` e helper `XmlSignatureHelper` para assinar XMLs fiscais com certificado digital.
- **Serialização XML**: Helper para serializar objetos para XML conforme layout da SEFAZ.
- **Preparação para integração com schemas oficiais**: Orientações e automação para uso dos XSDs oficiais da SEFAZ (NF-e 4.00).
- **Criação automática do wrapper XSD**: `NFe_Geracao.xsd` para facilitar a geração das classes.
- **Remoção do projeto de console**: O projeto `NFe.ConsoleApp` foi removido da solução.
- **Revisão das dependências**: Todas as dependências entre projetos foram revisadas e mantidas apenas as necessárias.
- **Build limpo**: Todos os projetos compilam com sucesso.

## O que NÃO foi implementado
- **Geração automática das classes C# a partir dos XSDs**: O comando `xsd.exe` deve ser executado manualmente pelo desenvolvedor.
- **Integração das classes geradas dos XSDs ao domínio**: As classes ainda não foram integradas ao projeto.
- **Transmissão e consulta de NF-e/NFC-e**: O módulo de transmissão está preparado, mas não implementado.
- **Testes automatizados**: Não há testes automatizados implementados.
- **Integração com webservices SEFAZ**: O projeto está pronto para integração, mas a comunicação ainda não foi implementada.

---

