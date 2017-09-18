using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class PlayerDefesas
    {

        public long ID { get; set; }
        public Nullable<long> IdPlayer { get; set; }
        public string NomeOponente { get; set; }
        public string NomeGuilda { get; set; }
        public Nullable<int> Vitoria { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }

        public Player Player { get; set; }
    }
}
