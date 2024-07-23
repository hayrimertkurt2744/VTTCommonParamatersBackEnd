using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTTCommonParameters.Dal.Entities.AccountEntities
{
    internal class RefreshToken
    {
        //You can add Id here to relate tokens to a particular user.
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; } 
    }
}
