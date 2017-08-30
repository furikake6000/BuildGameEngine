using System;
using UnityEngine;

[Serializable]
class Vector2Int
{
    public int x { get; set; }
    public int y { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="x">整数X</param>
    /// <param name="y">整数Y</param>
    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Vector2型へのキャスト
    /// </summary>
    /// <param name="value">Vector2型の値</param>
    public static implicit operator Vector2(Vector2Int value)
    {
        return new Vector2((float)value.x, (float)value.y);
    }
}
