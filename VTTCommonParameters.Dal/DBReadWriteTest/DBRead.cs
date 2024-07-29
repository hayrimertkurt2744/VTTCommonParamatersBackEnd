using MiniExcelLibs;
using System.Data;
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
            int pageId = 1;
            int pageParamId = 1;

            // Add Project

            Project project = new Project { Name = "Project1" };
            _context.Projects.Add(project);

            _context.SaveChanges();
            Console.WriteLine($"New Project:Project 1 ");

            // Get sheet names. Create pages based on these sheet names.
            var sheetNames = MiniExcel.GetSheetNames(filePath);
            //foreach (var sheetName in sheetNames)
            //{

            //    Console.WriteLine($"Project 1's New Page:{sheetName} ");

            //}
            ////_context.SaveChanges();

            foreach (var sheetName in sheetNames)
            {
                List<Parameter> pageParams = new List<Parameter>();
                Page page = new Page { ProjectId = project.Id, Name = sheetName };
                _context.Pages.Add(page);
                _context.SaveChanges();


                // Based on current pageId, get the rows of the current page.
                var rows = MiniExcel.Query(filePath, sheetName: sheetName).Where(x => !string.IsNullOrWhiteSpace(x.B)).ToList();
                int rowId = 0;
                int currentParamId = pageParamId;

                // Iterate over the rows and columns of the current page.
                foreach (var row in rows)
                {
                    int index = 0;
                    int orderId = 0;

                    var rowDict = (IDictionary<string, object>)row;
                    var keysToRemove = rowDict.Where(kv => kv.Value == null).Select(kv => kv.Key).ToList();

                    if (keysToRemove != null)
                    {
                        foreach (var key in keysToRemove)
                        {
                            rowDict.Remove(key);
                        }
                    }

                    foreach (var kvp in rowDict)
                    {
                        if (rowId == 0)
                        {
                            Parameter parameter = new Parameter
                            {
                                PageId = page.Id,
                                ColumnName = kvp.Value.ToString(),
                                OrderId = orderId,
                                IsUnique = false
                            };

                            pageParams.Add(parameter);
                            _context.Parameters.Add(parameter);
                            _context.SaveChanges();
                            // Remove the collected keys from the dictionary


                            orderId++;

                        }

                        else
                        {
                            var value = kvp.Value ?? "NULL";
                            var curerntPageParam = pageParams[index];
                            //if (curerntPageParam.Id==8)
                            //{

                            //}
                            ParameterValue parameterValue = new ParameterValue
                            {
                                ParameterId = curerntPageParam.Id,
                                Value = value.ToString(),
                                RowId = rowId
                            };


                            _context.ParameterValues.Add(parameterValue);
                            _context.SaveChanges();

                            Console.WriteLine($"PageId:{page.Id} ParameterId:{curerntPageParam.Id} Value:{value} RowId:{rowId} ");

                        }
                        if (index < pageParams.Count - 1)
                        {
                            index++;
                        }

                    }
                    rowId++;

                    Console.WriteLine(); // For separating rows in output

                }
            }

        }
    }

}
