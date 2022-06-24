using RestSharp;
using System.Collections.Generic;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Communication
{
    class PositionCommmandRequest : AuthRequest
    {
        private const string Path = "/api/position/command/{positionId}/{command}/{no}";

        private readonly int positionId;
        private readonly int command;
        private readonly int no;
        public PositionCommmandRequest(int positionId, int command, int no)
            : base(Path)
        {
            this.positionId = positionId;
            this.command = command + 10000;                                  //PLC提供的操作记录码与命令通讯码有重复，加10000给它们区分开
            this.no = no;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("command", command);
            request.AddUrlSegment("no", no);
            return request;
        }
    }

    class PositionApplyRequest : AuthRequest
    {
        private const string Path = "/api/position/apply/{positionId}/{apply}/{no}";


        private readonly int positionId;
        private readonly int apply;
        private readonly int no;
        public PositionApplyRequest(int positionId, int apply, int no)
            : base(Path)
        {
            this.positionId = positionId;
            this.apply = apply;
            this.no = no;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("apply", apply);
            request.AddUrlSegment("no", no);
            return request;
        }
    }

    class PositionSwitchCommandRequest : AuthRequest
    {
        private const string Path = "/api/position/switch/{index}";

        private readonly int index;
        public PositionSwitchCommandRequest(int index)
            : base(Path)
        {
            this.index = index;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("index", index);
            return request;
        }
    }

    class PositionResetRequest : AuthRequest
    {
        private const string Path = "/api/position/reset/{positionId}/{command}/{no}";

        private readonly int positionId;
        private readonly int command;
        private readonly int no;
        public PositionResetRequest(int positionId, int command, int no)
            : base(Path)
        {
            this.positionId = positionId;
            this.command = command;
            this.no = no;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("command", command);
            request.AddUrlSegment("no", no);
            return request;
        }
    }

    class PositionSafeConfirmRequest : AuthRequest
    {
        private const string Path = "/api/position/confirm/{positionId}/{command}/{no}";

        private readonly int positionId;
        private readonly int command;
        private readonly int no;
        public PositionSafeConfirmRequest(int positionId, int command, int no)
            : base(Path)
        {
            this.positionId = positionId;
            this.command = command + 20000; //PLC提供的操作记录码与命令通讯码有重复，加20000给它们区分开
            this.no = no;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Timeout = 5000;
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("command", command);
            request.AddUrlSegment("no", no);
            return request;
        }
    }

    class PositionAlgorithmConfirmRequest : AuthRequest
    {
        private const string Path = "/api/position/confirm/algorithm";

        private readonly int positionId;
        private readonly int command;
        private readonly int no;
        private readonly string msg;

        public PositionAlgorithmConfirmRequest(int positionId, int command, int no, string msg)
            : base(Path)
        {
            this.positionId = positionId;
            this.command = command + 20000;  //PLC提供的操作记录码与命令通讯码有重复，加20000给它们区分开
            this.no = no;
            this.msg = msg;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Timeout = 6000;
            request.Method = Method.POST;
            request.AddParameter("positionId", positionId);
            request.AddParameter("command", command);
            request.AddParameter("no", no);
            request.AddParameter("msg", msg);

            return request;
        }
    }

    class PositionSignalLightSwitchRequest : AuthRequest
    {
        private const string Path = "/api/position/signal/switch/{positionId}/{index}/{command}";

        private readonly int positionId;
        private readonly int index;
        private readonly int command;
        public PositionSignalLightSwitchRequest(int positionId, int index, int command)
            : base(Path)
        {
            this.positionId = positionId;
            this.index = index;
            this.command = command;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("index", index);
            request.AddUrlSegment("command", command);
            return request;
        }
    }

    class PositionGateSwitchRequest : AuthRequest
    {
        private const string Path = "/api/position/gate/switch/{positionId}/{index}/{command}";

        private readonly int positionId;
        private readonly int index;
        private readonly int command;
        public PositionGateSwitchRequest(int positionId, int index, int command)
            : base(Path)
        {
            this.positionId = positionId;
            this.index = index;
            this.command = command;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            request.AddUrlSegment("index", index);
            request.AddUrlSegment("command", command);
            return request;
        }
    }

    class PositionIssueUsersRequest : AuthRequest
    {
        private const string Path = "/api/position/issue/user";

        private readonly object parameters;

        public PositionIssueUsersRequest(int positionId, IEnumerable<int> operatorNos, IEnumerable<int> workerNos, bool? isInspect)
            : base(Path)
        {
            parameters = new
            {
                PositionId = positionId,
                WorkerNos = workerNos,
                OperatorNos = operatorNos,
                IsInspect = isInspect
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json", parameters.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class PositionRevokeUsersRequest : AuthRequest
    {
        private const string Path = "/api/position/revoke/user";

        private readonly object parameters;

        public PositionRevokeUsersRequest(int positionId, IEnumerable<int> operatorNos, IEnumerable<int> workerNos)
            : base(Path)
        {
            parameters = new
            {
                PositionId = positionId,
                WorkerNos = workerNos,
                OperatorNos = operatorNos
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json", parameters.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class PositionIssuedOperatorsRequest : AuthRequest
    {
        private const string Path = "/api/position/issued/operators/{positionId}";

        private readonly int positionId;
        public PositionIssuedOperatorsRequest(int positionId)
            : base(Path)
        {
            this.positionId = positionId;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            return request;
        }
    }

    class PositionIssuedWorkersRequest : AuthRequest
    {
        private const string Path = "/api/position/issued/workers/{positionId}";

        private readonly int positionId;
        public PositionIssuedWorkersRequest(int positionId)
            : base(Path)
        {
            this.positionId = positionId;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("positionId", positionId);
            return request;
        }
    }
}
