using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.App.Communication.Algorithm;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.Services
{
    public class AlgorithmService
    {
        public static object Heartbeat()
        {
            AlgorithmHeartRequest algorithmHeartRequest = new AlgorithmHeartRequest();
            return algorithmHeartRequest.Request<ResponseData<object>>();
        }

        public static DetectResult DetectSafety(int chan, string extension)
        {
            //Max Resolution Ratio [1920 x 1088]
            const string DefaultCoordinate = "{\"Coordinate\":{\"X\":[0,1900,1900,0,0],\"Y\":[0,0,1000,1000,0]}}";

            string pointsStr = ParseVideoCoordinate(extension);
            if (string.IsNullOrEmpty(pointsStr))
                pointsStr = ParseVideoCoordinate(DefaultCoordinate);


            AlgorithmDetectRequest request = new AlgorithmDetectRequest(DetectType.Safety, MethodType.SDK, channel: chan, streamUrl: "", extendedParams: pointsStr);
            var response = request.Request<ResponseData<DetectResult>>();

            return response?.Data;
        }

        public static DetectResult DetectTrain(string url, int chan)
        {
            AlgorithmDetectRequest request = new AlgorithmDetectRequest(DetectType.Train, MethodType.SDK, channel: chan, streamUrl: url);
            var response = request.Request<ResponseData<DetectResult>>();

            return response?.Data;
        }

        private static string ParseVideoCoordinate(string extension)
        {
            string pointsStr = "";
            try
            {
                if (!string.IsNullOrEmpty(extension))
                {
                    JObject jobj = JObject.Parse(extension);
                    if (jobj.TryGetValue("Coordinate", out JToken jt2))
                    {
                        var xx = jt2["X"].ToObject<List<int>>();
                        var yy = jt2["Y"].ToObject<List<int>>();
                        var points3 = xx.Zip(yy, (x, y) => new { x, y });
                        pointsStr = Newtonsoft.Json.JsonConvert.SerializeObject(points3);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }

            return pointsStr;
        }


    }
}
