using AutoMapper;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.DomainEvents.Product;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.Utils;
using ProductAgg = DncyTemplate.Domain.Aggregates.Product;


namespace DncyTemplate.Application.Command.Product
{
    [AutoResolveDependency]
    public partial class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        [AutoInject]
        private readonly IMapper _mapper;
        [AutoInject]
        private readonly ILogger<CreateProductCommandHandler> _logger;

        [AutoInject]
        private readonly EfUnitOfWork<DncyTemplateDbContext> efUow;

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var repository = efUow.EfRepository<DncyTemplate.Domain.Aggregates.Product.Product>();
            var entity = _mapper.Map<ProductAgg.Product>(request);
            entity.Id = SnowFlakeId.Generator.GetUniqueId();
            entity.CreationTime = DateTimeOffset.Now;
            entity.AddDomainEvent(new NewProductCreateDomainEvent(entity));
            entity = await repository.InsertAsync(entity, cancellationToken: cancellationToken);
            await efUow.CompleteAsync(cancellationToken);
            return _mapper.Map<ProductDto>(entity);
        }
    }
}
