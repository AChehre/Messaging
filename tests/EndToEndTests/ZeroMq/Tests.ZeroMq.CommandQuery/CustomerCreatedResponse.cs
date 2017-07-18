namespace Tests.ZeroMq.CommandQuery
{
    public class CustomerCreatedResponse : BaseClass
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"CustomerCreatedResponse - {Id}";
        }
    }
}