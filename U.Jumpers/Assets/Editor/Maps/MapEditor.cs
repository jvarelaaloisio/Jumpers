using System;
using System.Collections.Generic;
using Maps;
using UnityEditor;
using UnityEngine;

namespace Editor.Maps
{
    public class MapEditor : EditorWindow
    {
        private const int CellSize = 50;
        private const string PlayerIcon = "Avatar Icon";
        private const string ObstacleIcon = "AudioReverbFilter Icon";
        private int selection = 0;
        private int lineQty = 50;
        private Vector2Int dimensions = Vector2Int.one;
        private Vector2 scrollPos;
        private Map _map;

        [MenuItem("Maps/Map Editor")]
        public static void OpenWindow()
        {
            var w = EditorWindow.GetWindow<MapEditor>();
            w._map = GetMap(w.dimensions);
            PopulateMap(ref w._map);
            w.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("Selection", selection);
            EditorGUILayout.Vector2Field("Coords", new Vector2(selection % lineQty + 1, selection / lineQty + 1));
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            var oldDims = dimensions;
            dimensions = EditorGUILayout.Vector2IntField("Dimensions", dimensions);
            dimensions.x = Mathf.Clamp(dimensions.x, 1, 75);
            dimensions.y = Mathf.Clamp(dimensions.y, 1, 75);
            if(oldDims != dimensions)
            {
                _map = GetMap(dimensions);
                PopulateMap(ref _map);
            }
            var mapDimensions = GetMapDimensions(CellSize, new Vector2Int(_map.width, _map.height));
            // EditorGUILayout.Vector2Field("Map Dimensions", mapDimensions);
            GUILayout.BeginHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUIContent[] contents = new GUIContent[_map.Length];
            var emptyIcon = new GUIContent();
            emptyIcon.tooltip = "Empty";
            var playerIcon = EditorGUIUtility.IconContent(PlayerIcon);
            playerIcon.tooltip = "Player";
            var obstacleIcon = EditorGUIUtility.IconContent(ObstacleIcon);
            obstacleIcon.tooltip = "Obstacle";
            var cellDict = new Dictionary<MapCell.Type, GUIContent>
            {
                { MapCell.Type.Empty , emptyIcon},
                { MapCell.Type.Player , playerIcon},
                { MapCell.Type.Obstacle , obstacleIcon},
            };
            for (int y = 0; y < _map.height; y++)
            {
                for (int x = 0; x < _map.width; x++)
                {
                    var content = emptyIcon;
                    try
                    {

                        if (cellDict.ContainsKey(_map[x, y].type))
                            content = cellDict[_map[x, y].type];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.LogError($"x: {x} y: {y}");
                    }
                    
                    var contentsCoords = x + y * _map.width;
                    contents[contentsCoords] = content;
                }
            }

            selection = GUILayout.SelectionGrid(selection, contents, _map.width,
                                                GUILayout.Width(mapDimensions.x),
                                                GUILayout.Height(mapDimensions.y));
            GUILayout.EndScrollView();
            GUILayout.BeginVertical(GUILayout.Width(200));
            var labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Options", labelStyle);
            var normalWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            _map[selection].type = (MapCell.Type) EditorGUILayout.EnumPopup("Type", _map[selection].type);
            EditorGUIUtility.labelWidth = normalWidth;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private static Map GetMap(Vector2Int dimensions) => new Map(dimensions.x, dimensions.y);

        private static void PopulateMap(ref Map map)
        {
            if (map.height > 3 && map.width > 3)
                map[2, 2].type = MapCell.Type.Player;

            if (map.height >= 10)
                for (int x = 0; x < map.width; x++)
                    map[x, 9].type = MapCell.Type.Obstacle;
            if (map.width >= 10)
                for (int y = 0; y < map.height; y++)
                    map[9, y].type = MapCell.Type.Obstacle;
        }

        private static Vector2 GetMapDimensions(int cellSize, Vector2Int dimensions)
        {
            var mapDimensions = new Vector2
            {
                x = dimensions.x * cellSize,
                y = dimensions.y * cellSize,
            };
            return mapDimensions;
        }
    }
}