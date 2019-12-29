using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.WeChat
{
    public class WeChatException : Exception
    {
        public WeChatException(string msg) : base(msg) { }
    }

    public class WeChatSecretNullException : WeChatException
    {
        public WeChatSecretNullException(string msg) : base(msg) { }
    }

    public class WeChatAppIdNullException : WeChatException
    {
        public WeChatAppIdNullException(string msg) : base(msg) { }
    }

    public class WeChatConfigException : WeChatException
    {
        public WeChatConfigException(string msg) : base(msg) { }
    }

    public class WeChatPayException : WeChatException
    {
        public WeChatPayException(string msg) : base(msg) { }
    }

    public class WeChatTemplateMessageException : WeChatException
    {
        public WeChatTemplateMessageException(string msg) : base(msg) { }
    }
}
