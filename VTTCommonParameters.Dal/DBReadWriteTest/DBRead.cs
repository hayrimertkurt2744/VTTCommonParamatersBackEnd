using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniExcelLibs;
using System.IO;

namespace VTTCommonParameters.Dal.DBReadWriteTest
{
    public class DBRead
    {
        public void ReadExcelData(string filePath)
        {
           // var rows= MiniExcel.Query(filePath).ToList().Skip(1);
           // var headers= rows.Take(0).ToList();
           //foreach (var header in headers)
           // {
           //     Console.WriteLine(header.A.ToString());
           // }

           // foreach (var row in rows)
           // {

           //     Console.WriteLine(row.B.ToString());
                

           // }
            var rows = MiniExcel.Query(filePath).ToList();
            foreach (var row in rows)
            {
                var rowDict = (IDictionary<string, object>)row;
                foreach (var kvp in rowDict)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
                Console.WriteLine(); // For separating rows in output
            }


        }

    }
}
