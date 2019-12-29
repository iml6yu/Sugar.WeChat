using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat.Options
{
    public interface IWeChatOption
    {
        /// <summary>
        /// appid
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        string AppSecret { get; set; }

        /// <summary>
        /// 是否debug模式
        /// </summary>
       // bool IsDebug { get; set; }

        /// <summary>
        /// accesstoken存储模式
        /// </summary>
        //TokenStoreType TokenStore { get; set; }
    }
}
