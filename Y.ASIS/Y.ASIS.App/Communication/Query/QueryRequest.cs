using System;
using RestSharp;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Communication.Query
{
    class UserAuthQueryRequest : AuthRequest
    {
        private const string Path = "/api/query/issue/users";

        public int? TrackId { get; set; }

        public int? UserNo { get; set; }

        public int? PositionId { get; set; }

        public IssueType IssueType { get; set; }

        public bool Revoked { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public UserAuthQueryRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (TrackId != null)
            {
                request.AddParameter("TrackId", TrackId);
            }
            if (PositionId != null)
            {
                request.AddParameter("PositionId", PositionId);
            }
            if (UserNo != null)
            {
                request.AddParameter("UserNo", UserNo);
            }
            request.AddParameter("IssueType", (int)IssueType);
            request.AddParameter("Revoked", Revoked);
            request.AddParameter("StartTime", StartTime);
            request.AddParameter("EndTime", EndTime);
            request.AddParameter("Index", Index);
            request.AddParameter("Count", Count);
            return request;
        }
    }

    class ToolListRequest : AuthRequest
    {
        private const string Path = "/api/tools/{id}";

        private readonly int trackId;
        public ToolListRequest(int trackid)
            : base(Path)
        {
            this.trackId = trackid;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddUrlSegment("id", trackId);
            return request;
        }
    }

    class ToolAuthQueryRequest : AuthRequest
    {
        private const string Path = "/api/query/issue/tools";

        public int? TrackId { get; set; }

        public int? UserNo { get; set; }

        public int? ToolId { get; set; }

        public bool Revoked { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public ToolAuthQueryRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (TrackId != null)
            {
                request.AddParameter("TrackId", TrackId);
            }
            if (ToolId != null)
            {
                request.AddParameter("ToolId", ToolId);
            }
            if (UserNo != null)
            {
                request.AddParameter("UserNo", UserNo);
            }
            request.AddParameter("Revoked", Revoked);
            request.AddParameter("StartTime", StartTime);
            request.AddParameter("EndTime", EndTime);
            request.AddParameter("Index", Index);
            request.AddParameter("Count", Count);
            return request;
        }
    }

    class FaultQueryRequest : AuthRequest
    {
        private const string Path = "/api/query/faultsAndwarnings";

        public int? TrackId { get; set; }

        public int? FaultCode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Handled { get; set; }

        public int? HandledBy { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public FaultQueryRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (TrackId != null)
            {
                request.AddParameter("TrackId", TrackId);
            }
            if (FaultCode != null)
            {
                request.AddParameter("FaultCode", FaultCode);
            }
            request.AddParameter("StartTime", StartTime);
            request.AddParameter("EndTime", EndTime);
            request.AddParameter("Index", Index);
            request.AddParameter("Count", Count);
            request.AddParameter("Handled", Handled);
            request.AddParameter("HandledBy", HandledBy);
            return request;
        }
    }

    class OperationQueryRequest : AuthRequest
    {
        private const string Path = "/api/query/operations";

        public int? TrackId { get; set; }

        public int? UserNo { get; set; }

        public int? OperationCode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public OperationQueryRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (TrackId != null)
            {
                request.AddParameter("TrackId", TrackId);
            }
            if (UserNo != null)
            {
                request.AddParameter("UserNo", UserNo);
            }
            if (OperationCode != null)
            {
                request.AddParameter("OperationCode", OperationCode);
            }
            request.AddParameter("StartTime", StartTime);
            request.AddParameter("EndTime", EndTime);
            request.AddParameter("Index", Index);
            request.AddParameter("Count", Count);
            return request;
        }
    }

    class TrainNumberQueryRequest : AuthRequest
    {
        private const string Path = "/api/query/trainnumbers";

        public int? TrackId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public TrainNumberQueryRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (TrackId != null)
            {
                request.AddParameter("TrackId", TrackId);
            }

            request.AddParameter("StartTime", StartTime);
            request.AddParameter("EndTime", EndTime);
            request.AddParameter("Index", Index);
            request.AddParameter("Count", Count);
            return request;
        }
    }
}
