using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class Hash
    {
        public string BaseHash;

        public int i0 = 0;
        public int i1 = 0;
        public int i3 = 0;
        public int i5 = 0;
        public int i7 = 0;
        public int i9 = 0;

        public void SetParametrs(int i0, int i1, int i3, int i5, int i7, int i9)
        {
            this.i0 = i0;
            this.i1 = i1;
            this.i3 = i3;
            this.i5 = i5;
            this.i7 = i7;
            this.i9 = i9;
        }
    }
}

