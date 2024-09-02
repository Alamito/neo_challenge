﻿namespace ChallengeNeo.Models

{
    public class ProductionOrder
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public int OperationNumber { get; set; }
        public float Quantity { get; set; }
        public DateTime DueDate { get; set; }
        public int ProductNumber { get; set; }
        public string Product { get; set; }
    }
}
