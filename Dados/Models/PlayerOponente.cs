using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class PlayerOponente
    {
        public PlayerOponente()
        {
            this.Lutas = new List<Models.Lutas>();
        }

        public long ID { get; set; }
        public Nullable<long> CodGuilda { get; set; }
        public string Nome { get; set; }
        public Nullable<int> Bonus { get; set; }

        public virtual Batalhas Batalhas { get; set; }
        public List<Lutas> Lutas { get; set; }
    }
}
