namespace Tests.ZeroMq.CommandQuery
{
    public class DeleteCustomerRequest : BaseClass
    {
        public DeleteCustomerRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public override string ToString()
        {
            return $"DeleteCustomerRequest - {Id}";
        }
    }
}