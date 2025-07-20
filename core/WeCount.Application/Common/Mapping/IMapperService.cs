namespace WeCount.Application.Common.Mapping
{
    public interface IMapperService
    {
        TDestination Map<TDestination>(object source);
    }
}
