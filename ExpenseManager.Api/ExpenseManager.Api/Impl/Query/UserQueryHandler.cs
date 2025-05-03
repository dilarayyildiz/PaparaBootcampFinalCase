using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Api.Impl.Query;

public class UserQueryHandler :
    IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserResponse>>>,
    IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
{
    private readonly ExpenseManagerDbContext context;
    private readonly IMapper mapper;
    public UserQueryHandler(ExpenseManagerDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var Users = await context.Set<User>().ToListAsync(cancellationToken);

        var mapped = mapper.Map<List<UserResponse>>(Users);
        return new ApiResponse<List<UserResponse>>(mapped);
    }

    public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<User>(true);
        predicate = predicate.And(x => x.Id == request.Id);

        var User = await context.Set<User>().FirstOrDefaultAsync(predicate, cancellationToken);

        var mapped = mapper.Map<UserResponse>(User);
        return new ApiResponse<UserResponse>(mapped);
    }
    
    // predicate (şart zinciri) oluşturuyor: User.Id == request.Id.
}