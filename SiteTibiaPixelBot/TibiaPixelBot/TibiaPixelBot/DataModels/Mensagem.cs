using System;
using System.Collections.Generic;

namespace TibiaPixelBot.DataModels
{
    public partial class Mensagem
    {
        public Mensagem()
        {
            VinculoUsuarioMensagem = new HashSet<VinculoUsuarioMensagem>();
        }

        public int Id { get; set; }
        public string Corpo { get; set; }
        public string Cabecalho { get; set; }
        public byte[] Imagem { get; set; }
        public bool Obrigatorio { get; set; }
        public DateTime DataCadastro { get; set; }

        public ICollection<VinculoUsuarioMensagem> VinculoUsuarioMensagem { get; set; }
    }
}
