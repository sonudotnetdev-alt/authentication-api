using System.Runtime.Serialization;

namespace Authentication.Core.Entities.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        pending,
        [EnumMember(Value ="Payment Recieved")]
        PaymentRecieved,
        [EnumMember(Value ="PaymentFailed")]
        paymentFailded,
    }
}