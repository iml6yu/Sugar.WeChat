using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat
{
    public class TemplateMessageResult
    {
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

        [JsonProperty("msgid")]
        public string MsgId { get; set; }
    }
}
