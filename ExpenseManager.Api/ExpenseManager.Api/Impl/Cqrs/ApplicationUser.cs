using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace ExpenseManager.Api.Impl.Cqrs;

public record CreateApplicationUserAuthorizationTokenCommand(AuthorizationRequest Request) : IRequest<ApiResponse<AuthorizationResponse>>;
public record LogoutApplicationUserAuthorizationTokenCommand() : IRequest<ApiResponse>;
public record ChangeApplicationUserPasswordCommand(ChangePasswordRequest Request) : IRequest<ApiResponse>;
public record CreateApplicationUserCommand(UserRequest Request) : IRequest<ApiResponse>;
public record SendApplicationUserPasswordResetEmailCommand(string UserName) : IRequest<ApiResponse<string>>;
public record ResetApplicationUserPasswordCommand(ResetPasswordRequest Request) : IRequest<ApiResponse>;