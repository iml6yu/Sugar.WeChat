using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat.TemplateMsg.OffiAccount
{
    public class OffiAccountMessage
    {
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("miniprogram")]
        public MiniProgram MiniProgram { get; set; }

        [JsonProperty("data")]
        public MessageContent Data { get; set; }
    }
    public class MiniProgram
    {
        [JsonProperty("appid")]
        public string Appid { get; set; }

        [JsonProperty("pagepath")]
        public string PagePath { get; set; }
    }
    public class MessageContent
    {
        [JsonProperty("first")]
        public MessageContentItem MessageTitle { get; set; }
        /// <summary>
        /// 模板消息数据，对应的keywork1...keywordn
        /// </summary>
        public IList<MessageContentItem> MessageDatas { get; set; }

        [JsonProperty("remark")]
        public MessageContentItem Remark { get; set; }
    } 
}
