using System.ComponentModel.DataAnnotations.Schema;

namespace VTTCommonParameters.Dal.Entities.AppEntities
{
    public class Page
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
