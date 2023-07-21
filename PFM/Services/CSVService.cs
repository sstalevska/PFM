using CsvHelper;
using CsvHelper.Configuration;
using PFM.Models;
using System.Globalization;

namespace PFM.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", "")
            };
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);
            var transactions = csv.GetRecords<T>();
            return transactions;
            
        }
    }
}
