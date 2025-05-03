using AutoMapper;
using ExpenseManager.Base;

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
    
    
    
    
}