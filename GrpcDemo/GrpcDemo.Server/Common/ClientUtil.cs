using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;

namespace GrpcDemo.Server.Common
{
    public class ClientUtil
    {
        private static HttpClient client;
        private static object _lockHelper = new object();

        private static HttpClient GetClient()
        {
            if (client == null)
            {
                lock (_lockHelper)
                {
                    if (client == null)
                    {
                        client = new HttpClient();
                    }
                }
            }
            return client;
        }

        /// <summary>
        /// 向ETIBooking提交Get请求
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <returns></returns>
        public static string ClientGet(string action)
        {
            var ResponseMessage = GetClient().GetAsync(action).Result;
            return ResponseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 以Post方式向ETIBooking提交请求
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string ClientPost(string action, string parameter)
        {
            var client = GetClient();
            //转为链接需要的格式
            StringContent stringContent = new StringContent(parameter);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var ResponseMessage = client.PostAsync(action, stringContent).Result;
            return ResponseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
