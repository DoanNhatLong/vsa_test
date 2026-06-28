using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using bank.Entity;
using bank.Extensions;

namespace bank.Features.User;

public record UserResponse(long Id, string FullName);
public record GetUsersRequest(int Page = 1);

public class GetAll(
    AppDbContext context
    ) : Endpoint<GetUsersRequest, ApiResponse<List<UserResponse>>>
{
    public override void Configure()
    {
        Get("/user");
        AllowAnonymous();

    }

    public override async Task HandleAsync(
        GetUsersRequest request, CancellationToken ct)
    {
        var pagedData = await context.Accounts.ToPagedListAsync(request.Page, ct: ct);

        var users = pagedData.Items
            .Select(a => new UserResponse(a.Id, a.Name))
            .ToList();

        var response = ApiResponse<List<UserResponse>>.SuccessResponse(
            users,
            "Lấy danh sách người dùng thành công",
            pagedData.Pagination
        );

        await SendAsync(response, 200, ct);


    }

}
