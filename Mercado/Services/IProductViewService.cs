

namespace Mercado.Services
{
    public interface IProductViewService
    {
        void IncrementViewCount(int productId);
    }
}