using AlgorithmServer.Algorithm.DetectAlgorithm;
using AlgorithmServer.Model;
using Nancy;

namespace AlgorithmServer.Algorithm
{
    public class AlgorithmContext
    {
        private readonly Algorithm algorithm;
        public AlgorithmContext(Algorithms algType)
        {
            switch (algType)
            {
                case Algorithms.Cloth:
                    this.algorithm = new ClothCheckAlgorithm();
                    break;
                case Algorithms.Train:
                    this.algorithm = new TrainCheckAlgorithm();
                    break;
                case Algorithms.Safety:
                    this.algorithm = new SafetyChainAlgorithm();
                    break;
                case Algorithms.Personnel:
                    this.algorithm = new PersonNumAlgorithm();
                    break;
            }
        }

        /// <summary>
        /// 识别
        /// </summary>
        /// <returns></returns>
        public RecognizeResult Detect(Methods method, Request request)
        {
            algorithm.SetDetectMethod(method);
            var result = algorithm.Detect(request);
            return result;
        }

        public string GetVersion()
        {
            return algorithm.GetVersion();
        }
    }
}
