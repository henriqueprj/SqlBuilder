namespace rahhh.SqlBuilder.Abstractions
{
    public interface IPaginatedQuery
    {
        int? PageSize { get; }
        int? PageNumber { get; }
    }
}