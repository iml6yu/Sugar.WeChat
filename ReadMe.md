﻿# 微信类库

### 使用之前记得配置公众号ip白名单
*不清楚配置流程的朋友请自行查询*

## TemplateMessage 
    模板消息类库

e.g. 测试用例
```csharp
 [Fact]
        public async System.Threading.Tasks.Task TestWeChatTempMsgAsync()
        {
            TemplateMessageProvider p = new TemplateMessageProvider(new WeChatAccessOption("wx9b0f67e90ae6aff3", "68af606451a13737d0ae0bde2f31278b"), new WeChat.Access.WeChatAccessTokenManager(new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), new LoggerFactory())));
            var result = await p.SendOffiAccountMessageAsync(new OffiAccountMessage()
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
            Assert.Equal(result.Count, 1);
            Assert.Equal(result.First().Value.ErrCode, 0);
        }
```

