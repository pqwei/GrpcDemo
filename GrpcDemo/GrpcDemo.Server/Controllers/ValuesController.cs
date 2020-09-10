using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcDemo.Server.Common;
using GrpcDemo.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GrpcDemo.Server.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ResponseModel> GetA()
        {
            //var str = System.IO.File.ReadAllText("D:\\data.txt");
            //var v1 = ClientUtil.ClientPost("http://localhost:56956/api/EDBData/JFlatTable", str);
            //var v2 = ClientUtil.ClientPost("http://localhost:56956/api/EDBData/JFlatTableTwo", str);
            //var result = JsonConvert.DeserializeObject<List<ResponseModel>>(str);
            var str = System.IO.File.ReadAllText("D:\\a.txt");
            var result = JsonConvert.DeserializeObject<List<ResponseModel>>(str);
            result.AddRange(result);
            return result;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<dynamic> GetB()
        {
            var str = System.IO.File.ReadAllText("D:\\data.txt");
            //var v1 = ClientUtil.ClientPost("http://localhost:56956/api/EDBData/JFlatTable", str);
            var v2 = ClientUtil.ClientPost("http://localhost:56956/api/EDBData/JFlatTableTwo", str);
            //var result = JsonConvert.DeserializeObject<List<ResponseModel>>(str);
            var str2 = System.IO.File.ReadAllText("D:\\a.txt");
            var result = JsonConvert.DeserializeObject<List<ResponseModel>>(str);
            result.AddRange(result);

            var ie = result.Select(o => new ResponseModel
            {
                Code = o.Code,
                DataList = o.DataList.Select(x => new DataObject
                {
                    ErrorMsg = x.ErrorMsg,
                    FDate = x.FDate,
                    Value = x.Value
                })
            });
            var a = JsonConvert.SerializeObject(result);
            var b = JsonConvert.SerializeObject(ie);
            return result.Select(o => new ResponseModel
            {
                Code = o.Code,
                DataList = o.DataList.Select(x => new DataObject
                {
                    ErrorMsg = x.ErrorMsg,
                    FDate = x.FDate,
                    Value = x.Value
                })
            });
        }

        [HttpPost]
        public void Post([FromBody] Person person)
        {
        }

        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
