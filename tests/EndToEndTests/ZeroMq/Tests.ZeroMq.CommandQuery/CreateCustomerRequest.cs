namespace Tests.ZeroMq.CommandQuery
{
    public class CreateCustomerRequest : BaseClass
    {
        public CreateCustomerRequest(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return $"CreateCustomerRequest - {Id} : {Name}";
        }
    }
}