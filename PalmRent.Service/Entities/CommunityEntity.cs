using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service.Entities
{
    /// <summary>
    /// 小区
    /// </summary>
    public class CommunityEntity:BaseEntity
    {
        public string Name { get; set; }
        public long RegionId { set; get; }

        public virtual RegionEntity Region { set; get; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 交通
        /// </summary>
        public string Traffic { get; set; }
        /// <summary>
        /// 建造年份
        /// </summary>
        public int? BuiltYear { get; set; }
    }
}
