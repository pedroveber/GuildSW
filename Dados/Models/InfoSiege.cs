using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class InfoSiege
    {
        public class match_info
        {
            public long siege_id { get; set; }
            public long match_start_time { get; set; }

        }

        public class GuildList
        {
            public long siege_id { get; set; }
            public long guild_id { get; set; }
            public int pos_id { get; set; }
            public string guild_name { get; set; }
        }
        public class BaseList
        {
            public long siege_id { get; set; }
            public long guild_id { get; set; }
            public int base_number { get; set; }
            public int base_type { get; set; }
            public int base_status { get; set; }
        }

        public class WizardInfoList
        {
            public long wizard_id { get; set; }
            public string wizard_name { get; set; }
            public long guild_id { get; set; }
        }

        public class DefenseDeckAssignList
        {
            public int base_number { get; set; }
            public long deck_id { get; set; }
        }

        public class DefenseDeckList
        {
            public long deck_id { get; set; }
            public long wizard_id { get; set; }

        }

        public class DefenseDeckStatusList
        {
            public long siege_id { get; set; }
            public int base_number { get; set; }
            public long defense_guild_id { get; set; }

            public long deck_id { get; set; }

            public int status { get; set; }

            public long attack_guild_id { get; set; }
            public long attack_wizard_id { get; set; }

            public long battle_start_time { get; set; }


        }

        public class UsedUnitCountList
        {
            public long wizard_id { get; set; }
            public int used_unit_count { get; set; }
        }


        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }
            public match_info match_info { get; set; }

            public List<GuildList> guild_list { get; set; }

            public List<BaseList> base_list { get; set; }

            public List<WizardInfoList> wizard_info_list { get; set; }

            public List<DefenseDeckList> defense_deck_list { get; set; }

            public List<DefenseDeckAssignList> defense_deck_assign_list { get; set; }
            public List<DefenseDeckStatusList> defense_deck_status_list { get; set; }
            public List<UsedUnitCountList> used_unit_count_list { get; set; }

        }

    }
}
