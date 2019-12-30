using Microsoft.AspNetCore.Mvc;
using Sugar.WeChat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sugar.WeChatSample.Controllers
{
    public class WeChatTemplageMessageController : Controller
    {
       TemplateMessageProvider provider;
        public WeChatTemplageMessageController(TemplateMessageProvider provider)
        {
            this.provider = provider;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //发送公众号模板消息
            await provider.SendOffiAccountMessageAsync(new OffiAccountMessage()
            {
                TemplateId = "JLc7M--uP751GxvVf2_Msqp1m_gne0XXiSzJISloy8g",
                Url = "www.baidu.com",
                Data = new MessageContent()
                {
                    MessageTitle = new MessageContentItem("测试title"),
                    MessageDatas = new List<MessageContentItem>() { new MessageContentItem("数据1"), new MessageContentItem("数据2") },
                    Remark = new MessageContentItem("备注信息")
                }
            }, "o6qCa1CdDnDQhEkPmwWJynGMQ4Ho");
            return View();
        }
    }
}