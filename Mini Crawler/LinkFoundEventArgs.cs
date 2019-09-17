using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Crawler
{
    // Create an event to deal with found links specifically.
    class LinkFoundEventArgs : EventArgs
    {
        public readonly string Link;

        public LinkFoundEventArgs(string link)
        {
            Link = link;
        }
    }
}
