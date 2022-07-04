using AlgorithmServer.Model;
using Nancy;

namespace AlgorithmServer.Algorithm.DetectMethod
{
    public interface IDetectMethod
    {
        MethodParam GetMethodParam(Request request);
    }
}
