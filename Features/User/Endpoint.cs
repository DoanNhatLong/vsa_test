using FastEndpoints;

public class MyEndpoint : Endpoint<MyRequest, MyResponse>
{
    public override void Configure()
    {
        Post("user/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MyRequest req, CancellationToken ct)
    {
        var response = new MyResponse
        (
            $"{req.FirstName} {req.LastName}",
            req.Age > 18

        );
        await SendAsync(response, 200, ct);
    }
}
