namespace Characters
{
	public interface ICharacterModel
	{
		float MoveDistance { get; }
		float JumpDuration { get; }
		int LifePoints { get; }
	}
}