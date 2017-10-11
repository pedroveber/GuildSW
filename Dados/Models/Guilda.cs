using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class Guilda
    {
        public long Id { get; set; }
        public string Nome { get; set; }
    }

    public class GuildaPlayer
    {
        public long IdGuilda { get; set; }
        public string IdUsuario { get; set; }
        public long IdPlayer { get; set; }
        public bool Ativo { get; set; }
    }
}
