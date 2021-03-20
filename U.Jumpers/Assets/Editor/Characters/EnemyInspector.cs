using Characters.Enemies;
using Characters.Movements;
using UnityEditor;
using UnityEngine;

namespace Editor.Characters
{
	[CustomEditor(typeof(Enemy), true)]
	public class EnemyInspector : CharacterInspector
	{
		protected new Enemy View;
		protected Movement Movement;
		private UnityEditor.Editor _movementEditor;

		protected override void OnEnable()
		{
			base.OnEnable();
			View = (Enemy) serializedObject.targetObject;
			Movement = View.movement;
			if (!Movement)
				Debug.Log($"{View.name}: no movement set");
			_movementEditor = CreateEditor(Movement);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space(5);
			if (!Movement)
				return;

			EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 3), Color.black);
			EditorGUILayout.Space(5);
			_movementEditor.DrawHeader();
			_movementEditor.OnInspectorGUI();
		}
	}
}