using Common.Interfaces;

namespace DTOs.Query
{
    public class QueryDTO<TFilterPart>: IQueryBuilder where TFilterPart : IFilterPartItem, new()
    {
        public QueryDTO() 
        {
            Filters = new List<TFilterPart>();
            Paging = new PagingPartDTO();
            Sorting = new SortingPartDTO();
        }
        public IPagingPartBuilder Paging { get; set; }
        public ISortingPartBuilder Sorting { get; set; }
        public List<TFilterPart> Filters { get; set; }

        public string GetSQLFilterCondition()
        {
            if(Filters == null)
            {
                return String.Empty;
            }
            return String.Join(" AND ", Filters.Select(f => f.GetSQLCondition()));
        }
    }
    public class QueryDTO<TFilterPart1, TFilterPart2> : IQueryBuilder
        where TFilterPart1: IFilterPartItem, new() 
        where TFilterPart2: IFilterPartItem, new()
    {
        public QueryDTO()
        {
            Filters = new Tuple<List<TFilterPart1>, List<TFilterPart2>>(new List<TFilterPart1>(), new List<TFilterPart2>());
        }
        public IPagingPartBuilder Paging { get; set; } = new PagingPartDTO();
        public ISortingPartBuilder Sorting { get; set; } = new SortingPartDTO();
        public Tuple<List<TFilterPart1>, List<TFilterPart2>> Filters { get; set; }

        public string GetSQLFilterCondition()
        {
            if (Filters == null)
            {
                return String.Empty;
            }
            return String.Join(" AND ", Filters.Item1.Select(f => f.GetSQLCondition()).Union(Filters.Item2.Select(f => f.GetSQLCondition())));
        }
    }

    public class QueryDTO<TFilterPart1, TFilterPart2, TFilterPart3> : IQueryBuilder
        where TFilterPart1: IFilterPartItem, new() 
        where TFilterPart2: IFilterPartItem, new()
        where TFilterPart3: IFilterPartItem, new()
    {
        public QueryDTO()
        {
            Filters = new Tuple<List<TFilterPart1>, List<TFilterPart2>, List<TFilterPart3>>(new List<TFilterPart1>(), new List<TFilterPart2>(), new List<TFilterPart3>());
        }
        public IPagingPartBuilder Paging { get; set; } = new PagingPartDTO();
        public ISortingPartBuilder Sorting { get; set; } = new SortingPartDTO();
        public Tuple<List<TFilterPart1>, List<TFilterPart2>, List<TFilterPart3>> Filters { get; set; }

        public string GetSQLFilterCondition()
        {
            if (Filters == null)
            {
                return String.Empty;
            }
            return String.Join(" AND ", Filters.Item1.Select(f => f.GetSQLCondition()).Union(Filters.Item2.Select(f => f.GetSQLCondition())).Union(Filters.Item3.Select(f => f.GetSQLCondition())));
        }
    }
}
