using System.Collections.Generic;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Environment
{
	public class SeeThroughController : MonoBehaviour
	{
		[SerializeField] private float frequency;
		[SerializeField] private int degrees;
		[SerializeField] private float maxDistance;
		[SerializeField] private LayerMask targetMask;
		[SerializeField] private int sceneIndex;
		
		private readonly List<MaterialChanger> _targets = new List<MaterialChanger>();
		private ActionWithFrequency _updateMaterials;
		private Transform _myTransform;
		[SerializeField] private Material seeThrough;

		[ContextMenu("Awake (Reset Actions)")]
		private void Awake()
		{
			_updateMaterials = new ActionWithFrequency(UpdateMaterials, frequency, sceneIndex);
			_myTransform = transform;
		}

		private void Start()
		{
			_updateMaterials.StartAction();
		}

		[ContextMenu("Update Materials")]
		private void UpdateMaterials()
		{
			Vector3 myPosition = _myTransform.position;
			Vector3 myForward = _myTransform.forward;
			List<MaterialChanger> candidates = new List<MaterialChanger>();
			Ray ray = new Ray(myPosition, myForward);

			for (int i = -1; i < 2; i += 2)
			{
				var rotation = Quaternion.Euler(transform.right * degrees * i);
				CheckForCandidate(ray, rotation, myForward, candidates);
				rotation = Quaternion.Euler(transform.up * degrees * i);
				CheckForCandidate(ray, rotation, myForward, candidates);
			}

			foreach (MaterialChanger candidate in candidates)
			{
				if (_targets.Contains(candidate))
					continue;

				Debug.DrawLine(myPosition, candidate.transform.position, Color.green);
				_targets.Add(candidate);
				candidate.ChangeMaterial(seeThrough);
			}

			var oldTargets = _targets.GetRange(0, _targets.Count);
			foreach (MaterialChanger target in oldTargets)
			{
				if (candidates.Contains(target)) continue;
				target.ResetMaterials();
				_targets.Remove(target);
				Debug.DrawLine(myPosition, target.transform.position, Color.red);
			}
		}

		private void CheckForCandidate(Ray ray, Quaternion rotation, Vector3 myForward,
			List<MaterialChanger> candidates)
		{
			ray.direction = rotation * myForward;
			if (Physics.Raycast(ray, out var hit, maxDistance, targetMask, QueryTriggerInteraction.Collide)
			    && hit.transform.TryGetComponent(out MaterialChanger materialChanger))
			{
				candidates.Add(materialChanger);
			}
		}
	}
}