using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;

namespace NFe.Builders.Examples;

public static class ZeusXmlExamples
{
    public static string GerarXmlNFeExemplo()
    {
        var documento = CriarDocumentoExemplo(ModeloDocumento.NFe, TipoImpressao.tiRetrato, ConsumidorFinal.cfNao, PresencaComprador.pcNao);
        var builder = new NFeBuilder();
        return builder.GerarXml(documento);
    }

    public static string GerarXmlNFCeExemplo()
    {
        var documento = CriarDocumentoExemplo(ModeloDocumento.NFCe, TipoImpressao.tiNFCe, ConsumidorFinal.cfConsumidorFinal, PresencaComprador.pcPresencial);
        var builder = new NFCeBuilder();
        return builder.GerarXml(documento);
    }

    private static NFe.Classes.NFe CriarDocumentoExemplo(
        ModeloDocumento modelo,
        TipoImpressao tipoImpressao,
        ConsumidorFinal consumidorFinal,
        PresencaComprador presencaComprador)
    {
        var nfe = new NFe.Classes.NFe
        {
            infNFe = new infNFe
            {
                versao = "4.00",
                Id = "NFe00000000000000000000000000000000000000000000",
                ide = new ide
                {
                    cUF = Estado.SP,
                    cNF = "12345678",
                    natOp = "VENDA DE MERCADORIA",
                    mod = modelo,
                    serie = 1,
                    nNF = 1,
                    dhEmi = DateTimeOffset.Now,
                    tpNF = TipoNFe.tnSaida,
                    idDest = DestinoOperacao.doInterna,
                    cMunFG = 3550308,
                    tpImp = tipoImpressao,
                    tpEmis = TipoEmissao.teNormal,
                    cDV = 0,
                    tpAmb = TipoAmbiente.Homologacao,
                    finNFe = FinalidadeNFe.fnNormal,
                    indFinal = consumidorFinal,
                    indPres = presencaComprador,
                    procEmi = ProcessoEmissao.peAplicativoContribuinte,
                    verProc = "1.0.0"
                },
                emit = new emit
                {
                    CNPJ = "12345678000195",
                    xNome = "EMITENTE TESTE LTDA",
                    xFant = "EMITENTE TESTE",
                    IE = "111111111111",
                    CRT = CRT.RegimeNormal,
                    enderEmit = new enderEmit
                    {
                        xLgr = "RUA TESTE",
                        nro = "100",
                        xBairro = "CENTRO",
                        cMun = 3550308,
                        xMun = "SAO PAULO",
                        UF = Estado.SP,
                        CEP = "01001000",
                        cPais = 1058,
                        xPais = "BRASIL",
                        fone = 1130000000
                    }
                },
                dest = new dest(VersaoServico.Versao400)
                {
                    CNPJ = "11222333000181",
                    xNome = "CLIENTE TESTE LTDA",
                    indIEDest = NFe.Classes.Informacoes.Destinatario.indIEDest.ContribuinteICMS,
                    IE = "222222222222",
                    enderDest = new enderDest
                    {
                        xLgr = "RUA DESTINO",
                        nro = "200",
                        xBairro = "CENTRO",
                        cMun = 3550308,
                        xMun = "SAO PAULO",
                        UF = "SP",
                        CEP = "01002000",
                        cPais = 1058,
                        xPais = "BRASIL",
                        fone = 1131000000
                    }
                },
                det =
                [
                    new det
                    {
                        nItem = 1,
                        prod = new prod
                        {
                            cProd = "001",
                            cEAN = "SEM GTIN",
                            xProd = "PRODUTO TESTE",
                            NCM = "22030000",
                            CFOP = 5102,
                            uCom = "UN",
                            qCom = 1m,
                            vUnCom = 100m,
                            vProd = 100m,
                            cEANTrib = "SEM GTIN",
                            uTrib = "UN",
                            qTrib = 1m,
                            vUnTrib = 100m,
                            indTot = IndicadorTotal.ValorDoItemCompoeTotalNF
                        },
                        imposto = new imposto
                        {
                            ICMS = new ICMS
                            {
                                TipoICMS = new ICMS00
                                {
                                    orig = OrigemMercadoria.OmNacional,
                                    CST = Csticms.Cst00,
                                    modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                                    vBC = 100m,
                                    pICMS = 18m,
                                    vICMS = 18m
                                }
                            },
                            PIS = new PIS
                            {
                                TipoPIS = new PISAliq
                                {
                                    CST = CSTPIS.pis01,
                                    vBC = 100m,
                                    pPIS = 1.65m,
                                    vPIS = 1.65m
                                }
                            },
                            COFINS = new COFINS
                            {
                                TipoCOFINS = new COFINSAliq
                                {
                                    CST = CSTCOFINS.cofins01,
                                    vBC = 100m,
                                    pCOFINS = 7.60m,
                                    vCOFINS = 7.60m
                                }
                            }
                        }
                    }
                ],
                total = new total
                {
                    ICMSTot = new ICMSTot
                    {
                        vBC = 100m,
                        vICMS = 18m,
                        vBCST = 0m,
                        vST = 0m,
                        vProd = 100m,
                        vFrete = 0m,
                        vSeg = 0m,
                        vDesc = 0m,
                        vII = 0m,
                        vIPI = 0m,
                        vPIS = 1.65m,
                        vCOFINS = 7.60m,
                        vOutro = 0m,
                        vNF = 109.25m
                    }
                },
                pag =
                [
                    new pag
                    {
                        detPag =
                        [
                            new detPag
                            {
                                tPag = FormaPagamento.fpDinheiro,
                                vPag = 109.25m
                            }
                        ]
                    }
                ]
            }
        };

        return nfe;
    }
}

