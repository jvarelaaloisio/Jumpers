using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AI.AStar;
using Core.Providers;
using UnityEngine;
using static AI.AStar.AStar<UnityEngine.Vector3>;

namespace Characters.Movements
{
    [Obsolete]
    public class AStarMovement : Movement
    {
        [SerializeField] private DataProvider<List<Transform>> waypointsProvider;

        [SerializeField] private float arrivalTolerance;
        [SerializeField] private string goalKey = "goal";
        [SerializeField] private string pathKey = "path";
        private AStar<Vector3> _aStar;

        [Obsolete]
        public override async Task<Vector3> GetNextPositionAsync(Pawn pawn)
        {
            var transform = pawn.GetTransform;
            if (!pawn.Data.ContainsKey(goalKey))
            {
                return transform.position;
            }

            var goal = (Vector3)pawn.Data[goalKey];

            PathResult path;
            if (pawn.Data.ContainsKey(pathKey))
                // return ((PathResult)pawn.Data[pathKey]).Steps.Value.GetEnumerator().
            // else
                pawn.GetMonoBehaviour.StartCoroutine(FindPath(transform, goal, pawn));

            return Vector3.zero;
        }

        private IEnumerator FindPath(Transform transform, Vector3 goal, Pawn pawn)
        {
            _aStar.FindPath
                (transform.position,
                 goal,
                 Vector3.Distance,
                 pos => Vector3.Distance(pos, transform.position) < arrivalTolerance,
                 pos => pawn.GetAvailablePillars(pos)
                            .Select(t => new Arc(t.position, Vector3.Distance(t.position, goal))));
            yield break;
        }
    }
}