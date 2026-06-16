using Gorpozon.WarehouseSim.Data;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class OrderScreen: MonoBehaviour
	{
		[SerializeField] private ShippingOrder testOrder;

		[SerializeField] private RectTransform orderParent;
		[SerializeField] private ProductElement productElementPrefab;
		[SerializeField] private TextMeshProUGUI totalPriceText;

        private void Start()
        {
            ShowNextOrder(testOrder);
        }

        public void ShowNextOrder(ShippingOrder order)
		{
			// Clear previous entries
			for (int i = orderParent.childCount-1; i >= 0; i--)
			{
				Destroy(orderParent.GetChild(i).gameObject);
			}

			// Populate new Entries
			float totalPrice = 0;

			foreach (var item in order.Products)
			{
				totalPrice += item.Product.Price * item.Amount;
				var productElement = Instantiate(productElementPrefab, orderParent);
				productElement.Setup(item);
			}

			totalPriceText.text = $"$ {totalPrice:0.00}";
		}
	}
}