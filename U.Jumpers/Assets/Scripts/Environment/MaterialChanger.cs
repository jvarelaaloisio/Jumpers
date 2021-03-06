using System;
using UnityEngine;

namespace Environment
{
	[RequireComponent(typeof(MeshRenderer))]
	public class MaterialChanger : MonoBehaviour
	{
		private Material originalMaterial;
		private MeshRenderer meshRenderer;

		private void Start()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			originalMaterial = meshRenderer.material;
		}

		public void ChangeMaterial(Material material)
		{
			meshRenderer.sharedMaterial = material;
		}

		public void ResetMaterials()
		{
			meshRenderer.sharedMaterial = originalMaterial;
		}
	}
}