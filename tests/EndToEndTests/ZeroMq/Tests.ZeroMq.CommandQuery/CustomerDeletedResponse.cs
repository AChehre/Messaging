using System;

namespace Tests.ZeroMq.CommandQuery
{
    public class CustomerDeletedResponse : BaseClass
    {
        public CustomerDeletedResponse()
        {
            DeleteTime = DateTime.Now;
        }
        public bool Deleted { get; set; }
        public DateTime DeleteTime { get; set; }

        public override string ToString()
        {
            return $"CustomerDeleteResponse - {Deleted}";
        }
    }
}