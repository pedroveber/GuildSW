using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
   public class PlayerStatus
    {
        public long ID { get; set; }
        public Nullable<long> IdPlayer { get; set; }
        public Nullable<long> IdBatalha { get; set; }
        public string Status { get; set; }

        public virtual Batalhas Batalhas { get; set; }
        public virtual Player Player { get; set; }
    }
}
