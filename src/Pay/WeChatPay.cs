using Microsoft.Extensions.Logging;
using System;

namespace Sugar.WeChat
{
    public class WeChatPayer
    {
        private WeChatPayOptions option;

        private ILogger<WeChatPayer> logger;

        private WeChatPayApi weChatPayApi;

        public WeChatPayer(WeChatPayOptions option, ILoggerFactory loggerFactory)
        {
            this.option = option;
            this.logger = loggerFactory.CreateLogger<WeChatPayer>();
            weChatPayApi = new WeChatPayApi(option, loggerFactory);


        }

        /// <summary>
        /// 调用统一下单，获得下单结果
        /// </summary>
        /// <example>
        ///从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        ///微信浏览器调起JSAPI时的输入参数格式如下：
        ///{
        ///  "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        ///  "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        ///  "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        ///  "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        ///  "signType" : "MD5",         //微信签名方式:    
        ///  "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        ///}
        ///@return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        ///更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7 
        /// </example>
        /// <returns>统一下单结果</returns>
        /// <exception cref="WeChatPayException">失败时抛异常WxPayException</exception>
        public string GetJsApiParameters(string productDes, string billno, int total_fee, string openid, string ip)
        {
            var unifiedOrder = weChatPayApi.UnifiedOrderJsApi(productDes, billno, total_fee, openid, ip);
            return GetPayParapeters(unifiedOrder);
        }

        /// <summary>
        /// 判断是否已经支付成功了
        /// </summary>
        /// <param name="weChatPayed"></param>
        /// <returns></returns>
        public bool IsPayed(WeChatPayedParameters weChatPayed)
        {
            if (weChatPayed == null || weChatPayed.return_code != "SUCCESS") return false;
            try
            {
                WeChatPayData data = new WeChatPayData();
                data.SetValue("out_trade_no", weChatPayed.out_trade_no);
                var queryresult = weChatPayApi.OrderQuery(data);
                logger.LogDebug("查询单据结果：" + queryresult.ToJson());
                return queryresult.GetValue("trade_state")?.ToString() == "SUCCESS";
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 调用统一下单，获得下单结果
        /// </summary>
        /// <returns>统一下单结果</returns>
        /// <exception cref="WeChatPayException">失败时抛异常WxPayException</exception>
        public string GetH5Parameters(string productDes, string billno, int total_fee, string ip)
        {
            var unifiedOrder = weChatPayApi.UnifiedOrderH5(productDes, billno, total_fee, ip);
            return unifiedOrder.GetValue("mweb_url").ToString();
        }

        private string GetPayParapeters(WeChatPayData unifiedOrder)
        {
            WeChatPayData jsApiParam = new WeChatPayData();
            jsApiParam.SetValue("appId", option.AppId);
            jsApiParam.SetValue("timeStamp", weChatPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", Guid.NewGuid().ToString("N").ToLower());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrder.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(option));
            string parameters = jsApiParam.ToJson();
            logger.LogDebug("Get jsApiParam : " + parameters);
            return parameters;
        }
    }
}
