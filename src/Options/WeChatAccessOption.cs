using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat.Options
{
    /// <summary>
    /// 访问参数配置
    /// </summary>
    public class WeChatAccessOption : IWeChatOption
    {
        public WeChatAccessOption()
        {

        }
        public WeChatAccessOption(string appid, string appsecret)
        {
            AppId = appid;
            AppSecret = appsecret;
        }
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 是否debug模式
        /// </summary>
      //  public bool IsDebug { get; set; }

        /// <summary>
        /// accesstoken存储模式，默认是缓存
        /// </summary>
        //public TokenStoreType TokenStore { get; set; } = TokenStoreType.MemoryCache;
    }
}
