using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service.Entities
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class IdNameEntity:BaseEntity
    {
        public string TypeName { get; set; }
        public string Name { get; set; }
    }
}
