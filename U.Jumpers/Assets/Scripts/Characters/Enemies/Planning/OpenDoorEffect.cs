using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Open Door", fileName = "OpenDoor", order = 0)]
    public class OpenDoorEffect : Effect
    {
        [SerializeField] private string id = "DoorIsOpen";
        
        public override void Apply(GoapState state, Pawn pawn)
        {
            if (!state.Bools.TryAdd(id, true))
                state.Bools[id] = true;
        }
    }
}