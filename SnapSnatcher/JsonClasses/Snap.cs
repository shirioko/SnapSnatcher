using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher.JsonClasses
{
    public class Snap
    {
        [PrimaryKey, AutoIncrement]
        public int entity_id { get; set; }

        public string rp { get; set; }

        public string sn { get; set; }

        public string c_id { get; set; }

        [Indexed(Unique = true)]
        public string id { get; set; }

        public int st { get; set; }

        public int m { get; set; }

        public long ts { get; set; }

        public long sts { get; set; }
    }
}
