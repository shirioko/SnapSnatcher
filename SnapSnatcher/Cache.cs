using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher
{
    public class Cache
    {
        protected Dictionary<string, string> dict = new Dictionary<string, string>();
        public string Get(string key)
        {
            if(this.dict.ContainsKey(key))
            {
                return this.dict[key];
            }
            return string.Empty;
        }

        public void Set(string key, string value)
        {
            this.dict.Add(key, value);
        }
    }
}
