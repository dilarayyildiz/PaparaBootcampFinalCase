using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record CreateAuthorizationTokenCommand(AuthorizationRequest Request) : IRequest<ApiResponse<AuthorizationResponse>>;