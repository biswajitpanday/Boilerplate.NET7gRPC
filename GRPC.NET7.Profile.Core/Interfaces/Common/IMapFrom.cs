using AutoMapperProfile = AutoMapper.Profile;

namespace GRPC.NET7.Profile.Core.Interfaces.Common;

public interface IMapFrom<T>
{
    void Mapping(AutoMapperProfile profile)
    {
        profile.CreateMap(typeof(T), GetType());
        profile.CreateMap(GetType(), typeof(T));
    }
}