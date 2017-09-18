using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
   public class Player
    {
        public Player()
        {
            this.Lutas = new HashSet<Lutas>();
            this.PlayerStatus = new HashSet<PlayerStatus>();
            this.PlayerDefesas = new HashSet<PlayerDefesas>();
        }

        public long ID { get; set; }
        public string Nome { get; set; }
        public Nullable<long> Level { get; set; }
        public Nullable<long> PontoArena { get; set; }
        public string Status { get; set; }
        public string Imagem { get; set; }

        public virtual ICollection<Lutas> Lutas { get; set; }
        public virtual ICollection<PlayerStatus> PlayerStatus { get; set; }
        public virtual ICollection<PlayerDefesas> PlayerDefesas { get; set; }
    }
}
