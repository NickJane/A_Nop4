using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nop.Services.Helpers
{
    /// <summary>
    /// Setting service interface
    /// </summary>
    public partial interface ISettingService
    {
        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="storeId">Store identifier for which settigns should be loaded</param>
        T LoadSetting<T>() where T : ISettings, new();
    }
}
