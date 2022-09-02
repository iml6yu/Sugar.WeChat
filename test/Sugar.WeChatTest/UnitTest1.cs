using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sugar.WeChat;
using Sugar.WeChat.Cache;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sugar.WeChatTest
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task TestWeChatTempMsgAsync()
        {
            TemplateMessageProvider p = new TemplateMessageProvider(new WeChatAccessOption("wxe9a586dfd1ef2c71", "67eadd0b9c4ae4502869cb8c0e541ff8"),
                new WeChat.Access.WeChatAccessTokenManager(new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), new LoggerFactory())));
            var result = await p.SendOffiAccountMessageAsync(new OffiAccountMessage()
            {
                TemplateId = "JoeKgi6Kr0PRBEt2eiR61HrQ-S2lak0-dHMX6Rmof9o",
                Url = "www.baidu.com",
                Data = new MessageContent()
                {
                    MessageTitle = new MessageContentItem("测试title"),
                    MessageDatas = new List<MessageContentItem>() { new MessageContentItem("数据1"), new MessageContentItem("数据2") },
                    Remark = new MessageContentItem("备注信息")
                }
            }, "oRFyd57DxrrT_DdAHPFbVbpOHe70");
            Assert.Equal(result.Count, 1);
            Assert.Equal(result.First().Value.ErrCode, 0);
        }
    }
}
