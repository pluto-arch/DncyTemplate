namespace DncyTemplate.Uow
{
    public interface IUnitOfWorkAccessor
    {
        IUnitOfWork UnitOfWork { get; }

        void SetUnitOfWork(IUnitOfWork unitOfWork);
    }


    public class UnitOfWorkAccessor : IUnitOfWorkAccessor
    {
        /// <inheritdoc />
        public IUnitOfWork UnitOfWork  => _currentUow.Value;
        
        private readonly AsyncLocal<IUnitOfWork> _currentUow;
        
        public UnitOfWorkAccessor()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }
        

        /// <inheritdoc />
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }
    }
}