using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matchGame
{
    internal class CsvHandler<T>
    {
        private readonly string _fileName;

        public CsvHandler(string fileName)
        {
            _fileName = fileName;
        }

        public List<T> ReadData()
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
        
        public void WriteData(List<T> data)
        {
            using (var writer = new StreamWriter(_fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }
    }
}
