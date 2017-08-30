using System;
using UnityEngine;

[Serializable]
public class Vector2Int
{
    const int HashTimeX = 1;    //ハッシュ値を求める際にXにかける係数
    const int HashTimeY = 9973; //ハッシュ値を求める際にYにかける係数

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

    #region 演算子定義
    public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector2Int operator *(Vector2Int v1, int i1)
    {
        return new Vector2Int(v1.x * i1, v1.y * i1);
    }
    public static Vector2Int operator *(int i1, Vector2Int v1)
    {
        return v1 * i1;
    }

    public static Vector2Int operator *(Vector2Int v1, float f1)
    {
        return new Vector2Int((int)(f1 * v1.x), (int)(f1 * v1.y));
    }
    public static Vector2Int operator *(float f1, Vector2Int v1)
    {
        return v1 * f1;
    }

    public static bool operator ==(Vector2Int v1, Vector2Int v2)
    {
        //nullの確認
        if (object.ReferenceEquals(v1, v2))
        {
            return true;
        }
        if (((object)v1 == null) || ((object)v2 == null))
        {
            return false;
        }

        return (v1.x == v2.x) && (v1.y == v2.y);
    }

    public static bool operator !=(Vector2Int v1, Vector2Int v2)
    {
        return !(v1 == v2);
    }

    public override bool Equals(System.Object v)
    {
        if(v is Vector2Int)
        {
            return this == (v as Vector2Int);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return x * HashTimeX + y * HashTimeY;
    }
    #endregion
}
