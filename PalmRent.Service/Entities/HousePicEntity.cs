using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service.Entities
{
    /// <summary>
    /// 房屋照片
    /// </summary>
    public class HousePicEntity:BaseEntity
    {
        public long HouseId { get; set; }
        public virtual HouseEntity House { get; set; }
        public string Url { get; set; }
        public string ThumbUrl { get; set; }
    }
}
