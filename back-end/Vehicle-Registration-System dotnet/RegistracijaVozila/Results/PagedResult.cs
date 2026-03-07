namespace VehicleRegistrationSystem.Results
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];

        public int TotalCount { get; set; }
    }
}
