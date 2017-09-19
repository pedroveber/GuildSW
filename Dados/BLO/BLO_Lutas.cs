using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;

namespace Dados.BLO
{
    public class BLO_Lutas
    {
        public Lutas Insert(Lutas _obj)
        {
            if (DAO.DAO_Lutas._SelectByPlayer_Oponente(_obj) == null)
                return DAO.DAO_Lutas.Insert(_obj);
            else return DAO.DAO_Lutas._SelectByPlayer_Oponente(_obj);
        }

        public static List<Lutas> _SelectAllByBatalha(Batalhas _obj)
        {
            return DAO.DAO_Lutas._SelectAllByBatalha(_obj);
        }
    }
}
