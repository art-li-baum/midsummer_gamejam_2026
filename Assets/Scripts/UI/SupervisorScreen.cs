using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
    [System.Serializable]
    public class Message
    {
        [TextArea]
        public string Text;
        [Range(0.25f, 3)]
        public float MessageDelay = 1f;
        [Range(0, 50)]
        public float Jitter = 0;
        public bool CustomFontSize = false;
        [Range(60, 100)]
        public int FontSize = 60;
    }

    public class SupervisorScreen : MonoBehaviour
	{
        [SerializeField] private RectTransform messageParent;
		[SerializeField] private ChatMessage messagePrefab;

        private Queue<Message> messageQueue = new();
        private Coroutine messageRoutine;

        private ShiftManager shiftManager;
        private ProgressionManager progressionManager;

        private void Start()
        {
            ServiceLocator.TryGet(out shiftManager);
            ServiceLocator.TryGet(out progressionManager);

            shiftManager.OnShiftComplete += ClearChat;
            progressionManager.OnLevelUp += OnLevelUp;

            OnLevelUp(0);
        }

        private void OnDestroy()
        {
            shiftManager.OnShiftComplete -= ClearChat;
            progressionManager.OnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp(int lvl)
        {
            var intro = progressionManager.Progression.Levels[lvl].LevelIntroduction;
            if (intro == null || intro.Length == 0) return;

            SendChatMessages(intro);
        }

        public void SendChatMessages(params Message[] messages)
		{
            foreach (var msg in messages)
            {
                messageQueue.Enqueue(msg);
            }

            if (messageRoutine == null) messageRoutine = StartCoroutine(SendRoutine());
		}

        public void ClearChat()
        {
            messageQueue.Clear();

            if (messageRoutine != null)
            {
                StopCoroutine(messageRoutine);
                messageRoutine = null;
            }

            for (int i = messageParent.childCount - 1; i >= 0; i--)
            {
                Destroy(messageParent.GetChild(i).gameObject);
            }
        }

        private IEnumerator SendRoutine()
        {
            while (messageQueue.Count > 0)
            {
                var msg = messageQueue.Dequeue();

                yield return new WaitForSeconds(msg.MessageDelay);

                int fontSize = msg.CustomFontSize ? msg.FontSize : 60;

                var messageObj = Instantiate(messagePrefab, messageParent);
                messageObj.Setup(msg.Text, fontSize, msg.Jitter);
            }

            messageRoutine = null;
        }
	}
}