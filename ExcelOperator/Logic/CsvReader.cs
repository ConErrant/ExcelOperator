using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using Microsoft.VisualBasic.FileIO;

namespace ExcelOperator.Logic
{
    public class CsvReader
    {
        public delegate Task CallbackDelegate(List<List<string>> rows);

        public List<string> ReadHeader(string path, int headerRow)
        {
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                parser.ReadLine();

                var fields = parser.ReadFields();

                if (fields == null)
                    throw new Exception("File is empty");

                return fields.ToList();
            }
        }
        public async Task ReadData(string path, int startRow, int batchsize, CallbackDelegate processCallback)
        {
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                var batch = new List<List<string>>();


                if (!parser.EndOfData)
                    parser.ReadLine();

                while (!parser.EndOfData)
                {
                    batch.Add(new List<string>(parser.ReadFields()));

                    if (batch.Count() == batchsize)
                    {
                        await processCallback(batch);
                        batch.Clear();
                        GC.Collect();
                    }
                }

                if (batch.Count() > 0)
                {
                    await processCallback(batch);
                    batch.Clear();
                    GC.Collect();
                }
            }
        }





        // CSV HELPER
        //public List<string> ReadHeader(string path, int headerRow)
        //{
        //    using (var reader = new StreamReader(path))
        //    using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        if (csv.Read())
        //        {
        //            var result = new List<string>();
        //            string value;
        //            for (int i = 0; csv.TryGetField<string>(i, out value); i++)
        //            {
        //                result.Add(value);
        //            }
        //            return result;
        //        }

        //        throw new NotImplementedException();
        //    }
        //}
        //public async Task ReadData(string path, int startRow, int batchsize, CallbackDelegate processCallback)
        //{
        //    using (var reader = new StreamReader(path))
        //    using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        //    {
        //        var batch = new List<List<string>>();

        //        if(!csv.Read())
        //        {
        //            throw new Exception("File is empty");
        //        }

        //        for(var row = 1; csv.Read(); row++)
        //        {
        //            if (row < startRow)
        //                continue;

        //            var result = new List<string>();
        //            string value;
        //            for (int i = 0; csv.TryGetField<string>(i, out value); i++)
        //            {
        //                result.Add(value);
        //            }

        //            batch.Add(result);

        //            if (batch.Count() == batchsize)
        //            {
        //                await processCallback(batch);
        //                batch.Clear();
        //                GC.Collect();
        //            }
        //        }

        //        if(batch.Count() > 0)
        //        {
        //            await processCallback(batch);
        //            batch.Clear();
        //            GC.Collect();
        //        }
        //    }
        //}
    }
}
