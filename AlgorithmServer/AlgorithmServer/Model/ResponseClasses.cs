using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmServer
{
    public enum ResponseCode
    {
        Success = 200,
        NotFound = 404,
        MethodNotAllow = 405,
        UnhandleError = 500,
        AuthKeyNotFoundOrError = 501,
        ParameterIsNullOrMissing = 502,
        ParameterDataFormatOrValueError = 503,
        ContentCastError = 504,
        AuthKeyIsDisable = 505
    }

    public abstract class ResponseDataBase
    {
        public ResponseCode Code { get; set; }

        public string Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess
        {
            get { return Code == ResponseCode.Success; }
        }

        public override string ToString()
        {
            return this.JsonSerialize();
        }
    }
    
    public class ResponseData : ResponseDataBase
    {
        public object Data { get; set; }

        public static ResponseData Success(object data = null, string message = "")
        {
            ResponseData response = new ResponseData()
            {
                Code = ResponseCode.Success,
                Message = message,
                Data = data
            };
            return response;
        }

        public static ResponseData Error(ResponseCode code, string message = "")
        {
            ResponseData response = new ResponseData()
            {
                Code = code,
                Message = message
            };
            return response;
        }
    }

    public class PageData<T>
    {
        public PageData(IEnumerable<T> items, long total, int count, int index)
        {
            Items = items;
            Total = total;
            Count = count;
            Index = index;
        }

        public IEnumerable<T> Items { get; set; }

        public long Total { get; set; }

        public int Count { get; set; }

        public int Index { get; set; }
    }

    public class PageData
    {
        public PageData(IEnumerable<object> items, long total, int count, int index)
        {
            Items = items;
            Total = total;
            Count = count;
            Index = index;
        }

        public IEnumerable<object> Items { get; set; }

        public long Total { get; set; }

        public int Count { get; set; }

        public int Index { get; set; }
    }
}
