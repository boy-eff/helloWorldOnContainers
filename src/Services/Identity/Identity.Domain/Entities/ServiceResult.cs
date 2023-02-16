namespace Identity.Domain.Entities;

public class ServiceResult<T>
{
    public T Value { get; set; }
    public bool Succeeded => !Errors.Any();
    public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
}