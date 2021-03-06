using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core;
using Core.Helpers;
using UnityEngine;

namespace Maps
{
	public class MapManager : MonoBehaviour
	{
		private const string MAP_PATH = "Assets/Resources/Maps/Levels";
		private readonly Int3 mapSize = new Int3(10, 10, 2);
		private Map map;

		[ContextMenu("Save")]
		private void Start()
		{
			map = new Map(100);
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					map.Layout[i] = i * 100 + j;
				}
			}
			// BinaryFormatter bf = new BinaryFormatter();  
			// MemoryStream ms = new MemoryStream();  
			// // bf.Serialize(ms, asdf);  
			// var asdfString = new string[100];
			// for (var i = 0; i < asdfString.Length; i++)
			// {
			// 	string s = asdfString[i] = JsonUtility.ToJson(asdf[i]);
			// }

			File.WriteAllText(MAP_PATH + "/map.json", JsonUtility.ToJson(map.Layout[0]));
			// FileSystemHelper.FileSystemHelper.ForcePath(MAP_PATH);
			// var data = new List<Map> {map};
			// JsonHelper.Save(MAP_PATH, asdf.ToList(), "map.json");
		}
	}
}