using System;
using System.Collections.Generic;

namespace TibiaPixelBot.DataModels
{
    public partial class ConfiguracaoEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Dominio { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public bool? Ssl { get; set; }
        public string EmailSuporte { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Excluido { get; set; }
    }
}
