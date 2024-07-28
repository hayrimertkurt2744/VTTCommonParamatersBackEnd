using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniExcelLibs;
using System.IO;
using System.Data;
using System.Reflection.Metadata;
using VTTCommonParameters.Dal.Entities.AppEntities;
using Parameter = VTTCommonParameters.Dal.Entities.AppEntities.Parameter;

namespace VTTCommonParameters.Dal.DBReadWriteTest
{
    public class DBRead
    {
        private readonly VTTCommonParametersContext _context;

        public DBRead()
        {
            _context = new VTTCommonParametersContext();
        }

        public void ReadExcelData(string filePath)
        {
            //_context.Projects.Add(new Project { Name = "Project1" });
            //_context.SaveChanges();
            //Page page = new Page { ProjectId=1,Name="Authorizations"};
            //_context.Pages.Add(page);
            //_context.SaveChanges();

            var rows = MiniExcel.Query(filePath).ToList();
            int rowId = 0;
            foreach (var row in rows)
            {
                int paramId = _context.Parameters.Min(x => x.Id)-1;
                int orderId = 0;
                var rowDict = (IDictionary<string, object>)row;
                foreach (var kvp in rowDict)
                {   paramId++;
                    orderId++;
                    if (rowId==0)
                    {
                        //This if should check whether db has the parameter recorded on the Parameters table.

                        Console.WriteLine($"OrderID:{orderId} ParamID:{paramId} Row:{rowId} Value:{kvp.Value} ");
                        //Parameter parameter = new Parameter { PageId=1, ColumnName = kvp.Value.ToString(), OrderId = orderId, DefaultValue = kvp.Value.ToString() };
                        //_context.Parameters.AddRange(parameter);
                        //_context.SaveChanges();
                    }
                    else
                    {
                        var value = kvp.Value ?? "NULL";

                        Console.WriteLine($"ParamID:{paramId} Row:{rowId} Value:{value} ");

                        ParameterValue parameterValue = new ParameterValue
                        {
                            ParameterId = paramId,
                            RowId = rowId,
                            Value = value.ToString()
                        };

                        _context.ParameterValues.AddRange(parameterValue);
                        _context.SaveChanges();
                    }

                }
                Console.WriteLine(); // For separating rows in output
                rowId++;   
            }
        }
    }
}
