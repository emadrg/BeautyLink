using Common.Enums;
using Common.Interfaces;

namespace DTOs.Query
{
    public class SortingPartDTO: ISortingPartBuilder
    {
        public string? SortBy { get; set; }
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
    }
}
