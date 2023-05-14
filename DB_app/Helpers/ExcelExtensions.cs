using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Helpers;

public class ExcelExtensions
{

    public static bool ExportAsExcel<T>(List<string> headers, IEnumerable<T> data, string fullFileName)
    {
        try
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sample Sheet");
            worksheet.Cell("A1").InsertData(data);
            workbook.SaveAs(fullFileName);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
