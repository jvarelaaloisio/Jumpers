using System;
using Characters;
using UnityEditor;
using UnityEngine;

namespace Editor.Characters
{
	[CustomEditor(typeof(CharacterView), true)]
	public class CharacterInspector : UnityEditor.Editor
	{
		private CharacterView view;
		private CharacterModel model;
		private UnityEditor.Editor modelEditor;
		private void OnEnable()
		{
			view = (CharacterView) target;
			model = view.Model;
			modelEditor = CreateEditor(model);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space(5);
			if(!model)
				return;
			EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 3), Color.black);
			EditorGUILayout.Space(5);
			modelEditor.DrawHeader();
			modelEditor.OnInspectorGUI();
		}

		private void OnSceneGUI()
		{
			if(!model)
				return;
			Handles.color = Color.blue;
			Handles.DrawWireDisc(view.transform.position, Vector3.up, model.MoveDistance);
		}
	}
}
