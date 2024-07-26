using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniExcelLibs;
using System.IO;
using System.Data;

namespace VTTCommonParameters.Dal.DBReadWriteTest
{
    public class DBRead
    {
        public void ReadExcelData(string filePath)
        {
            //_context.Projects.Add(new Project { Name = "Project1" });
            //_context.Pages.Add(new Page { ProjectId=1(LINQ?)), Name = "Page1" }); iterate all pages first
            
            var rows = MiniExcel.Query(filePath).ToList();
            int rowId = 0;
            foreach (var row in rows)
            {
                int paramId = 0;
                int orderId = 0;
                var rowDict = (IDictionary<string, object>)row;
                foreach (var kvp in rowDict)
                {   paramId++;
                    orderId++;
                    if (rowId==0)
                    {


                        Console.WriteLine($"OrderID:{orderId} ParamID:{paramId} Row:{rowId} Value:{kvp.Value} ");
                        //_contetext.Parameters.Add(new Parameter {Id will be created automatically. I hope it creates starting with 1 and ++, ColumnName = kvp.Key, OrderId = orderId, DefaultValue = kvp.Value.ToString() });
                    }
                    else
                    {
                        Console.WriteLine($"ParamID:{paramId} Row:{rowId} Value:{kvp.Value} ");
                        //_context.ParameterValues.Add(new ParameterValue { ParameterId = paramId, RowId = rowId, Value = kvp.Value.ToString() });
                    }
                    

                }
                Console.WriteLine(); // For separating rows in output
                rowId++;
            }

        }

    }
}
