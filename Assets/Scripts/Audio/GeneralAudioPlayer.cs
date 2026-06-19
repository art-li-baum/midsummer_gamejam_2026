using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Audio
{
	public class GeneralAudioPlayer: MonoBehaviour
	{
		[SerializeField] private AudioSource musicPlayer;
		[SerializeField] private AudioSource sfxPlayer;

		[Header("Music")]
		[SerializeField] private AudioClip musicClip;
		[SerializeField] private AudioClip resultClip;


		[Header("ShiftSounds")]
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
			musicPlayer.resource = musicClip;
			musicPlayer.Play();
			sfxPlayer.PlayOneShot(startSound);
		}

		private void OnShiftEnd()
		{
			musicPlayer.Stop();
			musicPlayer.resource = resultClip;
			musicPlayer.Play();
			sfxPlayer.PlayOneShot(endSound);
		}
	}
}