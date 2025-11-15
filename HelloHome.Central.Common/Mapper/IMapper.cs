namespace HelloHome.Central.Common.Mapper;

public interface IMapper<in TF, out TT>
{
    TT Map(TF source);
}
