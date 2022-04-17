using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Maps
{
	public class MapEditor : EditorWindow
	{
		[MenuItem("Maps/Map Editor")]
		public static void OpenWindow()
		{
			var w = EditorWindow.GetWindow<MapEditor>();
			w.Show();
		}

		private void OnGUI()
		{
			Vector2 positionCol = Vector2.zero;
			Vector2 positionRow = Vector2.zero;
			Rect row = new Rect(positionCol, new Vector2(1000, 1));
			Rect col = new Rect(positionCol, new Vector2(1, 1000));
			GUILayout.BeginHorizontal();
			for (int i = 0; i < 100; i++)
			{
				int newPos = (i+1) * 10;
				positionRow.y = newPos;
				row.position = positionRow;
				positionCol.x = newPos;
				col.position = positionCol;
				EditorGUI.DrawRect(row, Color.black);
				EditorGUI.DrawRect(col, Color.black);
			}
			GUILayout.EndHorizontal();
		}
	}
}
