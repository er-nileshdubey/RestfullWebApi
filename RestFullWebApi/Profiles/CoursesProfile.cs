using AutoMapper;

namespace RestFullWebApi.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, Models.CoursesDto>();
            CreateMap<Models.CourseForCreationDto, Entities.Course>();
            CreateMap<Models.CourseForUpdationDto, Entities.Course>().ReverseMap();
        }
    }
}
