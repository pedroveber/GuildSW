using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.BLO
{
    public class BLO_Siege
    {
        public   Models.Siege InserirSiege(Models.Siege obj)
        {
            return new DAO.DAO_Siege().InserirSiege(obj);
        }

        public Models.SiegeGuilda InserirSiegeGuilda(Models.SiegeGuilda obj)
        {
            return new DAO.DAO_Siege().InserirSiegeGuilda(obj);
        }

        public void InserirSiegePlayer(Models.SiegePlayer obj)
        {
            new DAO.DAO_SiegePlayer().InserirSiegePlayer(obj);
        }

        public Models.SiegePlayerOponente InserirSiegePlayerOponente(Models.SiegePlayerOponente obj)
        {
            return new DAO.DAO_SiegePlayer().InserirSiegePlayerOponente(obj);
        }

        public void InsertDefenseDeck(Models.SiegeDefenseDeck obj)
        {
            new DAO.DAO_SiegeDefense().InsertDefenseDeck(obj);
        }

        public void InsertSiegeAtaque(Models.SiegeAtaque obj)
        {
            new DAO.DAO_SiegeAtaque().InserirSiegeAtaque(obj);
        }

        public void InserirSiegePlayerDefense(Models.SiegePlayerDefesa obj)
        {
            new DAO.DAO_SiegeDefense().InserirSiegePlayerDefense(obj);
        }

        public void InserirSiegeTimeDefesa(Models.SiegeTimeDefesa obj)
        {
            new DAO.DAO_SiegeDefense().InserirSiegeTimeDefesa(obj);
        }

        public List<Models.SiegeDefenseDeck> ListarDefenseDecks(long idSiege)
        {
            return new DAO.DAO_SiegeDefense().ListarDefenseDecks(idSiege);
        }

        public List<Models.SiegePlayerOponente> ListarPlayersOponentesSiege(long idSiege)
        {
            return new DAO.DAO_SiegePlayer().ListarPlayersOponentesSiege(idSiege);
        }

        public void InserirSiegeDefenseDeckAssign(Models.SiegeDefenseDeckAssign obj)
        {
            new DAO.DAO_SiegeDefense().InserirSiegeDefenseDeckAssign(obj);
        }

    }
}
