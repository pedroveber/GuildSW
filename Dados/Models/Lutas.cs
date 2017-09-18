using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class Lutas
    {
        public long ID { get; set; }
        public Nullable<long> CodBatalhas { get; set; }
        public Nullable<long> CodPlayer { get; set; }
        public Nullable<long> CodPlayerOponente { get; set; }
        public Nullable<int> Vitoria { get; set; }
        public Nullable<long> ValorBarra { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }
        public string MomentoVitoria { get; set; }

        public virtual Batalhas Batalhas { get; set; }
        public virtual Player Player { get; set; }
        public virtual PlayerOponente PlayerOponente { get; set; }
    }
}
