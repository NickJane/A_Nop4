
using Nop.Data.Domain.Localization;
using Nop.Data.Domain.Site;
using Nop.Services.Customer; 

namespace Nop.Services
{
    public interface IWorkContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        CustomerInfo CurrentCustomer { get; set; }
        
        /// <summary>
        /// 运行时语言环境
        /// </summary>
        Language RunTimeLanguage { get; set; }

        
        /// <summary>
        /// 运行时设备环境(预览和发布状态)
        /// </summary>
        DeviceMode RunTimeDeviceMode { get; set; }

        int CurrentSiteId { get;  }
    }



    public enum DeviceMode
    {
        PC=1,
        Mobile=2
    }
}
