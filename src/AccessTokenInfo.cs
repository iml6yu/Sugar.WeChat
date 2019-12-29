using Newtonsoft.Json;
using Sugar.WeChat.TemplateMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat
{
    public class AccessTokenInfo: TemplateMessageResult
    {
        /// <summary>
        /// token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 相对过期时间
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
