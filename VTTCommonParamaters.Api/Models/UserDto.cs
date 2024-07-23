using System.ComponentModel.DataAnnotations;

namespace VTTCommonParamaters.Api.Models
{
    public class UserDto
    {
        //[Required]
        public required string Password { get; set; } = string.Empty;

        [EmailAddress]
        //[Required]
        public required string Email { get; set; }=string.Empty;
        
      
    }
}
