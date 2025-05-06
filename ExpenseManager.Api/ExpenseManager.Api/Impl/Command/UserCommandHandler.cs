using AutoMapper;
using MediatR;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Impl.UnitOfWork;
using ExpenseManager.Base;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Base.Encryption;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;

namespace ExpenseManager.Api.Impl.Command;

public class UserCommandHandler:
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>,
    IRequestHandler<ChangeUserPasswordCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly IAppSession appSession;
    
    public  UserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAppSession appSession)
    {
        this.unitOfWork = unitOfWork;
        this._mapper = mapper;
        this.appSession = appSession;
        _httpContextAccessor = new HttpContextAccessor();
    }
    
    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == request.User.Email);
        if (existing != null)
            return new ApiResponse<UserResponse>("This email is already registered");

        
        var user = _mapper.Map<User>(request.User);
        
        user.Secret = Guid.NewGuid().ToString();
        user.PasswordHash = PasswordGenerator.CreateMD5(request.User.Password, user.Secret);
        user.IsActive = true;  
        
        await unitOfWork.UserRepository.AddAsync(user, cancellationToken); 
        await unitOfWork.Complete(cancellationToken);

        var response = _mapper.Map<UserResponse>(user);
        return new ApiResponse<UserResponse>(response);
         
    }
   
    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user == null)
            return new ApiResponse("User not found");

        if (!user.IsActive)
            return new ApiResponse("User is not active");

        user.Secret = Guid.NewGuid().ToString();
        user.PasswordHash = PasswordGenerator.CreateMD5(request.User.Password, user.Secret); 
        
        //Iban update olursa bankacılık bozulur DEGERLENDIR
        //user.IBAN = request.User.IBAN; 
        unitOfWork.UserRepository.Update(user);
        await unitOfWork.Complete(cancellationToken);
         
        return new ApiResponse();
    }
    public async Task<ApiResponse> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new ApiResponse("Unauthorized or missing UserId claim");
        } 
        var userId = int.Parse(userIdClaim);
        
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == request.User.email);
        
        if (user == null)
            return new ApiResponse("User not found");

        if (!user.IsActive)
            return new ApiResponse("User is not active");

        if (userId != user.Id)
            return new ApiResponse("You are not authorized to change this user's password");
        
 
        user.Secret = Guid.NewGuid().ToString();
        user.PasswordHash = PasswordGenerator.CreateMD5(request.User.Password, user.Secret); 
         
        unitOfWork.UserRepository.Update(user);
        await unitOfWork.Complete(cancellationToken);
        
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user == null)
            return new ApiResponse("User not found");

        if (!user.IsActive)
            return new ApiResponse("User already deleted");

        //Soft delete yapıyoruz..
        user.IsActive = false;

        unitOfWork.UserRepository.Update(user);
        await unitOfWork.Complete(cancellationToken);
          
        return new ApiResponse();
    }

   
}
