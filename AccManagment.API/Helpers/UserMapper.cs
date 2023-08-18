using AccManagment.API.Entities;
using AccManagment.API.Modules;
using AutoMapper;

namespace AccManagment.API.Helpers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserForm>().ReverseMap();
        CreateMap<User, UserRegistration>().ReverseMap();
    }
}