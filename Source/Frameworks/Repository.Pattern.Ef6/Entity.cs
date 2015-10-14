using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Ef6
{
    public abstract partial class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}