using Sugar.Utils;
using Sugar.WeChat.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sugar.WeChat.Access
{
    /// <summary>
    /// accesstoken管理类
    /// </summary>
    public class WeChatAccessTokenManager
    {
        private AccessTokenCacheManager cacheManager;
        public WeChatAccessTokenManager(AccessTokenCacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
            this.cacheManager.TokenEvictioned += CacheManager_TokenEvictioned;
        }

        /// <summary>
        /// 适用于只有一个appid的情况
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="option"></param>
        public WeChatAccessTokenManager(AccessTokenCacheManager cacheManager, WeChatAccessOption option) : this(cacheManager)
        {
            if (option != null)
                cacheManager.Add(option.AppId, option.AppSecret);
        }

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appscret"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<AccessTokenInfo> GetAccessTokenAsync(string appid, string appscret)
        {
            var value = cacheManager.Get(appid);
            if (value != null) return value;
            return await RequestAccessTokenAsync(appid, appscret);
        }

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<AccessTokenInfo> GetAccessTokenAsync(string appid)
        {
            var value = cacheManager.Get(appid);
            if (value != null) return value;
            if (cacheManager.GetSecret(appid) == null)
                throw new WeChatSecretNullException("当前appid第一次出现，请调用包含appsecret参数的方法");
            return await RequestAccessTokenAsync(appid, cacheManager.GetSecret(appid));
        }

        private async System.Threading.Tasks.Task<AccessTokenInfo> RequestAccessTokenAsync(string appid, string appscret)
        {
            var accesstoken = await HttpHelper.GetJsonAsync<AccessTokenInfo>( "https://api.weixin.qq.com/cgi-bin/token", new { grant_type = "client_credential", appid = appid, secret = appscret });
            return accesstoken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        private void CacheManager_TokenEvictioned(string appid)
        {
            RequestAccessTokenAsync(appid, cacheManager.GetSecret(appid)).Wait(60);
        }
    }
}
