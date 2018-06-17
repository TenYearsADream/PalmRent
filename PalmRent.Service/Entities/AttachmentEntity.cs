using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service.Entities
{
    /// <summary>
    /// 房屋配套设施
    /// </summary>
    public class AttachmentEntity:BaseEntity
    {
        /// <summary>
        /// 配套设施名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 配套设施图标
        /// </summary>
        public string IconName { get; set; }

        public virtual ICollection<HouseEntity> Houses { get; set; } = new List<HouseEntity>();
    }
}
