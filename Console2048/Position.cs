using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    /// <summary>
    /// 二维数组位置
    /// </summary>
    struct Position
    {
        public int RIndex { get; set; }
        public int CIndex { get; set; }

        //构造函数
        //public Position()
        //{
        //
        //}
        public Position(int rIndex,int cIndex):this()
        {
            this.RIndex = rIndex;
            this.CIndex = cIndex;
        }

        /// <summary>
        /// 返回二维数组的中间位置
        /// </summary>
        /// <param name="map">二维数组</param>
        /// <returns>返回的中间位置</returns>
        public static Position GetCenter(int[,] map)
        {
            return new Position(map.GetLength(0) / 2, map.GetLength(1) / 2);
        }
    }
}
