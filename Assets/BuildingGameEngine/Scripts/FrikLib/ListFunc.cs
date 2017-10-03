using System;
using System.Collections.Generic;

namespace FrikLib
{
    public static class ListFunc
    {
        private static Random rand = new Random();  //乱数生成

        //Listに関する拡張
        public static List<T> Swap<T>(this List<T> list, int a, int b)
        {
            T buf = list[a];
            list[a] = list[b];
            list[b] = buf;
            return list;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            //Fisher–Yates shuffle algorhythm

            for (int i = list.Count - 1; i >= 1; i--) list.Swap(rand.Next(i + 1), i);

            return list;
        }

        public static T RandomPick<T>(this List<T> list)
        {
            return list[rand.Next(list.Count)];
        }
    }
}