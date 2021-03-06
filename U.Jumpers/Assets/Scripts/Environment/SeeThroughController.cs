using System.Collections.Generic;
using Packages.UpdateManagement;
using UnityEngine;

namespace Environment
{
	public class SeeThroughController : MonoBehaviour
	{
		[SerializeField] private float frequency;
		[SerializeField] private int degrees;
		[SerializeField] private float maxDistance;
		[SerializeField] private LayerMask targetMask;
		private List<MaterialChanger> targets = new List<MaterialChanger>();
		private ActionWithFrequency updateMaterials;
		private Transform myTransform;
		[SerializeField] private Material seeThrough;

		[ContextMenu("Awake (Reset Actions)")]
		private void Awake()
		{
			updateMaterials = new ActionWithFrequency(UpdateMaterials, frequency);
			myTransform = transform;
		}

		private void Start()
		{
			updateMaterials.StartAction();
		}

		[ContextMenu("Update Materials")]
		private void UpdateMaterials()
		{
			Vector3 myPosition = myTransform.position;
			Vector3 myForward = myTransform.forward;
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
				if (targets.Contains(candidate))
					continue;

				Debug.DrawLine(myPosition, candidate.transform.position, Color.green);
				targets.Add(candidate);
				candidate.ChangeMaterial(seeThrough);
			}

			var oldTargets = targets.GetRange(0, targets.Count);
			foreach (MaterialChanger target in oldTargets)
			{
				if (candidates.Contains(target)) continue;
				target.ResetMaterials();
				targets.Remove(target);
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