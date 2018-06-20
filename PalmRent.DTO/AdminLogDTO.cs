using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.DTO
{
    public class AdminLogDTO:BaseDTO
    {
        public long AdminUserId { get; set; }
        public String Message { get; set; }
        public String AdminUserName { get; set; }
        public String AdminUserPhoneNum { get; set; }
    }
}
