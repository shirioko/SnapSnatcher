using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher
{
    class AppSettings
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [Indexed(Unique = true)]
        public string key { get; set; }

        public string value { get; set; }
    }
}
