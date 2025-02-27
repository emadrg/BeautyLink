using Common.Interfaces;

namespace DTOs.Query
{
    public class InValueListFilterDTO : IFilterPartItem
    {
        public InValueListFilterDTO()
        {
            Values = new List<string>();
        }

        public string PropertyDBName { get; set; } = null!;
        public List<string> Values { get; set; }

        public string GetSQLCondition()
        {
            return $"(CAST({PropertyDBName} AS nvarchar) in ('{String.Join("','", Values)}'))";
        }
    }

    public class LikeValueFilterDTO : IFilterPartItem
    {
        public string PropertyDBName { get; set; } = null!;
        public string Value { get; set; } = null!;

        public string GetSQLCondition()
        {
            return $"(CAST({PropertyDBName} AS nvarchar) like '%{Value}%')";
        }
    }

    public class BetweenValuesFilterDTO : IFilterPartItem
    {
        public string PropertyDBName { get; set; } = null!;
        public string GreaterThanValue { get; set; } = null!;
        public string LessThanValue { get; set; } = null!;

        public string GetSQLCondition()
        {
            return $"({PropertyDBName} > '{GreaterThanValue}' and {PropertyDBName} < '{LessThanValue}')";
        }
    }

}
