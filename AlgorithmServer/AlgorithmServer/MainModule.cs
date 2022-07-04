using AlgorithmServer.Algorithm;
using AlgorithmServer.Exceptions;
using AlgorithmServer.Model;
using Nancy;
using System;

namespace AlgorithmServer
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/api/heart", Heart);
            Post("/api/detect", Detect);
            Get("/api/version/{name}", GetDetectVersion);
            Get("/api/version", GetServerVersion);

            Before += BeforeCall;
            After += AfterCall;
            OnError += ThrownException;
        }

        private object Heart(dynamic _)
        {
            return ResponseData.Success("Heart").ToString();
        }

        private object Detect(dynamic _)
        {
            string detectStr = GetFormByKey("Detect");
            string methodStr = GetFormByKey("Method");
            Algorithms algorithm = (Algorithms)Enum.Parse(typeof(Algorithms), detectStr);
            Methods method = (Methods)Enum.Parse(typeof(Methods), methodStr);

            Console.WriteLine($"   Detect: {detectStr},Method: {methodStr}");
            AlgorithmContext context = new AlgorithmContext(algorithm);
            RecognizeResult result = context.Detect(method, Request);
            Console.WriteLine($"Result: {result?.Result}");

            return ResponseData<RecognizeResult>.Success(result).ToString();
        }


        private object GetDetectVersion(dynamic _)
        {
            string detect = _.name;
            Algorithms algorithm = (Algorithms)Enum.Parse(typeof(Algorithms), detect);
            AlgorithmContext context = new AlgorithmContext(algorithm);
            string version = context.GetVersion();
            return ResponseData<string>.Success(version).ToString();
        }

        private object GetServerVersion(dynamic _)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            version = "Algorithm Server: Version " + version;
            return ResponseData<string>.Success(version).ToString();
        }

        public string GetFormByKey(string key)
        {
            if (!Request.Form.ContainsKey(key))
            {
                throw new Exception();
            }
            return Request.Form.ToDictionary()[key];
        }


        private Response ThrownException(NancyContext ctx, Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            if (e is AuthKeyNotFoundException)
            {
                return ResponseData.Error(ResponseCode.AuthKeyNotFoundOrError).ToString();
            }
            else if (e is AuthKeyIsDisableException)
            {
                return ResponseData.Error(ResponseCode.AuthKeyIsDisable).ToString();
            }
            else if (e is ParameterIsNullOrMissingException parameterIsNullOrMissingException)
            {
                return ResponseData.Error(
                    ResponseCode.ParameterIsNullOrMissing,
                    $"parameter \'{parameterIsNullOrMissingException.ParameterName}\' is null or missing."
                    ).ToString();
            }
            else if (e is ParameterDataFormatOrValueException parameterDataFormatOrValueException)
            {
                return ResponseData.Error(
                    ResponseCode.ParameterDataFormatOrValueError,
                    $"parameter \'{parameterDataFormatOrValueException.ParameterName}\' value \'{parameterDataFormatOrValueException.ParameterValue}\' data format or value error."
                    ).ToString();
            }
            else if (e is ContentCastException contentCastException)
            {
                return ResponseData.Error(
                    ResponseCode.ContentCastError,
                    $"content \'{contentCastException.Content}\' can not cast to designative object."
                    ).ToString();
            }
            return ResponseData.Error(
                ResponseCode.UnhandleError,
                $"{e.Message}"
                ).ToString();
        }
        private Response BeforeCall(NancyContext ctx)
        {
            if (!ctx.Request.Url.Path.EndsWith("/api/heart"))
                Console.WriteLine($">> {ctx.Request.UserHostAddress} {ctx.Request.Method} {ctx.Request.Url}");

            return null;
        }
        private void AfterCall(NancyContext ctx)
        {

        }

    }
}
