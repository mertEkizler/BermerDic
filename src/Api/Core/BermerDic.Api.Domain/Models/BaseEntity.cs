namespace BermerDic.Api.Domain.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
    }
}