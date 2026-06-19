using Gorpozon.WarehouseSim.Services;
using Gorpozon.WarehouseSim.UI;
using SBG.ServiceLocating;
using System;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Management
{
	public class MessageSender : MonoBehaviour
	{
		[SerializeField] private Message[] FirstShiftStarted;
		[SerializeField] private Message[] FirstPackageShipped;
		[SerializeField] private Message[] SecondPackageShipped;

		private ShiftManager shiftManager;
		private SupervisorScreen supervisorChat;

		private bool firstShiftStarted;
		private int packagesShipped = 0;

        void Start()
		{
			ServiceLocator.TryGet(out shiftManager);
			ServiceLocator.TryGet(out supervisorChat);

            shiftManager.OnShiftBegin += OnShiftBegin;
			shiftManager.OnOrderShipped += OnOrderShipped;
		}

        private void OnDestroy()
        {
            shiftManager.OnShiftBegin -= OnShiftBegin;
            shiftManager.OnOrderShipped -= OnOrderShipped;
        }

        private void OnShiftBegin()
        {
            if (!firstShiftStarted)
			{
				supervisorChat.SendChatMessages(FirstShiftStarted);
				firstShiftStarted = true;
				return;
			}
        }

        private void OnOrderShipped()
        {
			packagesShipped++;

            if (packagesShipped == 1)
			{
				supervisorChat.SendChatMessages(FirstPackageShipped);
				return;
			}
			if (packagesShipped == 2)
			{
				supervisorChat.SendChatMessages(SecondPackageShipped);
			}
        }
	}
}