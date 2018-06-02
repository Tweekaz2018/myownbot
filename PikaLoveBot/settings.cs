using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PikaLoveBot
{
    public class stats
    {
        public int users;
        public int acconts;
        public int req;
        public int towns;
        public stats(int u, int acs, int r, int t)
        {
            users = u;
            acconts =acs;
            req = r;
            towns = t;
        }
    }
}
