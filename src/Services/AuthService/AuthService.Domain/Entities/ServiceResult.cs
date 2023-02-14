namespace AuthService.Domain.Entities
{
    public class ServiceResult<T>
    {
        public T Value { get; set; }
        public bool Succeeded { get; set; } = true;
        public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
    }
}