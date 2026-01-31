using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record RatingEntityId : NotEmptyGuid
    {
        public RatingEntityId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(RatingEntityId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator RatingEntityId(Guid value) => value == default ? null : new RatingEntityId(value);
    }

}

