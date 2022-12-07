using AutoMapper;

namespace GRPC.NET7.Core.Interfaces.Common;

internal interface IMapFrom<T>
{
    internal void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType()).ReverseMap();
    }
}