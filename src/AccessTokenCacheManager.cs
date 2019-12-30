using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat.Cache
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class AccessTokenCacheManager
    {
        private IMemoryCache cache;
        private ILogger<AccessTokenCacheManager> logger;

        /// <summary>
        /// token超时回收触发
        /// </summary>
        public event Action<string> TokenEvictioned;

        public AccessTokenCacheManager(IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            cache = memoryCache;
            logger = loggerFactory.CreateLogger<AccessTokenCacheManager>();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="token"></param>
        public void Add(string appid, AccessTokenInfo token)
        {
            var option = GetOptions(token);
            cache.Set(appid, token, option);
        }

        /// <summary>
        /// 添加appid和key
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        public void Add(string appid, string secret)
        {
            var option = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365000), Priority = CacheItemPriority.NeverRemove };
            option.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                logger.LogDebug($"密钥缓存失效。键：{key},值：{value},原因：{reason.ToString()},状态：{state}");
            });
            cache.Set(appid + "secret", secret, option);
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public AccessTokenInfo Get(string appid)
        {
            return cache.Get(appid) as AccessTokenInfo;
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public string GetSecret(string appid)
        {
            return cache.Get(appid + "secret")?.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> 
        private MemoryCacheEntryOptions GetOptions(AccessTokenInfo token)
        {
            var option = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn) };
            option.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                TokenEvictioned?.Invoke(key.ToString());
                logger.LogDebug($"缓存已失效。键：{key},值：{value},原因：{reason.ToString()},状态：{state}");
            });
            return option;
        }
    }
}
