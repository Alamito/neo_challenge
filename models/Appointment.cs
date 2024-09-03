namespace ChallengeNeo.Models

{
    public class Appointment
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public int OperationNumber { get; set; }
        public float Quantity { get; set; }
        public DateTime ProductionDateTime { get; set; }
    }
}
