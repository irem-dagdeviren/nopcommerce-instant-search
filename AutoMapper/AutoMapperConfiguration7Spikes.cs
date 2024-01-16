using AutoMapper;

namespace Nop.Plugin.InstantSearch.AutoMapper
{
  public static class AutoMapperConfiguration7Spikes
  {
    private static MapperConfigurationExpression _mapperConfigurationExpression;
    private static IMapper _mapper;
    private static readonly object mapperConfigurationExpressionLockObject = new object();
    private static readonly object mapperLockObject = new object();

    public static MapperConfigurationExpression MapperConfigurationExpression
    {
      get
      {
        if (AutoMapperConfiguration7Spikes._mapperConfigurationExpression == null)
        {
          lock (AutoMapperConfiguration7Spikes.mapperConfigurationExpressionLockObject)
          {
            if (AutoMapperConfiguration7Spikes._mapperConfigurationExpression == null)
              AutoMapperConfiguration7Spikes._mapperConfigurationExpression = new MapperConfigurationExpression();
          }
        }
        return AutoMapperConfiguration7Spikes._mapperConfigurationExpression;
      }
    }

    public static IMapper Mapper
    {
      get
      {
        if (AutoMapperConfiguration7Spikes._mapper == null)
        {
          lock (AutoMapperConfiguration7Spikes.mapperLockObject)
          {
            if (AutoMapperConfiguration7Spikes._mapper == null)
              AutoMapperConfiguration7Spikes._mapper = new MapperConfiguration(AutoMapperConfiguration7Spikes.MapperConfigurationExpression).CreateMapper();
          }
        }
        return AutoMapperConfiguration7Spikes._mapper;
      }
    }

    public static TDestination MapTo<TSource, TDestination>(this TSource source) => AutoMapperConfiguration7Spikes.Mapper.Map<TSource, TDestination>(source);

    public static TDestination MapTo<TSource, TDestination>(
      this TSource source,
      TDestination destination)
    {
      return AutoMapperConfiguration7Spikes.Mapper.Map<TSource, TDestination>(source, destination);
    }
  }
}
