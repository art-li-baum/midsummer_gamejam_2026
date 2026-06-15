using Gorpozon.WarehouseSim.Data;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class ProductElement : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private TextMeshProUGUI amountText;
		[SerializeField] private TextMeshProUGUI priceText;
		[SerializeField] private TextMeshProUGUI descriptionText;

		public void Setup(ShippingOrder.ProductQuantity productData)
		{
			nameText.text = productData.Product.DisplayName;
			amountText.text = $"Quantity: {productData.Amount:00}";
			priceText.text = $"$ {productData.Product.Price:0.00}";
			descriptionText.text = productData.Product.Description;
		}
    }
}