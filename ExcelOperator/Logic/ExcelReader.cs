using XlsxHelper;

//https://stackoverflow.com/questions/31515776/how-can-i-catch-uniquekey-violation-exceptions-with-ef6-and-sql-server
//https://dotnetcoretutorials.com/better-exception-handling-with-entityframeworkcore-exceptions/

namespace ExcelOperator.Logic
{
    public class ExcelReader
    {
        public delegate Task CallbackDelegate(List<List<string>> rows);

        public List<string> ReadHeader(string path, int headerRow)
        {
            using (var workbook = XlsxReader.OpenWorkbook("C:\\Users\\chris\\Downloads\\Livre.xlsx"))
            {
                var worksheet = workbook.Worksheets.First();
                foreach (var row in worksheet.WorksheetReader)
                {
                    if(row.RowNumber == headerRow)
                    {
                        return row.Cells.Select(x => x.CellValue.ToString()).ToList();
                    }
                }

                throw new NotImplementedException();
            }
        }
        public async Task ReadData(string path, int startRow, int batchsize, CallbackDelegate processCallback)
        {
            using (var workbook = XlsxReader.OpenWorkbook("C:\\Users\\chris\\Downloads\\Livre.xlsx"))
            {
                var worksheet = workbook.Worksheets.First();
                var batch = new List<List<string>>();
                foreach (var row in worksheet.WorksheetReader)
                {
                    if (row.RowNumber < startRow)
                        continue;

                    batch.Add(row.Cells.Select(x => x.CellValue.ToString()).ToList());

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
    }
}
