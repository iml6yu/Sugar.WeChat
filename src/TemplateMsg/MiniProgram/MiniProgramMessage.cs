using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat
{
    public class MiniProgramMessage
    {
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("emphasis_keyword")]
        public string EmphasisKeyword { get; set; }

        [JsonProperty("data")]
        public IList<MessageContentItem> Data { get; set; }
    }


}
