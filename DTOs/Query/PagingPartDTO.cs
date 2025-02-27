using Common.Interfaces;

namespace DTOs.Query
{
    public class PagingPartDTO: IPagingPartBuilder
    {
        public int Take { get; set; }
        public int Skip { get; set; } = 0;
    }
}
