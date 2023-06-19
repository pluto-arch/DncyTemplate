using DncyTemplate.Application.Command;
using DncyTemplate.Infra.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Transactions;

namespace DncyTemplate.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;


        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? NullLogger<TransactionBehavior<TRequest, TResponse>>.Instance;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string typeName = request?.GetGenericTypeName();
            TResponse response = default;
            try
            {
                bool isTran = false;
                if (request is ICommand cmd)
                {
                    isTran = cmd.Transactional;
                }

                if (isTran)
                {
                    response = await ExecuteTransactional(next);
                }
                else
                {
                    response = await Execute(next);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling [{CommandName}] ({@Command})", typeName, request);
                throw;
            }

            return response;
        }


        private static async Task<TResponse> ExecuteTransactional(RequestHandlerDelegate<TResponse> next)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
            var response = await next();
            scope.Complete();
            return response;
        }



        private static async Task<TResponse> Execute(RequestHandlerDelegate<TResponse> next)
        {
            return await next();
        }
    }
}
