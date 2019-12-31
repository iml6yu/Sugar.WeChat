namespace Sugar.WeChat
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatPayOptions : IWeChatOption
    {
        /// <summary>
        /// 绑定支付的APPID（必须配置）
        /// </summary>
        public string AppId { get; set; }


        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        /// </summary>
        public string AppSecret { get; set; }


        /// <summary>
        /// 商户号（必须配置）
        /// </summary>
        public string MchID { get; set; }

        /// <summary>
        /// 商户支付密钥
        /// </summary>
        public string Key { get; set; }

        #region 证书配置
        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
         * 1.证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载；
         * 2.建议将证书文件名改为复杂且不容易猜测的文件
         * 3.商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
        */
        /// <summary>
        /// 证书文件路径
        /// </summary>
        public string SSlCertPath { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string SSlCertPassword { get; set; }
        #endregion

        /// <summary>
        /// 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public int ReportLevel { get; set; }

        /// <summary>
        /// 支付回调通知接口
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDebug { get; set; }
    }
}
