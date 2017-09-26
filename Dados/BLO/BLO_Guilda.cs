using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;

namespace Dados.BLO
{
    public class BLO_Guilda
    {
        public Guilda Insert(Guilda _obj)
        {
            if (DAO.DAO_Guilda._SelectByID(_obj) == null)
                return DAO.DAO_Guilda.Insert(_obj);
            else return DAO.DAO_Guilda.UpDate(_obj);
        }
    }
}
