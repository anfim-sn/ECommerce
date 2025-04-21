namespace ECommerce.Core.RabbitMQ;

public record ProductNameUpdateMessage(Guid ProductId, string? NewName)
{
    
}