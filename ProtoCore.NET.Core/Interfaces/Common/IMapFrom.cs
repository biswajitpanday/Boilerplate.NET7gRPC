using AutoMapper;

namespace ProtoCore.NET.Core.Interfaces.Common;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
        profile.CreateMap(GetType(), typeof(T));
    }
}