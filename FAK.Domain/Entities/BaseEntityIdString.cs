using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public abstract class BaseEntityIdString : BaseEntity
    {
        public new string Id { get; set; }
    }
}
