using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2Ex.Utility
{
    internal class LoadingDot
    {
        private bool go;
        private uint count;

        public LoadingDot()
        {
            go = true;
            Task.Run(() =>
            {
                while (go)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                    count++;
                }
            });
        }

        public void Stop()
        {
            go = false;
            Console.Write($" ({count}s)");
        }
    }
}
