namespace ECommerce.Core.DTO;

public record UserDTO(Guid UserId, string? Email, string? PersonName, string Gender)
{
    public UserDTO() : this(default, default, default, default) {}
}