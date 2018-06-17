using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service.Entities
{
    /// <summary>
    /// 区/县
    /// </summary>
    public class RegionEntity:BaseEntity
    {
        public string Name { get; set; }
        public long CityId { get; set; }

        public virtual CityEntity City { get; set; }
    }
}
