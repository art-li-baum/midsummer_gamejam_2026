using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;

namespace Gorpozon.WarehouseSim.Management
{
	public class LifetimeServiceInitializer : LifetimeServiceInitializerBase
	{
        protected override void RegisterCustomServices()
        {
            Register(new ShiftManager());
            Register(new PlayerService());
        }
	}
}