using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InfoLogBatalhas
    {
        public class GuildWarMatchLog
        {
            public long match_time { get; set; }
            public long log_type { get; set; }
            public long win_lose { get; set; }
            public long match_score_var { get; set; }
            public long opp_guild_id { get; set; }

            public long opp_guild_master_channel_uid { get; set; }
            public long opp_guild_level { get; set; }
            public string opp_guild_name { get; set; }
            public long match_end { get; set; }
        }
        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }

            public List<GuildWarMatchLog> match_log { get; set; }
        }
    }
}
