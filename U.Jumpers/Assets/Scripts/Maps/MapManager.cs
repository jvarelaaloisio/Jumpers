﻿using System.IO;
using Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Maps
{
	public class MapManager : MonoBehaviour
	{
		private const string MAP_PATH = "Assets/Resources/Maps/Levels";
		private readonly Int3 _mapSize = new Int3(10, 10, 2);
		private MapOLD _mapOld;

		[ContextMenu("Save")]
		private void Start()
		{
			_mapOld = new MapOLD(_mapSize);
			for (int i = 0; i < _mapSize.x; i++)
			{
				for (int j = 0; j < _mapSize.y; j++)
				{
					_mapOld.Layout[i,j,0] = i * 100 + j * 10;
					_mapOld.Layout[i,j,1] = i * 100 + j * 10 + 1;
				}
			}
			File.WriteAllText(MAP_PATH + "/map.json", JsonConvert.SerializeObject(_mapOld));
		}
	}
}