using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mbrace.stockticker
{
    public  class StockDataRow
    {
        public float AdjClose { get; set; }

        public int Volume { get; set; }

        public float Close { get; set; }

        public float Low { get; set; }

        public float High { get; set; }

        public float Open { get; set; }

        public DateTime Date { get; set; }
    }
}
