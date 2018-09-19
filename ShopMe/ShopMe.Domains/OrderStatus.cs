namespace ShopMe.Domains
{
    public enum OrderStatus
    {
        Pending = 10,

        Processing = 20,

        Unconfirmed = 30,

        Confirmed = 40,

        Complete = 50,

        Cancelled = 60,

        HasShipped = 70,

        Packed = 80
    }
}