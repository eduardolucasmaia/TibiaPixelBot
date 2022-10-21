using System;
using System.Collections.Generic;

namespace TibiaPixelBot.DataModels
{
    public partial class Usuario
    {
        public Usuario()
        {
            VinculoUsuarioMensagem = new HashSet<VinculoUsuarioMensagem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool UsouFreeTrial { get; set; }
        public DateTime? DataExpirar { get; set; }
        public string CodigoMaquina { get; set; }
        public DateTime? DataUltimoAcesso { get; set; }
        public bool Logado { get; set; }
        public string ChaveRecuperacao { get; set; }
        public string NomePlayerTibia { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Excluido { get; set; }

        public ICollection<VinculoUsuarioMensagem> VinculoUsuarioMensagem { get; set; }
    }
}
