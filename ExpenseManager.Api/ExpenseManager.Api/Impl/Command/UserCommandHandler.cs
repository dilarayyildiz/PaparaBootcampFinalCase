using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Base.Encryption;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;

namespace ExpenseManager.Api.Impl.Command;

public class UserCommandHandler
{
    private readonly ExpenseManagerDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IAppSession appSession;
    
    public  UserCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper, IAppSession appSession)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.appSession = appSession;
    }
    
    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == request.User.Email, cancellationToken);
        if (existing != null)
            return new ApiResponse<UserResponse>("This email is already registered");

        
        var user = mapper.Map<User>(request.User);
        
        user.Secret = Guid.NewGuid().ToString();
        user.PasswordHash = PasswordGenerator.CreateMD5(request.User.Password, user.Secret);
        user.IsActive = true;  
        
        await dbContext.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<UserResponse>(user);
        return new ApiResponse<UserResponse>(response);
        
        //TESTLERDEN SONRA BASARILI CALISIRSA ALTI SIL!!!
        //var entity = await dbContext.AddAsync(user, cancellationToken);
        //await dbContext.SaveChangesAsync(cancellationToken);
        //var response = mapper.Map<UserResponse>(entity.Entity);

        //return new ApiResponse<UserResponse>(response);
    }
   
    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null)
            return new ApiResponse("User not found");

        if (!user.IsActive)
            return new ApiResponse("User is not active");

        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;
        user.Phone = request.User.Phone;
        user.Role = Enum.Parse<UserRole>(request.User.Role);
        
        //Iban update olursa bankacılık bozulur DEGERLENDIR
        //user.IBAN = request.User.IBAN; 
        dbContext.Set<User>().Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        //TESTLER SONRASI BAKILACAK
        //await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null)
            return new ApiResponse("User not found");

        if (!user.IsActive)
            return new ApiResponse("User already deleted");

        //Soft delete yapıyoruz..
        user.IsActive = false;

        dbContext.Set<User>().Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        //TESTLER SONRASI BAKILACAK
        //await dbContext.SaveChangesAsync(cancellationToken);
        
        return new ApiResponse();
    }

   
}
