using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat
{
    public class WeChatPayedParameters
    {
        /// <summary>
        ///  	是	String(16)	SUCCESS	SUCCESS 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        ///  	是	String(128)	OK	OK
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        ///  	是	String(32)	wx8888888888888888	微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        ///  	是	String(32)	1900000109	微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        ///  	否	String(32)	013467007045764	微信支付分配的终端设备号，
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// //	是	String(32)	5K8264ILTKCH16CQ2502SI8ZNMTM67VS	随机字符串，不长于32位
        /// </summary> 
        public string nonce_str { get; set; }
        /// <summary>
        ///  	是	String(32)	C380BEC2BFD727A4B6845133519F3AD6	签名，详见签名算法
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// //	否	String(32)	HMAC-SHA256	签名类型，目前支持HMAC-SHA256和MD5，默认为MD5
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// //	是	String(16)	SUCCESS	SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// //	否	String(32)	SYSTEMERROR	错误返回的信息描述
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// //	否	String(128)	系统错误	错误返回的信息描述
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// //	是	String(128)	wxd930ea5d5a258f4f	用户在商户appid下的唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// //	是	String(1)	Y	用户是否关注公众账号，Y-关注，N-未关注
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// //	是	String(16)	JSAPI	JSAPI、NATIVE、APP 
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// //	是	Int	100	订单总金额，单位为分 
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// //	是	String(32)	1217752501201407033233368018	微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// //	是	String(32)	1212321211201407033568112322	商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// //	否	String(128)	123456	商家数据包，原样返回
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// //	是	String(14)	20141030133525	支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        public string time_end { get; set; }

    }
}
