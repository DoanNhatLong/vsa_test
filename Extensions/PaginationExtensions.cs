using Microsoft.EntityFrameworkCore;
using bank.Entity;

namespace bank.Extensions;

public record PagedResult<T>(List<T> Items, PaginationInfo Pagination);

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int page = 1,
        int pageSize = 4,
        CancellationToken ct = default)
    {
        var totalItems = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(x => EF.Property<long>(x!, "Id"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagination = new PaginationInfo(
            Page: page,
            PageSize: pageSize,
            TotalPages: totalPages,
            TotalItems: totalItems,
            HasNext: page < totalPages,
            HasPrevious: page > 1
        );

        return new PagedResult<T>(items, pagination);
    }
}
