using PalmRent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.IService
{
    public interface IAttachmentService: IServiceSupport
    {
        //获取所有的设施
        AttachmentDTO[] GetAll();

        //获取房子houseId有用的设施
        AttachmentDTO[] GetAttachments(long houseId);
    }
}
