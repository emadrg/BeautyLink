using BL.UnitOfWork;
using DA.Entities;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Utils;

namespace BL.Logger
{
    public class Logger : ILogger
    {
        private readonly AppUnitOfWork UnitOfWork;
        private readonly ClaimsPrincipal CurrentUser;

        public Logger(AppUnitOfWork unitOfWork, ClaimsPrincipal currentUser) 
        {
            UnitOfWork = unitOfWork;
            CurrentUser = currentUser;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = String.IsNullOrEmpty(exception?.Message) ? null : exception.Message;
            var stackTrace = String.IsNullOrEmpty(exception?.StackTrace) ? null : exception.StackTrace;
            var error = new Log
            {
                CreatedDate = DateTime.Now,
                ErrorMessage = message ?? string.Empty,
                StackTrace = stackTrace ?? string.Empty,
                CreatedBy = CurrentUser!.Identity!.IsAuthenticated ? CurrentUser.Id() : null,
                LogLevel = (byte)logLevel
            };
            UnitOfWork.Repository<Log>().Add(error);
            UnitOfWork.SaveChangesSync();
        }

        public async Task LogError(List<string> exMessages) => await LogError(String.Join('\n', exMessages));
        public async Task LogError(string exMessage) => await LogError(new Exception(exMessage));
        public async Task LogError(Exception e)
        {
            if (e.InnerException != null)
            {
                await LogError(e.InnerException);
            }
            var message = String.IsNullOrEmpty(e.Message) ? null : e.Message;
            var stackTrace = String.IsNullOrEmpty(e.StackTrace) ? null : e.StackTrace;
            var error = new Log
            {
                CreatedDate = DateTime.Now,
                ErrorMessage = message ?? string.Empty,
                StackTrace = stackTrace ?? string.Empty,
                CreatedBy = CurrentUser!.Identity!.IsAuthenticated ? CurrentUser.Id() : null,
                LogLevel = (byte)LogLevel.Error
            };
            UnitOfWork.Repository<Log>().Add(error);
            await UnitOfWork.SaveChanges();
        }
    }
}
