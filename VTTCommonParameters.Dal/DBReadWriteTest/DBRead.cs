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
using System.Security;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            int pageId = 1;
            int pageParamId = 1;

            // Add Project
            
            Project project=new Project { Name = "Project1" };
            _context.Projects.Add(project);
            _context.SaveChanges();
            Console.WriteLine($"New Project:Project 1 ");

            var sheetNames = MiniExcel.GetSheetNames(filePath);


            foreach (var sheetName in sheetNames)
            {
                
                Page page = new Page { ProjectId = project.Id, Name = sheetName};
                _context.Pages.Add(page);
                _context.SaveChanges();
                
                var rows = MiniExcel.Query(filePath, sheetName: sheetName).Where(x=>!string.IsNullOrWhiteSpace( x.B)).ToList();
                int currentParamId = pageParamId;
                int orderId = 1;
                foreach (var column in rows[0])
                {
                    Parameter parameter = new Parameter
                    {
                        PageId = page.Id,
                        ColumnName = column.ToString(),
                        OrderId = orderId++,
                        IsUnique = false
                    };
                    _context.Parameters.Add(parameter);
                    foreach (var row in rows.Skip(1))
                    {
                        var columnValue = row[column];
                        Console.WriteLine($"Column: {column}, Value: {columnValue}");
                        // Perform actions with columnValue if needed
                    }


                }
 
               
            }
        }


    }
}
