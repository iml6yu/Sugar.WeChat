using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat.TemplateMsg
{
    public class MessageContentItem
    {
        public MessageContentItem()
        {

        }
        public MessageContentItem(string text)
        {
            Text = text; 
        }
        public MessageContentItem(string text, string color):this(text)
        {
            Color = color;
        }
        [JsonProperty("value")]
        public string Text { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
