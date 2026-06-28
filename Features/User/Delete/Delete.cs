using bank.Entity;
using bank.Exceptions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace bank.Features.User.Delete;

public record DeleteUserRequest(long Id);

public class Delete(AppDbContext context) : Endpoint<DeleteUserRequest>
{
    public override void Configure()
    {
        Delete("/user/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var account = await context.Accounts.FindAsync(new object[] { req.Id }, ct);

        if (account is null)
        {
            throw new BusinessException(new List<ApiError>
            {
                new("Id", "Người dùng không tồn tại")
            });
        }

        context.Accounts.Remove(account);
        await context.SaveChangesAsync(ct);

        await SendAsync(ApiResponse<string>.SuccessResponse("Xóa thành công"), 200, ct);
    }
}
