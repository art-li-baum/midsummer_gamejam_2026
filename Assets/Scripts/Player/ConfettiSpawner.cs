using Gorpozon.WarehouseSim.Management;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class ConfettiSpawner : MonoBehaviour
	{
		public ParticleSystem[] Particles;

		[SerializeField] private AudioSource sfxPlayer;

		private ProgressionManager progressionManager;

		void Start()
		{
			ServiceLocator.TryGet(out progressionManager);

			progressionManager.OnLevelUp += SpawnConfetti;
		}

        private void SpawnConfetti(int lvl)
        {
            sfxPlayer.Play();

            foreach (var particle in Particles)
			{
				particle.Play();
			}
        }

        private void OnDestroy()
        {
			progressionManager.OnLevelUp -= SpawnConfetti;
        }
    }
}