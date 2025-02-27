using Common.Enums;

namespace Common.Interfaces
{
    public interface IQueryBuilder
    {
        IPagingPartBuilder Paging { get; set; }
        ISortingPartBuilder Sorting { get; set; }

        string GetSQLFilterCondition();
    }
    public interface IPagingPartBuilder
    {
        int Take { get; set; }
        int Skip { get; set; }
    }
    public interface ISortingPartBuilder
    {
        string? SortBy { get; set; }
        SortDirections SortDirection { get; set; }
    }
    public interface IFilterPartItem
    {
        string PropertyDBName { get; set; }
        string GetSQLCondition();
    }
}
