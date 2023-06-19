using System.Transactions;
using DncyTemplate.Application.Command;
using DncyTemplate.Infra.Extensions;
using Microsoft.Extensions.Logging.Abstractions;

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
                bool isTran=false;
                if (request is ICommand cmd)
                {
                    isTran = cmd.Transactional;
                }

                if (isTran)
                {
                    response = await ExecuteTransactional(request,next, cancellationToken);
                }
                else
                {
                    response = await ExecuteTransactional(request,next, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling [{CommandName}] ({@Command})", typeName, request);
                throw;
            }

            return response;
        }


        private async Task<TResponse> ExecuteTransactional(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = default;
            using var scope = new TransactionScope(
                TransactionScopeOption.Required, 
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, 
                TransactionScopeAsyncFlowOption.Enabled);
            response = await next();
            scope.Complete();
            return response;
        }


       
        private async Task<TResponse> Execute(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return  await next();
        }
    }
}
