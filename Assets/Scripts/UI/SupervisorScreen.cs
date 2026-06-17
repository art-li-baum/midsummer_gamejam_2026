using System.Collections;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class SupervisorScreen : MonoBehaviour
	{
		[System.Serializable]
		public struct TestMessage
		{
			public string message;
			public float fontSize;
		}

		[TextArea]
		public string firstTestMessage;
		public string[] testMessages;
		public TestMessage[] newTestMessages;

        [SerializeField] private RectTransform messageParent;
		[SerializeField] private ChatMessage messagePrefab;

        private void Start()
        {
            StartCoroutine(SendTestMessages());
        }

        public void SendChatMessage(string message, float fontSize=60)
		{
			var messageObj = Instantiate(messagePrefab, messageParent);
			messageObj.Setup(message, fontSize);
		}

		IEnumerator SendTestMessages()
		{
            yield return new WaitForSeconds(1);
			SendChatMessage(firstTestMessage);

            for (int i = 0; i < newTestMessages.Length; i++)
			{
				yield return new WaitForSeconds(1.5f);
				SendChatMessage(newTestMessages[i].message, newTestMessages[i].fontSize);
			}
		}
	}
}