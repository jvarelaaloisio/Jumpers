using Characters;
using UnityEditor;
using UnityEngine;

namespace Editor.Characters
{
	[CustomEditor(typeof(CharacterView), true)]
	public class CharacterInspector : UnityEditor.Editor
	{
		protected CharacterView View;
		protected PawnModel Model;
		protected UnityEditor.Editor ModelEditor;

		protected virtual void OnEnable()
		{
			View = (CharacterView) target;
			Model = View.Model;
			if(!Model)
				Debug.Log($"{View.name}: no model set");
			ModelEditor = CreateEditor(Model);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space(5);
			if(!Model)
				return;
			EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 3), Color.black);
			EditorGUILayout.Space(5);
			ModelEditor.DrawHeader();
			ModelEditor.OnInspectorGUI();
		}

		protected virtual void OnSceneGUI()
		{
			if(!Model)
				return;
			Handles.color = Color.blue;
			Handles.DrawWireDisc(View.transform.position, Vector3.up, Model.MoveDistance);
		}
	}
}
