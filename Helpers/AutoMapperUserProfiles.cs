using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;
#nullable disable
public class AutoMapperUserProfiles : Profile
{
    public AutoMapperUserProfiles()
    {
        CreateMap<AppUser, MemberDto>()
        .ForMember(
            user => user.Age,
            opt => opt.MapFrom(
                user => user.BirthDate.CalculateAge()
            )
        )
        .ForMember(
                user => user.MainPhotoUrl,
                opt => opt.MapFrom(
                    user => user.Photos.FirstOrDefault(photo => photo.IsMain).Url
                    )
                );
        CreateMap<Photo, PhotoDto>();
    }
}