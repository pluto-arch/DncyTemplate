using System.Collections.Immutable;

namespace DncyTemplate.Uow
{
    public interface IUnitOfWorkAccessor
    {
        void SetUnitOfWork(IUnitOfWork unitOfWork);
    }
    
    public interface IUnitOfWorkAccessor<TContext> : IUnitOfWorkAccessor
        where TContext : IDataContext
    {
        IUnitOfWork<TContext> UnitOfWork { get; }

        void SetUnitOfWork(IUnitOfWork<TContext> unitOfWork);
    }


    public class UnitOfWorkAccessor<TContext> : IUnitOfWorkAccessor<TContext>
        where TContext : IDataContext
    {
        /// <inheritdoc />
        public IUnitOfWork<TContext> UnitOfWork  => _currentUow.Value;
        
        private readonly AsyncLocal<IUnitOfWork<TContext>> _currentUow;
        
        public UnitOfWorkAccessor()
        {
            _currentUow = new AsyncLocal<IUnitOfWork<TContext>>();
        }
        

        /// <inheritdoc />
        public void SetUnitOfWork(IUnitOfWork<TContext> unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork) 
        {
            if (unitOfWork is IUnitOfWork<TContext> uow)
            {
                SetUnitOfWork(uow);
            }
        }
    }


    public static class UnitWorkAccessorMap
    {
        private static Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();
        public static ImmutableDictionary<Type, Type> UowWithAccessorMap => _cache.ToImmutableDictionary();

        public static void Add(Type accessorType,Type uowType)
        {
            _cache.TryAdd(accessorType, uowType);
        }
    }
}