using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mbrace.stockticker
{
    public class StockDataInfo
    {
        private string data;

        public string Data
        {
            get { return data; }
        }

        public StockDataInfo(string data)
        {
            this.data = data;
        }

    }
}
