# 微信类库

## TemplateMessage 
    模板消息类库

e.g. 测试用例
```csharp
 [Fact]
        public async System.Threading.Tasks.Task TestWeChatTempMsgAsync()
        {
            TemplateMessageProvider p = new TemplateMessageProvider(new WeChatAccessOption("appid", "apsecret"), new WeChat.Access.WeChatAccessTokenManager(new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), new LoggerFactory()), new WeChatAccessOption("appid", "apsecret")));
            var result = await p.SendOffiAccountMessageAsync(new OffiAccountMessage()
            {
                TemplateId = "JLc7M--uP751GxvVf2_Msqp1m_gne0XXiSzJISloy8g",
                Url = "www.baidu.com",
                Data = new MessageContent()
                {
                    MessageTitle = new MessageContentItem("测试title"),
                    MessageDatas = new List<MessageContentItem>() { new MessageContentItem("数据1"),new MessageContentItem("数据2")},
                    Remark = new MessageContentItem("备注信息")
                }
            }, "o6qCa1CdDnDQhEkPmwWJynGMQ4Ho");
            Assert.Equal(result.Count, 1);
        }
```

