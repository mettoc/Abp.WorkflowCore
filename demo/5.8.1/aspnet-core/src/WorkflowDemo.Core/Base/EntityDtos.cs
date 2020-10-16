using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Application.Services.Dto
{
    public class EntityDtos<T>
    {
        public IEnumerable<T> Ids { get; set; }
    }
}
