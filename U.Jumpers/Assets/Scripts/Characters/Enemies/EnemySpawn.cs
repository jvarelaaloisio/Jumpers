using Characters.Movements;
using UnityEngine;

namespace Characters.Enemies
{
	public class EnemySpawn : MonoBehaviour
	{
		[Header("Setup")]
		[SerializeField]
		private PawnModel model;

		[SerializeField]
		private Movement movement;

		[SerializeField]
		private DamageModel damageModel;

		[Space, Header("Visual")]
		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private Material[] materials;
		
		[Space, Header("Audio")]

		[SerializeField] private AudioClip onMoveClip;
		[SerializeField] private AudioClip onDamageClip;
		[SerializeField] private AudioClip onDeathClip;
		
		public virtual void SetValues(Enemy enemy)
		{
			enemy = new Enemy.Builder(enemy)
					.SetModel(model)
					.SetMovement(movement)
					.SetDamage(damageModel)
					.SetMesh(mesh)
					.SetMaterials(materials)
					.SetOnMoveClip(onMoveClip)
					.SetOnDamageClip(onDamageClip)
					.SetOnDeathClip(onDeathClip)
					.Build();
		}
	}
}