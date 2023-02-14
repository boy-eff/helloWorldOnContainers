namespace AuthService.Domain.Entities
{
    public class ServiceResult<T>
    {
        public T Value { get; set; }
        public bool Succeeded { 
            get { return !Errors.Any(); }
        }
        public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
    }
}