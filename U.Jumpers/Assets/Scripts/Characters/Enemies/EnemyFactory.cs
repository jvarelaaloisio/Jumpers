using System;
using Core.Factory;
using Debugging;
using UnityEngine;

namespace Characters.Enemies
{
	/// <summary>
	/// This class is obsolete.
	/// Use Enemy.Factory instead.
	/// </summary>
	[Obsolete]
	public class EnemyFactory : UnityFactory<Enemy>
	{
		private readonly string _namePrefix;
		private readonly Transform _parent;

		private int _lastId;

		public EnemyFactory(
			string namePrefix,
			Transform parent = null)
			: base(namePrefix,
					" Enemy ",
					parent)
		{
		}

		protected override Enemy InstantiateObject()
		{
			int id = GetNextID();
			Printer.Log(LogLevel.Log, "instantiated id: " + id);
			var newGO = new GameObject(GetName(id));
			newGO.gameObject.SetActive(false);
			newGO.transform.SetParent(Parent);
			var source = newGO.AddComponent<AudioSource>();
			source.playOnAwake = false;
			var enemy = newGO.AddComponent<Enemy>();
			enemy.onDeath.AddListener(c => DisposeObject((Enemy) c));
			return enemy;
		}

		protected override void EnableObject(Enemy enemy)
			=> enemy.gameObject.SetActive(false);

		protected override void DisposeObject(Enemy enemy)
			=> enemy.gameObject.SetActive(false);
	}
}