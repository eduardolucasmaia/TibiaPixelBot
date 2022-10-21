using System;
using System.Collections.Generic;

namespace TibiaPixelBot.DataModels
{
    public partial class VinculoUsuarioMensagem
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdMensagem { get; set; }
        public bool JaRecebeu { get; set; }
        public bool Excluido { get; set; }

        public Mensagem IdMensagemNavigation { get; set; }
        public Usuario IdUsuarioNavigation { get; set; }
    }
}
