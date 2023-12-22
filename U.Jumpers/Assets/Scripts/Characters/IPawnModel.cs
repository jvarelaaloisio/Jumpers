namespace Characters
{
	public interface IPawnModel
	{
		float MoveDistance { get; }
		float JumpDuration { get; }
		int LifePoints { get; }
	}
}