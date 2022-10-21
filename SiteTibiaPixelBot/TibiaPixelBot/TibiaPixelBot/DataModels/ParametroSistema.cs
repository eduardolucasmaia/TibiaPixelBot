using System;
using System.Collections.Generic;

namespace TibiaPixelBot.DataModels
{
    public partial class ParametroSistema
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Excluido { get; set; }
    }
}
