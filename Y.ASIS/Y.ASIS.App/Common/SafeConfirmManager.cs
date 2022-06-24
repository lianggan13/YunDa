using System;
using System.Collections.Generic;
using System.IO;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Common
{
    public class SafeConfirmManager
    {
        private static SafeConfirmManager instance;
        public static SafeConfirmManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SafeConfirmManager();
                }
                return instance;
            }
        }

        private readonly Dictionary<int, SafeConfirm> dict;

        private SafeConfirmManager()
        {
            string file = Path.Combine(AppGlobal.Instance.ExecuteDirectory, "Config", "SafeConfirmConfig.json");
            using (StreamReader reader = File.OpenText(file))
            {
                string json = reader.ReadToEnd();
                dict = json.JsonDeserialize<Dictionary<int, SafeConfirm>>();
            }
        }

        public SafeConfirm GetSafeConfirm(int code)
        {
            if (dict != null && dict.TryGetValue(code, out SafeConfirm value))
                return value.JsonDeepCopy();
            else
                return null;
        }


        public void SendSafeConfirmRequest<T>(int posId, int no, int userNo, Action<T> callback)
        {
            int command = no + 100;
            var request = new PositionSafeConfirmRequest(posId, command, userNo);
            request.RequestAsync(callback);
        }

        public void SendAlgorithmConfirmRequest<T>(int posId, int no, int userNo, bool pass, string msg, Action<T> callback)
        {
            int command = no + (pass ? 100 : 200);
            var request = new PositionAlgorithmConfirmRequest(posId, command, userNo, msg);
            request.RequestAsync(callback);
        }

    }
}
