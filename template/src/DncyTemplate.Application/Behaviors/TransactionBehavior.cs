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

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string typeName = request?.GetGenericTypeName();
            TResponse response = default;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    response = await next();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                    throw;
                }
            }

            return response;
        }
    }
}
