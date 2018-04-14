﻿using System;
using UnityEngine;

namespace Models
{
    public class BoardPath : ICloneable
    {
        public Vector2Int Start;
        public Vector2Int End;

        public BoardPath(int x1, int y1, int x2, int y2)
        {
            Start = new Vector2Int(x1, y1);
            End = new Vector2Int(x2, y2);
        }

        public Vector3 GetPathScale(int cellSize, int cellOffset)
        {
            return new Vector3(Mathf.Abs(End.x - Start.x) * (cellSize + cellOffset) + cellSize, 1.0f,
                Mathf.Abs(End.y - Start.y) * (cellSize + cellOffset) + cellSize);
        }
        
        public Vector3 GetPathPosition(int cellSize, int cellOffset)
        {
            var pathPosition = GetPathScale(cellSize, cellOffset);

            if (GameManager.Instance != null)
            {
                pathPosition.x = (pathPosition.x - cellSize) / 2.0f + GameManager.Instance.BoardManager.CellToWorldPosition(Start.x);
                pathPosition.y = 1.0f;
                pathPosition.z = (pathPosition.z - cellSize) / 2.0f + GameManager.Instance.BoardManager.CellToWorldPosition(Start.y);
            }
            else
            {
                pathPosition.x = (pathPosition.x - cellSize) / 2.0f + LevelEditorTD.Instance.BoardManager.CellToWorldPosition(Start.x);
                pathPosition.y = 1.0f;
                pathPosition.z = (pathPosition.z - cellSize) / 2.0f + LevelEditorTD.Instance.BoardManager.CellToWorldPosition(Start.y);
            }
            
            return pathPosition;
        }

        public object Clone()
        {
            return new BoardPath(this.Start.x, this.Start.y, this.End.x, this.End.y);
        }

        public override String ToString()
        {
            return String.Format("X1: {0}, Y1: {1} / X2: {2}, Y2:{3}", Start.x, Start.y, End.x, End.y);
        }
    }
}