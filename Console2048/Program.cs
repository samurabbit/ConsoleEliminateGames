using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    class Program
    {
        static void Main(string[] args)
        {
            GameCore core = new GameCore();
            ShowMap(core.Map);
            do
            {
                InputMove(core);
                ShowMap(core.Map);
            } while (core.NotEndGame);
            Console.WriteLine("Game Over!");
            Console.ReadLine();
        }

        static void ShowMap(int[,] map)
        {
            Console.Clear();
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    Console.Write(map[row, col].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }

        static void InputMove(GameCore core)
        {
            Console.WriteLine("上下左右：");
            switch (Console.ReadLine())
            {
                case "上":
                    core.Move(MoveDirection.Up);
                    break;
                case "下":
                    core.Move(MoveDirection.Down);
                    break;
                case "左":
                    core.Move(MoveDirection.Left);
                    break;
                case "右":
                    core.Move(MoveDirection.Right);
                    break;
                default:
                    Console.WriteLine("输入错误！");
                    break;
            }

        }

    }
}
