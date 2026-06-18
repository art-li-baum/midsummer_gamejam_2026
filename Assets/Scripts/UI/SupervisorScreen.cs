using System.Collections;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class SupervisorScreen : MonoBehaviour
	{
		[System.Serializable]
		public class TestMessage
		{
			public string message;
			public float fontSize = 60;
			[Min(0)]
			public float jitter = 0;
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

        public void SendChatMessage(string message, float fontSize=60, float jitter = 0)
		{
			var messageObj = Instantiate(messagePrefab, messageParent);
			messageObj.Setup(message, fontSize, jitter);
		}

		IEnumerator SendTestMessages()
		{
            yield return new WaitForSeconds(1);
			SendChatMessage(firstTestMessage);

            for (int i = 0; i < newTestMessages.Length; i++)
			{
				yield return new WaitForSeconds(1.5f);
				SendChatMessage(newTestMessages[i].message, newTestMessages[i].fontSize, newTestMessages[i].jitter);
			}
		}
	}
}