using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class Batalhas
    {
        public Batalhas()
        {
            Lutas = new List<Models.Lutas>();
            PlayerOponente = new List<Models.PlayerOponente>();
            PlayerStatus = new List<Models.PlayerStatus>();
        }

        public long ID { get; set; }
        public string Guilda { get; set; }
        public Nullable<long> Life { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public Nullable<long> PontuacaoOponente { get; set; }
        public Nullable<long> PontuacaoGuild { get; set; }
        public Nullable<long> RankGuild { get; set; }
        public Nullable<long> idGuilda { get; set; }
        public Nullable<long> idGuildaAtacante { get; set; }

        public List<Lutas> Lutas { get; set; }
        public List<PlayerOponente> PlayerOponente { get; set; }
        public List<PlayerStatus> PlayerStatus { get; set; }
    }
}
