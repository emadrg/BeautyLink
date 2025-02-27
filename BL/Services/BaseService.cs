using BL.UnitOfWork;
using Common.AppSettings;
using Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BL.Services
{
    public class BaseService : IDisposable, IService
    {
        protected AppUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        protected IAppSettings AppSettings { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        /// <param name="appSettings"></param>
        public BaseService(AppUnitOfWork unitOfWork, ILogger logger = null!, IAppSettings appSettings = null!)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
            AppSettings = appSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UnitOfWork != null)
                {
                    UnitOfWork.Dispose();
                    UnitOfWork = null!;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await UnitOfWork.SaveChanges();
        }
    }
}
