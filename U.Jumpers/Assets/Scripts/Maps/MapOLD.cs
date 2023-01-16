using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Maps
{
    [Serializable]
    public struct MapOLD
    {
        public int[,,] Layout;

        public MapOLD(int[,,] layout)
        {
            this.Layout = layout;
        }

        public MapOLD(int width, int height, int depth)
        {
            Layout = new int[width, height, depth];
        }

        public MapOLD(Int3 size)
        {
            Layout = new int[size.x, size.y, size.z];
        }
    }

    [Serializable]
    public struct MapCell
    {
        public List<string> args;
        public Type type;

        public MapCell(Type type)
        {
            this.type = type;
            args = new List<string>();
        }
        public MapCell(Type type, params string[] args)
        {
            this.type = type;
            this.args = args.ToList();
        }
        
        public enum Type
        {
            Empty,
            Player,
            AI,
            Obstacle,
            Ability
        }
    }

    [Serializable]
    public struct Map
    {
        public int width;
        public int height;
        public MapCell[] cells;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new MapCell[width * height];
            for (var i = 0; i < cells.Length; i++)
                cells[i] = new MapCell(MapCell.Type.Empty);
        }
        
        public ref MapCell this[int index] => ref cells[index];
        public ref MapCell this[int x, int y] => ref cells[x + y * width];

        public int Length => cells.Length;

        public override string ToString()
        {
            string result = string.Empty;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var xString = this[x, y].type;
                    result += xString + (x < width - 1 ? "\t" : string.Empty);
                }

                result += '\n';
            }

            return result;
        }

        public bool TryGetFirstCellOfType(MapCell.Type type, out MapCell cell)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (!cells[i].type.Equals(type))
                    continue;
                cell = cells[i];
                return true;
            }

            cell = default;
            return false;
        }
    }
}