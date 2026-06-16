using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class ChatMessage: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI messageText;

        public void Setup(string message, float fontSize=60)
		{
            messageText.text = message;
			messageText.fontSize = fontSize;
		}
	}
}