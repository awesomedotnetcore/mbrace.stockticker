using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mbrace.stockticker
{
    public class StockDataConverter
    {
        public async Task<string> Run(string symbol)
        {
            var dataInfo = await Get(symbol);
            var rows = GetRows(dataInfo);
            var stringified = GetIndividualJson(rows);
            return stringified;
        }

        private string GetIndividualJson(StockDataRow[] rows)
        {
            var sbOutput = new StringBuilder();
            foreach (var item in rows)
            {
                sbOutput.AppendLine(JObject.FromObject(item).ToString(Formatting.None));
            }
            return sbOutput.ToString();
        }

        public async Task<StockDataInfo> Get(string symbol)
        {
            using (var webClient = new WebClient())
            {
                var data = await webClient.DownloadStringTaskAsync(string.Format("http://real-chart.finance.yahoo.com/table.csv?s={0}&d=3&e=13&f=2015&g=d&a=7&b=9&c=1996&ignore=.csv", symbol));
                return new StockDataInfo(data);
            }
        }

        public StockDataRow[] GetRows(StockDataInfo yahooFinance)
        {
            var csv = yahooFinance.Data;
            List<StockDataRow> rows = new List<StockDataRow>();
            var csvDocument = csv.Split('\n');
            foreach (var csvRow in csvDocument)
            {
                if (!IsHeaderRow(csvRow))
                    rows.Add(ConvertToJson(csvRow));
            }
            return rows.ToArray();
        }

        public StockDataRow ConvertToJson(string csv)
        {
            if (!IsHeaderRow(csv))
            {
                try
                {
                    var values = csv.Split(',');
                    var rowData = new StockDataRow();
                    rowData.Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    rowData.Open = float.Parse(values[1]);
                    rowData.High = float.Parse(values[2]);
                    rowData.Low = float.Parse(values[3]);
                    rowData.Close = float.Parse(values[4]);
                    rowData.Volume = int.Parse(values[5]);
                    rowData.AdjClose = float.Parse(values[6]);

                    return rowData;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        private static bool IsHeaderRow(string csv)
        {
            return string.IsNullOrEmpty(csv) || csv.StartsWith("Date");
        }
    }
}
