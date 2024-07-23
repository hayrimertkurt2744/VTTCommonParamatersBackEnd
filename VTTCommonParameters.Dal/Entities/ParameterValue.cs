using System.ComponentModel.DataAnnotations.Schema;

namespace VTTCommonParameters.Dal.Entities
{
    public class ParameterValue
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
        public int RowId { get; set; }

        [ForeignKey("ParameterId")]
        public Parameter? Parameter { get; set; }
    }
}
