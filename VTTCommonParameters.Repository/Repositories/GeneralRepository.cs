using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTTCommonParameters.Dal;
using VTTCommonParameters.Dal.Entities;

namespace VTTCommonParameters.Repository.Repositories
{
    public class GeneralRepository
    {
        private readonly VTTCommonParametersContext _context;

        public GeneralRepository()
        {
            _context = new VTTCommonParametersContext();
        }


        public DataTable GetPageValuesById(int pageId, int skip, int take)
        {
            var Parameters = _context.Parameters
                .Where(cp => cp.PageId == pageId)
                .OrderBy(cp => cp.OrderId)
                .ToList();

            var ParameterValues = _context.ParameterValues
                .Where(cpv => Parameters.Select(cp => cp.Id).Contains(cpv.ParameterId))
                .ToList();

            var dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            foreach (var param in Parameters)
            {
                dataTable.Columns.Add(param.ColumnName, typeof(string));
            }

            var groupedValues = ParameterValues
                .GroupBy(cpv => cpv.RowId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var paginatedValues = groupedValues
                .Skip(skip)
                .Take(take);

            foreach (var group in paginatedValues)
            {
                var row = dataTable.NewRow();
                row["Id"] = group.Key;

                foreach (var param in Parameters)
                {
                    var value = group.Value.FirstOrDefault(v => v.ParameterId == param.Id)?.Value;
                    row[param.ColumnName] = value ?? param.DefaultValue;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public int GetTotalCount(int pageId)
        {
            var Parameters = _context.Parameters
                .Where(cp => cp.PageId == pageId)
                .OrderBy(cp => cp.OrderId)
                .ToList();

            var ParameterValues = _context.ParameterValues
                .Where(cpv => Parameters.Select(cp => cp.Id).Contains(cpv.ParameterId))
                .ToList();

            var totalCount = ParameterValues
                .GroupBy(cpv => cpv.RowId)
                .Count();

            return totalCount;
        }
        public DataTable GetParameterIDs(int pageId)
        {
            //var parameterValues = _context.ParameterValues.ToList();
            var parameters = _context.Parameters
                .Where(x=>x.PageId == pageId)
                .OrderBy(x=>x.OrderId)
                .ToList();

            var dataTable = new DataTable();

            dataTable.Columns.Add("ParameterId", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));

            foreach (var parameter in parameters)
            {
                var row = dataTable.NewRow();

                row["ParameterId"] = parameter.Id;
                row["Name"] = parameters.FirstOrDefault(a => a.Id == parameter.Id)?.ColumnName;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public DataTable GetAllPages()
        {
            var pages = _context.Pages.ToList();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string)); // Add more columns as needed

            foreach (var page in pages)
            {
                var row = dataTable.NewRow();
                row["Id"] = page.Id;
                row["Name"] = page.Name; // Populate other columns as needed
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public void AddData(List<ParameterValue> parameterValues)
        {
            int rowId = _context.ParameterValues.Max(x => x.RowId) +1;
            parameterValues.ForEach(x => x.RowId = rowId);

            _context.ParameterValues.AddRange(parameterValues);
            _context.SaveChanges();
        }   
        public void UpdateData(int rowId, int pageId, List<ParameterValue> parameterValues)
        {
            if (parameterValues == null || !parameterValues.Any())
            {
                // If no parameter values are provided, there's nothing to update.
                return;
            }

            foreach (var paramValue in parameterValues)
            {
                var existingParamValue = _context.ParameterValues
                                                 .FirstOrDefault(x => x.RowId == rowId && x.Parameter.PageId == pageId && x.ParameterId == paramValue.ParameterId);

                if (existingParamValue != null)
                {
                    existingParamValue.Value = paramValue.Value;
                }
            }

            _context.SaveChanges();
        }
        public void RemoveData(int rowId, int pageId)
        {
            
            var deletedParameterValues= _context.ParameterValues.Where(x => x.RowId == rowId)
                                                                .Where(x => x.Parameter.PageId == pageId);
            if (deletedParameterValues != null)
            {

                foreach (var deletedParameterValue in deletedParameterValues)
                {
                    _context.ParameterValues.Remove(deletedParameterValue);
                }
                
                _context.SaveChanges();
            }
        }
    }
}

