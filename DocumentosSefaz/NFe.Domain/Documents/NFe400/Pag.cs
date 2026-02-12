using System.Collections.Generic;

namespace NFe.Domain.Models;

public class Pag
{
    public List<DetPag> detPag { get; set; }
}

public class DetPag
{
    public string tPag { get; set; }
    public decimal vPag { get; set; }
}
