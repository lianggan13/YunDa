namespace AlgorithmServer
{
    class ResponseData<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T>()
            {
                Code = 200,
                Data = data
            };
        }

        public static ResponseData<object> Error(string message)
        {
            return new ResponseData<object>()
            {
                Code = 500,
                Message = message
            };
        }


    }



}
