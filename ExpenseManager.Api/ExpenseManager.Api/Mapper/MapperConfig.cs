using ExpenseManager.Api.Request;
using ExpenseManager.Schema;

namespace ExpenseManager.Api.Mapper;

using AutoMapper;
using ExpenseManager.Api.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        // User mapping
        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // hash handler/service içinde basılacak
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));

        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        
        // Expense mapping
        CreateMap<ExpenseRequest, Expense>();
        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ExpenseStatus, opt => opt.MapFrom(src => src.ExpenseStatus.ToString()));
        
        CreateMap<CreateExpenseRequest, Expense>()
            .ForMember(dest => dest.ExpenseStatus, opt => opt.MapFrom(src => ExpenseStatus.Pending)) // çünkü enum string geliyor
            .ForMember(dest => dest.ReceiptUrl, opt => opt.Ignore()) // bu dosya işlemi handler’da yapılacak
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id DB tarafından atanacak

        
        // ExpenseCategory mapping
        CreateMap<ExpenseCategoryRequest, ExpenseCategory>();
        CreateMap<ExpenseCategory, ExpenseCategoryResponse>();
        
        // AccountHistory mapping
        CreateMap<AccountHistory, AccountHistoryResponse>();
        
        //ApplicationUser mapping
        //CreateMap<ApplicationUserRequest, ApplicationUser>();
        //CreateMap<ApplicationUser, ApplicationUserResponse>();
    }
}


   // Ne Zaman ForMember Kullanırsın?
   //  Entity’den gelen ilişkisel veriyi (örn. bağlı user, bağlı category) response’a geçirmek istediğinde.
  //  Enum veya karmaşık tipleri düz string veya sayıya dönüştürmek istediğinde.
  //  Alan isimleri request/response ile entity arasında farklı olduğunda.
