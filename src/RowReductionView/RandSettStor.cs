using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowReductionView
{
    public sealed class RandSettStor
    {
        private static readonly RandSettStor _mInstance = new RandSettStor();

        public static int MinValue { get; set;}
        public static int MaxValue { get; set; }
        public static bool IsDoubleValue { get; set; }
        public static int RatioOfZero { get; set; }
        public static int MinDimension { get; set; }
        public static int MaxDimension { get; set; }
        
        static RandSettStor()
        {
            // Default

            MinValue = -10;
            MaxValue = 10;
            IsDoubleValue = false;
            RatioOfZero = 20;
            MinDimension = 1;
            MaxDimension = 9;
        }

        public static RandSettStor Instance { get { return _mInstance; } }
    }

    
}
