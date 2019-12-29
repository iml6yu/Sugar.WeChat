using Sugar.WeChat.Access;
using Sugar.WeChat.Options;
using Sugar.WeChat.TemplateMsg.MiniProgram;
using Sugar.WeChat.TemplateMsg.OffiAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar.WeChat.TemplateMsg
{
    public class TemplateMessageProvider
    {
        private WeChatAccessOption option;
        private WeChatAccessTokenManager tokenManager;
        private OffiAccountTemplate offiAccountTemplate;
        private MiniProgramTemplate miniProgramTemplate;

        public TemplateMessageProvider(WeChatAccessTokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
            offiAccountTemplate = new OffiAccountTemplate();
            miniProgramTemplate = new MiniProgramTemplate();
        }

        public TemplateMessageProvider(WeChatAccessOption option, WeChatAccessTokenManager tokenManager) : this(tokenManager)
        {
            this.option = option;
        }

        /// <summary>
        /// 发送公众号模板消息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="message"></param>
        /// <param name="openids"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Dictionary<string, TemplateMessageResult>> SendOffiAccountMessageAsync(string appid, OffiAccountMessage message, params string[] openids)
        {
            var result = await offiAccountTemplate.SendTemplateMessageAsync((await tokenManager.GetAccessTokenAsync(appid)).AccessToken, message, openids);
            return result;
        }

        /// <summary>
        /// 发送公众号模板消息（默认配置）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="openids"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Dictionary<string, TemplateMessageResult>> SendOffiAccountMessageAsync(OffiAccountMessage message, params string[] openids)
        {
            CheckOption(option);
            return await SendOffiAccountMessageAsync(option.AppId, message, openids);
        }

        /// <summary>
        /// 发送小程序模板消息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="openid"></param>
        /// <param name="formid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TemplateMessageResult> SendMiniProgramMessageAsync(string appid, string openid, string formid, MiniProgramMessage message)
        {
            var result = await miniProgramTemplate.SendTemplateMessageAsync((await tokenManager.GetAccessTokenAsync(appid)).AccessToken, openid, formid, message);
            return result;
        }

        /// <summary>
        /// 发送小程序模板消息（默认配置）
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="formid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TemplateMessageResult> SendMiniProgramMessageAsync(string openid, string formid, MiniProgramMessage message)
        {
            CheckOption(option);
            return await SendMiniProgramMessageAsync(option.AppId, openid, formid, message);
        }

        private void CheckOption(WeChatAccessOption option)
        {
            if (option == null || string.IsNullOrEmpty(option.AppId))
                throw new WeChatException("没有配置默认参数WeChatAccessOption,请在Add或者Use的时候配置参数");
        }
    }
}
