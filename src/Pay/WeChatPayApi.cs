
using Microsoft.Extensions.Logging;
using System;

namespace Sugar.WeChat
{
    internal class WeChatPayApi
    {
        private string payHost = string.Empty;
        private WeChatPayOptions option;
        private ILogger<WeChatPayApi> logger;
        private HttpService httpService;

        internal WeChatPayApi(WeChatPayOptions option, ILoggerFactory loggerFactory)
        {
            if (option.IsDebug)
                payHost = "https://api.mch.weixin.qq.com/sandboxnew";
            else
                payHost = "https://api.mch.weixin.qq.com";
            logger = loggerFactory.CreateLogger<WeChatPayApi>();
            httpService = new HttpService(option);
            this.option = option;
        }
        /// <summary>
        /// 统一下单（微信内）
        /// </summary>
        /// <param name="productDes"></param>
        /// <param name="billno"></param>
        /// <returns></returns>
        internal WeChatPayData UnifiedOrderJsApi(string productDes, string billno, int total_fee, string openid, string ip)
        {
            WeChatPayData data = GetWeChatPayData(productDes, billno, total_fee, ip, "JSAPI", openid);
            return UnifiedOrder(data);
        }

        /// <summary>
        /// 统一下单 非微信内（web）
        /// </summary>
        /// <param name="productDes"></param>
        /// <param name="billno"></param>
        /// <param name="notifyUrl"></param>
        /// <param name="TradeType"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal WeChatPayData UnifiedOrderH5(string productDes, string billno, int total_fee, string ip)
        {
            WeChatPayData payData = GetWeChatPayData(productDes, billno, total_fee, ip, "MWEB");
            return UnifiedOrder(payData);
        }

        private WeChatPayData GetWeChatPayData(string productDes, string billno, int total_fee, string ip, string tradeType, string openid = "")
        {
            WeChatPayData data = new WeChatPayData();
            data.SetValue("body", productDes);
            data.SetValue("out_trade_no", billno);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddHours(25).ToString("yyyyMMddHHmmss"));
            data.SetValue("trade_type", tradeType);
            if (openid != "")
                data.SetValue("openid", openid);
            data.SetValue("notify_url", option.NotifyUrl);
            data.SetValue("appid", option.AppId);//公众账号ID
            data.SetValue("mch_id", option.MchID);//商户号 
            data.SetValue("spbill_create_ip", ip);
            data.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            data.SetValue("sign_type", WeChatPayData.SIGN_TYPE_MD5);//签名类型
            //签名
            data.SetValue("sign", data.MakeSign(option));
            return data;
        }

        internal WeChatPayData UnifiedOrder(WeChatPayData inputObj, int timeOut = 6)
        {
            logger.LogInformation($"下单地址：{payHost}/pay/unifiedorder");
            string url = payHost + "/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WeChatPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new WeChatPayException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WeChatPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new WeChatPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new WeChatPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new WeChatPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                throw new WeChatPayException("统一支付接口中，缺少必填参数notify_url！trade_type为JSAPI时，notify_url为必填参数！");
            }
            logger.LogDebug(inputObj.ToJson());
            string xml = inputObj.ToXml();

            logger.LogDebug("统一下单请求参数 : " + xml);
            string response = httpService.Post(xml, url, false, timeOut);
            logger.LogDebug("统一下单返回结果: " + response);

            WeChatPayData result = new WeChatPayData();
            result.FromXml(response);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                throw new WeChatPayException("统一下单下单失败");
            }
            return result;
        }

        internal WeChatPayData OrderQuery(WeChatPayData inputObj, int timeOut = 6)
        {
            string url = payHost + "/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WeChatPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            inputObj.SetValue("appid", option.AppId);//公众账号ID
            inputObj.SetValue("mch_id", option.MchID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            inputObj.SetValue("sign_type", WeChatPayData.SIGN_TYPE_MD5);//签名类型
            inputObj.SetValue("sign", inputObj.MakeSign(option));//签名 
            string xml = inputObj.ToXml();
            logger.LogDebug("OrderQuery request : " + xml);
            string response = httpService.Post(xml, url, false, timeOut);//调用HTTP通信接口提交数据
            logger.LogDebug("OrderQuery response : " + response);
            //将xml格式的数据转化为对象以返回
            WeChatPayData result = new WeChatPayData();
            result.FromXml(response);
            return result;
        }


        /// <summary>
        /// 撤销订单API接口
        /// </summary>
        /// <param name="inputObj"> 提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个</param>
        /// <param name="timeOut">接口超时时间</param>
        /// <returns></returns>
        internal WeChatPayData Reverse(WeChatPayData inputObj, int timeOut = 6)
        {
            string url = payHost + "/secapi/pay/reverse";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WeChatException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }
            inputObj.SetValue("appid", option.AppId);//公众账号ID
            inputObj.SetValue("mch_id", option.MchID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            inputObj.SetValue("sign_type", "MD5");//签名类型
            inputObj.SetValue("sign", inputObj.MakeSign(option));//签名
            string xml = inputObj.ToXml();
            logger.LogDebug("WxPayApi", "Reverse request : " + xml);
            string response = httpService.Post(xml, url, true, timeOut);
            logger.LogDebug("WxPayApi", "Reverse response : " + response);
            WeChatPayData result = new WeChatPayData();
            result.FromXml(response);
            return result;
        }


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="inputObj">提交给申请退款API的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        internal WeChatPayData Refund(WeChatPayData inputObj, int timeOut = 6)
        {
            string url = payHost + "/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WeChatException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WeChatException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WeChatException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WeChatException("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new WeChatException("退款申请接口中，缺少必填参数op_user_id！");
            }

            inputObj.SetValue("appid", option.AppId);//公众账号ID
            inputObj.SetValue("mch_id", option.MchID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            inputObj.SetValue("sign_type", WeChatPayData.SIGN_TYPE_MD5);//签名类型
            inputObj.SetValue("sign", inputObj.MakeSign(option));//签名 
            string xml = inputObj.ToXml();
            logger.LogDebug("WxPayApi", "Refund request : " + xml);
            string response = httpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API
            logger.LogDebug("WxPayApi", "Refund response : " + response);
            //将xml格式的结果转换为对象以返回
            WeChatPayData result = new WeChatPayData();
            result.FromXml(response);
            return result;
        }

        /// <summary>
        /// 查询退款
        /// </summary>
        /// <remarks>提交退款申请后，通过该接口查询退款状态。退款有一定延时，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。 out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个</remarks>
        /// <param name="inputObj">提交给查询退款API的参数</param>
        /// <param name="timeOut"> 接口超时时间</param>
        /// <returns></returns>
        internal WeChatPayData RefundQuery(WeChatPayData inputObj, int timeOut = 6)
        {
            string url = payHost + "/pay/refundquery";
            //检测必填参数
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new WeChatException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }
            inputObj.SetValue("appid", option.AppId);//公众账号ID
            inputObj.SetValue("mch_id", option.MchID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            inputObj.SetValue("sign_type", WeChatPayData.SIGN_TYPE_MD5);//签名类型
            inputObj.SetValue("sign", inputObj.MakeSign(option));//签名  
            string xml = inputObj.ToXml();
            logger.LogDebug("WxPayApi", "RefundQuery request : " + xml);
            string response = httpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            logger.LogDebug("WxPayApi", "RefundQuery response : " + response);
            //将xml格式的结果转换为对象以返回
            WeChatPayData result = new WeChatPayData();
            result.FromXml(response);
            return result;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="inputObj">提交给关闭订单API的参数</param>
        /// <param name="timeOut">接口超时时间</param>
        /// <returns></returns>
        internal WeChatPayData CloseOrder(WeChatPayData inputObj, int timeOut = 6)
        {
            string url = payHost + "/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WeChatException("关闭订单接口中，out_trade_no必填！");
            } 
            inputObj.SetValue("appid", option.AppId);//公众账号ID
            inputObj.SetValue("mch_id", option.MchID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString("N").ToLower());//随机字符串
            inputObj.SetValue("sign_type", WeChatPayData.SIGN_TYPE_MD5);//签名类型
            inputObj.SetValue("sign", inputObj.MakeSign(option));//签名  
            string xml = inputObj.ToXml(); 
            string response = httpService.Post(xml, url, false, timeOut);
            WeChatPayData result = new WeChatPayData();
            result.FromXml(response); 
            return result;
        }

        internal string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
 }