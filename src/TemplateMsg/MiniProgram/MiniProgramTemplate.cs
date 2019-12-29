using Sugar.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sugar.WeChat.TemplateMsg.MiniProgram
{
    public class MiniProgramTemplate
    {
        const string URL = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}";
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="openid"></param>
        /// <param name="formid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TemplateMessageResult> SendTemplateMessageAsync(string accesstoken, string openid, string formid, MiniProgramMessage message)
        { 
            var url = string.Format(URL, accesstoken);
            var data = GetPostData(openid, formid, message);
            var result = await HttpHelper.PostJsonAsync<TemplateMessageResult>(url, data);
            return result;
        }
        private object GetPostData(string openid, string formid, MiniProgramMessage message)
        {

            var data = new Dictionary<string, MessageContentItem>();

            for (var i = 1; i <= message.Data.Count; i++)
            {
                data.Add("keyword" + i.ToString(), message.Data[i]);
            }

            var msg = new
            {
                touser = openid,
                template_id = message.TemplateId,
                formid = formid,
                data = data,
                emphasis_keyword = message.EmphasisKeyword
            };
            return msg;
        }
    }
}
