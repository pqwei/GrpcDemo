using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcDemo.Server.Models
{
    public class ResponseModel
    {
        public string Code { get; set; }
        public IEnumerable<DataObject> DataList { get; set; }
    }
    public class DataObject
    {
        public DateTime? FDate { get; set; }
        public double? Value { get; set; }
        public string ErrorMsg { get; set; }
    }
}
