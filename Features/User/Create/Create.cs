using bank.Entity;
using bank.Exceptions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace bank.Features.User.Create;

public class Create(AppDbContext context) : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post("/user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        if (await context.Accounts.AnyAsync(a => a.Email == req.Gmail, ct))
        {
            throw new BusinessException(new List<ApiError>
            {
                new("Gmail", "Gmail đã tồn tại")
            });
        }

        var newAccount = new Account { Name = req.Name, Email = req.Gmail };
        context.Accounts.Add(newAccount);
        await context.SaveChangesAsync(ct);

        var response = ApiResponse<string>.SuccessResponse("Thêm thành công");
        await SendAsync(response, 201, ct);
    }
}
