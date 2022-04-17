using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Characters.UiEl
{
	public class MapEditorXML : EditorWindow
	{
		public StyleSheet myUss;
		private VisualElement _lastSelection;
		private VisualElement map;
		private VisualElement sideBar;
		private Label selectedCellName;

		[MenuItem("Maps/MapEditorXML")]
		public static void ShowExample()
		{
			var wnd = GetWindow<MapEditorXML>();
			wnd.titleContent = new GUIContent("MapEditorXML");
		}

		public void OnEnable()
		{
			VisualElement root = rootVisualElement;
			root.AddToClassList("root");
			myUss = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Characters/UiEl/MapEditorXML.uss");
			root.styleSheets.Add(myUss);

			DrawMap();
			root.Add(map);
			sideBar = new VisualElement();
			sideBar.AddToClassList("side-bar");
			var sideBarData = new VisualElement();
			sideBarData.AddToClassList("side-bar-data");
			selectedCellName = new Label();
			selectedCellName.AddToClassList("data-line");
			sideBarData.Add(selectedCellName);
			var idField = new IntegerField("id");
			idField.AddToClassList("data-line");
			sideBarData.Add(idField);
			sideBar.Add(sideBarData);
			var sideBarTree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Characters/UiEl/MapEditorXML.uxml");
			VisualElement sideBarXML = sideBarTree.CloneTree();
			sideBarData.Add(sideBarXML);
			root.Add(sideBar);
			root.RegisterCallback<GeometryChangedEvent>(ClampWindow);
		}

		private void DrawMap()
		{
			map = new VisualElement();
			map.AddToClassList("map");
			for (int j = 0; j < 100; j++)
			{
				var row = new VisualElement();
				row.AddToClassList("row");
				for (int i = 0; i < 100; i++)
				{
					var cell = new VisualElement();
					cell.AddToClassList("cell");
					// cell.style.backgroundColor = new StyleColor(new Color(i * .01f, j * .01f, .5f));
					cell.name = (j * 100 + i).ToString();
					var cellButton = new Button();
					cellButton.clicked += () => OnCellSelection(cell);
					cellButton.AddToClassList("cell-button");
					cellButton.AddToClassList("invisible");
					cell.Add(cellButton);
					row.Add(cell);
				}

				map.Add(row);
			}
		}

		private void OnCellSelection(VisualElement cell)
		{
			_lastSelection?.RemoveFromClassList("selection");
			cell.AddToClassList("selection");
			_lastSelection = cell;
			int cellPos = int.Parse(cell.name);
			selectedCellName.text = $"Position: ({cellPos / 100}, {cellPos % 100})";
		}

		private void ClampWindow(GeometryChangedEvent eventData)
		{
			map.style.width = eventData.newRect.height;
			sideBar.style.width = eventData.newRect.width - eventData.newRect.height;
		}
	}
}