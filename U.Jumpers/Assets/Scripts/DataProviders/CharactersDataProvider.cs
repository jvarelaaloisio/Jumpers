using System.Collections.Generic;
using Characters;
using Core.Providers;
using UnityEngine;

namespace DataProviders
{
    [CreateAssetMenu(menuName = "Models/Providers/Character List", fileName = "CDP_")]
    public class CharactersDataProvider : DataProvider<List<CharacterView>>
    {
        public override List<CharacterView> Value { get; set; } = new();
    }
}
