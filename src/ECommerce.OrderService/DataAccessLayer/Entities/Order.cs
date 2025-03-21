using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid _id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid OrderId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public DateTime OrderDate { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public decimal TotalBill { get; set; }
 
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid _id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public decimal UnitPrice { get; set; }

    [BsonRepresentation(BsonType.Int32)]
    public int Quantity { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public decimal TotalPrice { get; set; }
}