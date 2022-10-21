using System.Collections.Generic;

namespace TibiaPixelBot.Models
{
    public class ObjetoRetornoAplicativo
    {
        public bool Valido { get; set; }
        public string Mensagem { get; set; }
        public List<string> Mensagens { get; set; }
    }
}
