using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTTCommonParameters.Repository.Dto
{
    public class ResponseModel
    {
        public bool Result { get; set; } = true;
        public object Data { get; set; }
        public string ErrorMessge { get; set; } = "";
    }
}
