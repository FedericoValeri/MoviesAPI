using MoviesAPI.Models.DTOs;

namespace MoviesAPI.Customizations.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> query,
            PaginationDTO paginationDTO)
        {
            return query
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage);
        }
    }
}