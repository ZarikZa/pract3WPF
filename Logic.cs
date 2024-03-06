using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material_design
{
    internal class Logic
    {
        public string[] Suffle(string[] suffled)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            suffled = suffled.OrderBy(x => rnd.Next()).ToArray();
            return suffled;
        }
        public List<FileInfo> SuffleForList(List<FileInfo> suffled)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            suffled = suffled.OrderBy(x => rnd.Next()).ToList();
            return suffled;
        }
    }
}
