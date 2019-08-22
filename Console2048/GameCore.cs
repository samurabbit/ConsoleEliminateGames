using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    /// <summary>
    /// 处理核心算法
    /// 核心数据为二维数组
    /// 用户根据数据来提供移动方向
    /// 核心根据移动方向处理数组，再返回处理后的数组给用户
    /// 处理需求：
    /// 1、根据移动方向来修改数组
    /// 2、（这个也可交给用户判断）判断游戏是否终结，即再做任何移动都无法再改变数组
    /// 3、在数组为0的区域随机生成一个新的数字，90%为2，10%为4
    /// 4、将数组返回给用户
    /// </summary>
    class GameCore
    {
        //字段和属性
        /// <summary>
        /// 核心二维数组
        /// </summary>
        private int[,] map;
        //移动修改每行的辅助数组
        private int[] moveArray;
        //生成随机数的辅助对象
        private Random randomNum;
        //存储为零的位置
        private List<Position> listPosition;
        private int countMove;
        /// <summary>
        /// 存储最后一次移动前的图
        /// </summary>
        private int[,] lastMap;
        /// <summary>
        /// 是否结束游戏
        /// </summary>
        private bool notEndGame;
        public bool NotEndGame
        {
            get
            {
                return notEndGame; 
            }
         }
        //用户可以看到数据
        /// <summary>
        /// 核心二维数组的数据读取器
        /// </summary>
        public int[,] Map
        {
            get
            {
                return map;
            }
        }

        //构造函数
        public GameCore()
        {
            //初始化数组
            map = new int[4, 4];
            moveArray = new int[4];
            randomNum = new Random();
            listPosition = new List<Position>(16);
            countMove = 0;
            lastMap = new int[4, 4];
            notEndGame = true;
            //先对空数组生成2个随机数
            RandomSite();
            RandomSite();
        }

        /// <summary>
        /// 将数组中的零移到末尾
        /// </summary>
        private void MoveZero()
        {
            //前一个如果为零，则与后面依次比较，如果遇到不为零的，则交换
            for (int current = 0; current < moveArray.Length - 1; current++)
            {
                //如果与最后一个发生交换，则说明方法已经完成
                bool end = false;
                //如果当前不为零，则看下一个
                if (moveArray[current] != 0) continue;
                //当前为零，则依次与后面比较
                for (int follow = current + 1; follow < moveArray.Length; follow++)
                {
                    //如果后面为零，则看下一个
                    if (moveArray[follow] == 0) continue;
                    //如果遇到不为零，则与当前交换
                    int temp = moveArray[current];
                    moveArray[current] = moveArray[follow];
                    moveArray[follow] = temp;
                    countMove++;
                    //如果已经与最后一个交换，则说明去零完成，退出方法
                    if (follow == moveArray.Length - 1)
                        end = true;
                    //当前位置只交换一次即可，然后开始下个位置的去零
                    break;
                }
                if (end) break;
            }
        }

        //合并
        /// <summary>
        /// 合并数组中相邻元素
        /// </summary>
        private void CombArray()
        {
            //去零
            MoveZero();

            //如果第一位置为零，则说明都为零
            if (moveArray[0] == 0) return;

            for (int current = 0; current < moveArray.Length - 1; current++)
            {
                //当前与相邻相同则合并
                if (moveArray[current] != 0 && moveArray[current] == moveArray[current + 1])
                {
                    moveArray[current] *= 2;
                    //下一个被设零，所以下一个不用合并，索引指向下下一个
                    moveArray[++current] = 0;
                    countMove++;
                }
            }
            //去零
            MoveZero();
        }

        //上移方法
        /// <summary>
        /// 将图向上合并相邻元素
        /// </summary>
        private void MoveUpArray()
        {
            int mapRow = map.GetLength(0);
            int mapCol = map.GetLength(1);

            for (int collum = 0; collum < mapCol; collum++)
            {
                //从上到下获取数组
                for (int row = 0; row < mapRow; row++)
                {
                    moveArray[row] = map[row, collum];
                }
                //合并
                CombArray();
                //从上到下返回原来数组
                for (int row = 0; row < mapRow; row++)
                {
                    map[row, collum] = moveArray[row];
                }
            }
        }

        private void MoveDownArray()
        {
            int mapRow = map.GetLength(0);
            int mapCol = map.GetLength(1);

            for (int collum = 0; collum < mapCol; collum++)
            {
                //从下到上获取数组
                for (int row = mapRow - 1; row >= 0; row--)
                {
                    moveArray[mapRow - 1 - row] = map[row, collum];
                }
                //合并
                CombArray();
                //从下到上返回原来数组
                for (int row = mapRow - 1; row >= 0; row--)
                {
                    map[row, collum] = moveArray[mapRow - 1 - row];
                }
            }
        }

        private void MoveLeftArray()
        {
            int mapRow = map.GetLength(0);
            int mapCol = map.GetLength(1);

            for (int row = 0; row < mapRow; row++)
            {
                //从左到右获取数组
                for (int collum = 0; collum < mapCol; collum++)
                {
                    moveArray[collum] = map[row, collum];
                }
                //合并
                CombArray();
                //从左到右返回原来数组
                for (int collum = 0; collum < mapCol; collum++)
                {
                    map[row, collum] = moveArray[collum];
                }
            }
        }

        private void MoveRightArray()
        {
            int mapRow = map.GetLength(0);
            int mapCol = map.GetLength(1);

            for (int row = 0; row < mapRow; row++)
            {
                //从右到左获取数组
                for (int collum = mapCol - 1; collum >= 0; collum--)
                {
                    moveArray[mapCol - 1 - collum] = map[row, collum];
                }
                //合并
                CombArray();
                //从右到左返回原来数组
                for (int collum = mapCol - 1; collum >= 0; collum--)
                {
                    map[row, collum] = moveArray[mapCol - 1 - collum];
                }
            }
        }

        /// <summary>
        /// 在空区域随机生成一个数字2或4
        /// </summary>
        private void RandomSite()
        {
            //先将位置列表清零
            listPosition.Clear();
            //生成为零位置列表
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 0)
                        listPosition.Add(new Position(r, c));
                }
            }
            //如果列表有空位置，则生成随机数位置
            if (listPosition.Count > 0)
            {
                int index = randomNum.Next(listPosition.Count);
                Position position = listPosition[index];
                //将该位置放入2（90%）或4（10%）
                map[position.RIndex, position.CIndex] = (randomNum.Next(10) < 9) ? 2 : 4;
                //将填入了数字的位置从列表中移除
                listPosition.RemoveAt(index);
            }
        }

        /// <summary>
        /// 测试移动是否有效，有效则恢复图，设置结束标志为假
        /// </summary>
        /// <returns>移动可行则真移动无效为假</returns>
        private bool TestMove()
        {
            if (countMove > 0)
            {
                notEndGame = true;
                Array.Copy(lastMap, map, map.GetLength(0) * map.GetLength(1));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断四个方向移动是否还有可能的合并，如果有则游戏未结束，如果没有则游戏结束
        /// </summary>
        /// <returns>判断游戏是否存在继续进行的可能</returns>
        private bool IfNotEndGame()
        {
            //备份图
            Array.Copy(map, lastMap, map.GetLength(0) * map.GetLength(1));
            //判断四个方向的移动是否可行，
            //只要有可能就设置结束标志为假，并恢复移动前的图
            MoveUpArray();
            if (TestMove()) return true ;
            MoveDownArray();
            if (TestMove()) return true;
            MoveLeftArray();
            if (TestMove()) return true;
            MoveRightArray();
            if (TestMove()) return true;
            return false;

        }

        public void Move(MoveDirection direction)
        {
            countMove = 0;
            switch (direction)
            {
                case MoveDirection.Up:
                    MoveUpArray();
                    break;
                case MoveDirection.Down:
                    MoveDownArray();
                    break;
                case MoveDirection.Left:
                    MoveLeftArray();
                    break;
                case MoveDirection.Right:
                    MoveRightArray();
                    break;
            }
            //生成一个随机位置放置2或4
            if (countMove>0)
                RandomSite();
            //如果没有移动并且没有空位，则判断是否游戏结束
            else if(listPosition.Count==0)
            {
                //判断并设置结束标志
                notEndGame = IfNotEndGame();
            }
        }

    }
}
