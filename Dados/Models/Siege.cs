using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class Siege
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
    }

    public class SiegeGuilda
    {

        public long IdSiege { get; set; }
        public long IdGuilda { get; set; }
        public int Posicao { get; set; }
    }

    public class SiegePlayer
    {
        public long IdSiege { get; set; }
        public long IdPlayer { get; set; }
        public int UsedUnits { get; set; }
    }
    public class SiegePlayerOponente
    {
        public long Id { get; set; }
        public long IdSiege { get; set; }
        public long IdGuild { get; set; }
        public long IdPlayer { get; set; }
        public string Nome { get; set; }
    }

    public class SiegeDefenseDeck
    {
        public long Id { get; set; }
        public long IdDeck { get; set; }
        public long IdSiege { get; set; }
        public long IdPlayer { get; set; }
        public long IdGuild { get; set; }

    }

    public class SiegeAtaque
    {
        public long Id { get; set; }
        public long IdSiege { get; set; }
        public long IdPlayer { get; set; }
        public long IdPlayerOponente { get; set; }
        public int Vitoria { get; set; }
        public DateTime Data { get; set; }
    }

    public class SiegePlayerDefesa
    {
        public long Id { get; set; }
        public long IdDeck { get; set; }
        public long IdPlayerOponente { get; set; }
        public int Vitoria { get; set; }
        public DateTime Date { get; set; }

    }

    public class SiegeTimeDefesa
    {
        public long IdDeck { get; set; }
        public int Base { get; set; }
        public long Monstro1 { get; set; }
        public long Monstro2 { get; set; }
        public long Monstro3 { get; set; }
    }


}


