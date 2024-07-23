using System.ComponentModel.DataAnnotations.Schema;

namespace VTTCommonParameters.Dal.Entities.AppEntities
{
    public class Parameter
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string ColumnName { get; set; }
        public int OrderId { get; set; }
        public string Type { get; set; }
        public string? DefaultValue { get; set; }

        [ForeignKey("PageId")]
        public Page Page { get; set; }
    }
}
