using AutoMapper;
using Moq;

namespace Venice.Orders.Tests.Builders;

public class AutoMapperBuilder
{
    private readonly Mock<IMapper> _mapperMock;

    public AutoMapperBuilder()
    {
        _mapperMock = new Mock<IMapper>();
    }

    public AutoMapperBuilder WithMap<TSource, TDestination>(TSource source, TDestination destination)
    {
        _mapperMock.Setup(m => m.Map<TDestination>(It.Is<TSource>(s => ReferenceEquals(s, source) || s!.Equals(source))))
            .Returns(destination);
        return this;
    }

    public AutoMapperBuilder WithMap<TSource, TDestination>(TDestination destination)
    {
        _mapperMock.Setup(m => m.Map<TDestination>(It.IsAny<TSource>()))
            .Returns(destination);
        return this;
    }

    public AutoMapperBuilder WithMapCollection<TSource, TDestination>(IEnumerable<TDestination> destination)
    {
        _mapperMock.Setup(m => m.Map<IEnumerable<TDestination>>(It.IsAny<IEnumerable<TSource>>()))
            .Returns(destination);
        return this;
    }

    public AutoMapperBuilder WithMapList<TSource, TDestination>(List<TDestination> destination)
    {
        _mapperMock.Setup(m => m.Map<List<TDestination>>(It.IsAny<IEnumerable<TSource>>()))
            .Returns(destination);
        return this;
    }

    public IMapper Build()
    {
        return _mapperMock.Object;
    }

    public Mock<IMapper> BuildMock()
    {
        return _mapperMock;
    }
}

