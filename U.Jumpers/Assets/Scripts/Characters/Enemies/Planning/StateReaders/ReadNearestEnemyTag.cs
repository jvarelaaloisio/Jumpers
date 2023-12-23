using System.Collections.Generic;
using System.Linq;
using AI.GOAP;
using Core.Providers;
using UnityEngine;

namespace Characters.Enemies.Planning.StateReaders
{
    [CreateAssetMenu(menuName = "Models/States/World Reader/Read Nearest Enemy Tag", fileName = "Reader_NearestEnemyTag", order = 0)]
    public class ReadNearestEnemyTag : WorldStateReader
    {
        [SerializeField] private string id = "nearestEnemyTag";
        [SerializeField] private DataProvider<List<CharacterView>> charactersProvider;


        public override void PopulateState(GoapState state, Pawn pawn)
        {
            if (!charactersProvider || charactersProvider.Value == null)
                return;
            var nearest = charactersProvider
                          .Value
                          .Where(view => view.pawn != pawn)
                          .OrderBy(charView => Vector3.Distance(pawn.GetTransform.position, charView.transform.position))
                          .FirstOrDefault();
            if(nearest == default)
                return;
            var nearestTag = nearest.tag;
            if (!state.Strings.TryAdd(id, nearestTag))
                state.Strings[id] = nearestTag;
        }
    }
}