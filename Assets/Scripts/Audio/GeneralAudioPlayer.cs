using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Audio
{
	public class GeneralAudioPlayer: MonoBehaviour
	{
		[SerializeField] private AudioSource musicPlayer;
		[SerializeField] private AudioSource sfxPlayer;


		[SerializeField] private AudioClip startSound;
        [SerializeField] private AudioClip endSound;


        private ShiftManager shiftManager;


		void Start()
		{
			ServiceLocator.TryGet(out shiftManager);

			shiftManager.OnShiftBegin += OnShiftStart;
			shiftManager.OnShiftComplete += OnShiftEnd;
		}
	
		private void OnShiftStart()
		{
			musicPlayer.Play();
			sfxPlayer.PlayOneShot(startSound);
		}

		private void OnShiftEnd()
		{
			musicPlayer.Stop();
			sfxPlayer.PlayOneShot(endSound);
		}
	}
}