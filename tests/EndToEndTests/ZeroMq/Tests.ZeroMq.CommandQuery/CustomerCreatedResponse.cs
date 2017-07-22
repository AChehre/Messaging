using System;

namespace Tests.ZeroMq.CommandQuery
{
    public class CustomerCreatedResponse : BaseClass
    {
        public CustomerCreatedResponse()
        {
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public override string ToString()
        {
            return $"CustomerCreatedResponse - {Id}";
        }
    }
}