using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Services.Token;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Base.Encryption;
using ExpenseManager.Base.Token;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Command;

public class AuthorizationCommandHandler :
    IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
{
    private readonly ExpenseManagerDbContext dbContext;
    private readonly ITokenService tokenService;
    private readonly JwtConfig jwtConfig;

    public AuthorizationCommandHandler(ExpenseManagerDbContext dbContext, ITokenService tokenService, JwtConfig jwtConfig)
    {
        this.jwtConfig = jwtConfig;
        this.dbContext = dbContext;
        this.tokenService = tokenService;
    }
    public async Task<ApiResponse<AuthorizationResponse>> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == request.Request.Email, cancellationToken);
        if (user == null)
            return new ApiResponse<AuthorizationResponse>("Email is incorrect");

        var hashedPassword = PasswordGenerator.CreateMD5(request.Request.Password, user.Secret);
        if (hashedPassword != user.PasswordHash)
            return new ApiResponse<AuthorizationResponse>("Password is incorrect");

        var token = tokenService.GenerateToken(user);
        var entity = new AuthorizationResponse
        {
            UserName = user.Name + " " + user.Surname,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(jwtConfig.AccessTokenExpiration)
        };

        return new ApiResponse<AuthorizationResponse>(entity);
    }
}