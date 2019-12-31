using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sugar.WeChat.Cache;
using Sugar.WeChat.TemplateMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar.WeChat
{
    public static class WeChatExtensions
    {
        #region 模板消息

        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="acOption"></param>
        /// <returns></returns>
        public static IServiceCollection UseWeChatTemplateMessage(this IServiceCollection services, Action<WeChatAccessOption> acOption)
        {
            WeChatAccessOption option = new WeChatAccessOption();
            acOption?.Invoke(option);
            if (string.IsNullOrEmpty(option.AppId) || string.IsNullOrEmpty(option.AppSecret))
                throw new WeChatConfigException("配置异常，没有配置appid或者appsecret");
            return AddWeChatTemplateMessage(services, option);
        }

        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [ObsoleteAttribute("推荐在start类的ConfigureServices中使用services对象调用UseWeChatTemplateMessage")]
        public static IServiceCollection AddWeChatTemplateMessage(this IServiceCollection services, IConfigurationSection section)
        {
            if (!section.Exists())
                throw new WeChatConfigException($"节点{section.Key}不存在，请检查配置信息");
            var option = section.Get<WeChatAccessOption>();
            if (option == null)
                throw new WeChatConfigException($"节点{section.Key}配置错误，请检查配置信息");
            return AddWeChatTemplateMessage(services, option);
        }

        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        [ObsoleteAttribute("推荐在Start类的ConfigureServices中使用services对象调用UseWeChatTemplateMessage")]
        public static IServiceCollection AddWeChatTemplateMessage(this IServiceCollection services, WeChatAccessOption option)
        {
            AddWeChatAccessTokenManager(services, option);
            var tokenManager = services.First(t => t.ServiceType == typeof(Access.WeChatAccessTokenManager)).ImplementationInstance as Access.WeChatAccessTokenManager;
            if (tokenManager == null)
                throw new WeChatException("在此之前没有添加WeChatAccessTokenManager，请添加WeChatAccessTokenManager后在添加WeChatTemplateMessage单例");
            services.AddSingleton(new TemplateMessageProvider(option, tokenManager));
            return services;
        }

        /// <summary>
        /// 添加token管理工具，一般都不用添加，其他方法会自动检测并且自主添加的
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeChatAccessTokenManager(IServiceCollection services, WeChatAccessOption option)
        {
            AddAccessTokenManager(services);
            if (services.Any(t => t.ServiceType == typeof(Access.WeChatAccessTokenManager))) return services;
            var cacheManager = services.First(t => t.ServiceType == typeof(AccessTokenCacheManager)).ImplementationInstance as AccessTokenCacheManager;
            services.AddSingleton(new Access.WeChatAccessTokenManager(cacheManager, option));
            return services;
        }

        #endregion

        #region 微信支付
        /// <summary>
        /// 注册微信支付
        /// </summary>
        /// <param name="services"></param>
        /// <param name="acOption"></param>
        /// <returns></returns>
        public static IServiceCollection UseWeChatPay(this IServiceCollection services, Action<WeChatPayOptions> acOption)
        {
            if (acOption == null)
            {
                throw new ArgumentException(nameof(acOption));
            }
            var option = new WeChatPayOptions();
            acOption?.Invoke(option);
            return AddWeChatPay(services, option);
        }

        /// <summary>
        /// 注册微信支付
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeChatPay(this IServiceCollection services, IConfigurationSection section)
        {
            if (!section.Exists())
                throw new WeChatConfigException($"节点{section.Key}不存在，请检查配置信息");
            var option = section.Get<WeChatPayOptions>();
            if (option == null)
                throw new WeChatConfigException($"节点{section.Key}配置错误，请检查配置信息");
            return AddWeChatPay(services, option);
        }

        /// <summary>
        /// 注册微信支付
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeChatPay(this IServiceCollection services, WeChatPayOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }
            //添加单例
            services.AddSingleton(new WeChatPayer(options, services.BuildServiceProvider().GetRequiredService<ILoggerFactory>()));
            return services;
        }
        #endregion

        private static void AddWeChatAccessTokenManager(IServiceCollection services, string appid, string appsecret)
        {
            AddWeChatAccessTokenManager(services, new WeChatAccessOption(appid, appsecret));
        }

        private static void AddAccessTokenManager(IServiceCollection services)
        {
            if (services.Any(t => t.ServiceType == typeof(AccessTokenCacheManager))) return;
            services.AddSingleton<AccessTokenCacheManager>();
        }
    }
}
