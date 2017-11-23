using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class InfoSiegeDefense
    {
        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }
            public List<WizardInfoList> wizard_info_list { get; set; }
            public List<DefenseDeckList> defense_deck_list { get; set; }

            public List<DefenseUnitList> defense_unit_list { get; set; }
        }
        public class WizardInfoList
        {
            public long wizard_id { get; set; }
            public string wizard_name { get; set; }
            public long guild_id { get; set; }
        }

        public class DefenseDeckList
        {
            public long deck_id { get; set; }
            public long wizard_id { get; set; }

        }

        public class DefenseUnitList
        {
            public long deck_id { get; set; }
            public int pos_id { get; set; }
            public UnitInfo unit_info { get; set; }
        }

        public class UnitInfo
        {
            public long unit_master_id { get; set; }

        }
    }
}
