namespace Benefits.BusinessLogic
{
    public interface IDiscountHandler
    {
        bool EligibleForDiscount(string name);
    }
}