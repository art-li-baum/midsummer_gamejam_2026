namespace Gorpozon.WarehouseSim.Objects
{
	public interface IInteractible
	{
		string InteractionPrompt { get; }
		bool CanInteract { get; }
		void Interact();
	}
}