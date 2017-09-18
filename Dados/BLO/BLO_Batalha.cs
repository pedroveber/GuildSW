using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;

namespace Dados.BLO
{
    public class BLO_Batalha
    {
        public static List<Batalhas> _SelectAll()
        {
            return DAO.DAO_Batalha._SelectAll();
        }

        public Batalhas Insert(Batalhas _obj)
        {

            Batalhas objRetorno = new Batalhas();
            objRetorno = DAO.DAO_Batalha._SelectByIdDate(_obj);

            if (objRetorno == null || objRetorno.ID <= 0)
                return DAO.DAO_Batalha.Insert(_obj);

            else
            {
                _obj.ID = objRetorno.ID;
                DAO.DAO_Batalha.AtualizarData(_obj);
            }

            return _obj;
        }

        public Batalhas SelectByID(Batalhas _obj)
        {
            return DAO.DAO_Batalha._SelectByID(_obj);
        }

        public Batalhas SelectByIdDate(Batalhas _obj)
        {
            return DAO.DAO_Batalha._SelectByIdDate(_obj);
        }
    }
}
