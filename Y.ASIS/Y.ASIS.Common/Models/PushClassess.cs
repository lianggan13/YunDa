using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.Common.Models
{
    public class PushMessage
    {
        public PushMessage(PushDataType type, object data)
        {
            Type = type;
            Data = data;
        }

        /// <summary>
        /// 推送数据类型
        /// </summary>
        public PushDataType Type { get; set; }

        /// <summary>
        /// 推送数据
        /// </summary>
        public object Data { get; set; }

        public override string ToString()
        {
            return this.JsonSerialize();
        }
    }

    public class PushMessage<T>
    {
        /// <summary>
        /// 推送数据类型
        /// </summary>
        public PushDataType Type { get; set; }

        /// <summary>
        /// 推送数据
        /// </summary>
        public T Data { get; set; }
    }
}
