using Newtonsoft.Json;
using Sugar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sugar.WeChat.TemplateMsg.OffiAccount
{
    public class OffiAccountTemplate
    {
        const string URL = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="message"></param>
        /// <param name="openids"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Dictionary<string, TemplateMessageResult>> SendTemplateMessageAsync(string accesstoken, OffiAccountMessage message, params string[] openids)
        {
            var result = new Dictionary<string, TemplateMessageResult>();
            var url = string.Format(URL, accesstoken);
            foreach (var openid in openids)
            {
                if (result.ContainsKey(openid)) continue;
                var data = GetPostData(openid, message);
                var sendresult = await HttpHelper.PostJsonAsync<TemplateMessageResult>(url, data);
                result.Add(openid, sendresult);
            }
            return result;
        }
        private object GetPostData(string openid, OffiAccountMessage message)
        {
            if (message == null)
                throw new WeChatTemplateMessageException("消息体空异常");
            if (string.IsNullOrEmpty(message.TemplateId))
                throw new WeChatTemplateMessageException("消息模板ID空异常");
            if (message.Data == null)
                throw new WeChatTemplateMessageException("消息数据空异常");
            var data = new Dictionary<string, MessageContentItem>();
            if (message.Data.MessageTitle != null)
                data.Add("first", message.Data.MessageTitle);
            if (message.Data.MessageDatas != null)
                for (var i = 0; i < message.Data.MessageDatas.Count; i++)
                    data.Add("keyword" + (i + 1).ToString(), message.Data.MessageDatas[i]);
            if (message.Data.Remark != null)
                data.Add("remark", message.Data.Remark);
            var msg = new
            {
                touser = openid,
                template_id = message.TemplateId,
                url = message.Url,
                miniprogram = message.MiniProgram,
                data = data
            };
            return msg;
        }
    }
}
