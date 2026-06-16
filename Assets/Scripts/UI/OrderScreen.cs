using Gorpozon.WarehouseSim.Data;
using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class OrderScreen: MonoBehaviour
	{
		[SerializeField] private RectTransform orderParent;
		[SerializeField] private ProductElement productElementPrefab;
		[SerializeField] private TextMeshProUGUI totalPriceText;

		private ShiftManager shiftManager;

        private void Start()
        {
			ServiceLocator.TryGet(out shiftManager);
			shiftManager.OnOrderChanged += ShowNextOrder;
        }

        private void OnDestroy()
        {
            shiftManager.OnOrderChanged -= ShowNextOrder;
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

			if (order != null)
			{
                foreach (var item in order.Products)
                {
                    totalPrice += item.Product.Price * item.Amount;
                    var productElement = Instantiate(productElementPrefab, orderParent);
                    productElement.Setup(item);
                }
            }

			totalPriceText.text = $"$ {totalPrice:0.00}";
		}
	}
}