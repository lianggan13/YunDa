using Newtonsoft.Json;
using System.Collections.Generic;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.Common.Models
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

    public class ResponseData<T> : ResponseDataBase
    {
        public T Data { get; set; }

        public static ResponseData<T> Success(T data = default, string message = "")
        {
            ResponseData<T> response = new ResponseData<T>()
            {
                Code = ResponseCode.Success,
                Message = message,
                Data = data
            };
            return response;
        }

        public static ResponseData<T> Error(ResponseCode code, string message = "")
        {
            ResponseData<T> response = new ResponseData<T>()
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

    public class LoginResponse
    {
        private LoginResponse()
        {

        }

        public LoginResult Result { get; set; }

        public int No { get; set; }

        public string AccountName { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public int RoleId { get; set; }

        public List<int> Functions { get; set; }

        public enum LoginResult
        {
            Success = 0,
            AccountNonExistent,
            PasswordError
        }

        public static LoginResponse Success()
        {
            LoginResponse response = new LoginResponse()
            {
                Result = LoginResult.Success
            };
            return response;
        }

        public static LoginResponse AccountNonExistent()
        {
            LoginResponse response = new LoginResponse()
            {
                Result = LoginResult.AccountNonExistent
            };
            return response;
        }

        public static LoginResponse PasswordError()
        {
            LoginResponse response = new LoginResponse()
            {
                Result = LoginResult.PasswordError
            };
            return response;
        }
    }
}
