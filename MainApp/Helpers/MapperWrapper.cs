//using AutoMapper;
//using MainApp.ViewModels;
//using Models;

//namespace MainApp.Helpers
//{
//    public static class MapperWrapper
//    {
//        public static void Initialize()
//        {
//            var config = new MapperConfiguration(cfg => cfg.CreateMap<Card, CardViewModel>()
//                    .ForMember("Id", opt => opt.MapFrom(c => c.Id + " " + c.Id))
//                    .ForMember("Name", opt => opt.MapFrom(src => src.Name))
//                    .ForMember("Name", opt => opt.Ignore()));

//            Mapper = new Mapper(config);
//        }

//        public static Mapper Mapper
//        {
//            get; private set;
//        }
//    }
//}
