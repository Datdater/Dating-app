namespace HumanShop.Extensions
{
    public static class DatetimeExtentions
    {
        public static int CalculateAge(this DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - date.Year;
            return age;
        }
    }
}
