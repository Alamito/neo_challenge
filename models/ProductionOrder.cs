namespace ChallengeNeo.Models

{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int OperationNumber { get; set; }
        public int Quantity { get; set; }
        public DateTime DueDate { get; set; }
        public int ProductNumber { get; set; }
        public string Product { get; set; }
    }
}
