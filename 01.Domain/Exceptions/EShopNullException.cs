using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopNullException : EShopException
    {
        public object? Par { get; set; }
        public EShopNullException(string message = $"{nameof(Par)} is null.") : base(message)
        {

        }
    }
}
