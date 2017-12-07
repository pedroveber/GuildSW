using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
   public class InfoSiegeBattleLog
    {
        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }

            public List<LogList> log_list { get; set; }

        }

        public class LogList
        {
            public List<GuildInfoList> guild_info_list { get; set; }
            public List<BattleLogList> battle_log_list { get; set; }

        }

        public class GuildInfoList
        {
            public long siege_id { get; set; }
            public long guild_id { get; set; }
            public int pos_id { get; set; }
            public string guild_name { get; set; }
            
        }
        public class BattleLogList
        {
            public int log_type { get; set; }
            public long siege_id { get; set; }
            public int base_number { get; set; }
            public long guild_id { get; set; }
            public string guild_name { get; set; }
            public long wizard_id { get; set; }
            public long opp_guild_id { get; set; }
            public long opp_wizard_id { get; set; }
            public string opp_wizard_name { get; set; }
            public int win_lose { get; set; }
            public long log_timestamp { get; set; }
        }
    }
}
