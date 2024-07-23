using System.ComponentModel.DataAnnotations;

namespace VTTCommonParamaters.Api.Models
{
    public class UserDto
    {
        public required string Password { get; set; } = string.Empty;

        [EmailAddress]
        public required string Email { get; set; }=string.Empty;
        
      
    }
}
