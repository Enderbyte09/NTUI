using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUI
{
    public static class DONOTUSE
    {
        public static void Main(string[] args)
        {
            Test();
        }

        public static void Test()
        {
            Console.WriteLine(Console.WindowHeight);
            Console.WriteLine(Console.WindowWidth);

            Console.ReadKey();
        }
    }

    public class WriteableCharacter
    {
        private bool isempty;
        private char character;
        public ConsoleColor ForegroundColour = ConsoleColor.White;
        public ConsoleColor BackgroundColour = ConsoleColor.Black;
        public WriteableCharacter(char character)
        {
            this.character = character;
            isempty = false;
        }
        public WriteableCharacter(char character,ConsoleColor f,ConsoleColor b)
        {
            this.character= character;
            isempty = false;
            this.BackgroundColour = b;
            this.ForegroundColour = f;
        }
        private WriteableCharacter(bool ia) {
            this.isempty = ia;
        }
        public bool IsEmpty()
        {
            return this.isempty;
        }
        public override string ToString()
        {
            return character.ToString();
        }
        public char GetChar()
        {
            return character;
        }
        public void PrintAt(int x,int y)
        {
            ConsoleColor oldbg = Console.BackgroundColor;
            ConsoleColor oldfg = Console.ForegroundColor;
            Console.CursorLeft = x; Console.CursorTop = y;
            Console.BackgroundColor = BackgroundColour;
            Console.ForegroundColor = ForegroundColour;
            Console.Write(GetChar());
            Console.ForegroundColor = oldfg;
            Console.BackgroundColor= oldbg;
        }
        public static WriteableCharacter Empty()
        {
            return new WriteableCharacter(true);//Only local can access
        }
    }
    public class ConsoleArray
    {
        private WriteableCharacter[][] raw;
        public ConsoleArray (int xsize, int ysize)
        {
            raw = new WriteableCharacter[xsize][];
            for (int x = 0; x < xsize; x++)
            {
                raw[x] = new WriteableCharacter[ysize];
                for (int y = 0; y < ysize; y++)
                {
                    raw[x][y] = WriteableCharacter.Empty();
                }
            }
        }
        public WriteableCharacter Get(int x,int y)
        {
            return raw[x][y];
        }
        public void Set(int x,int y,WriteableCharacter d) { raw[x][y] = d;}
    }

    public class ConsoleScreen
    {
        private ConsoleArray RawScreen;
        public ConsoleScreen(int xsize, int ysize)
        {
            RawScreen = new ConsoleArray(xsize, ysize);
        }
        public void AddChar(char c)
        {
        }
    }

    public static class Master
    {
        public static ConsoleArray Screen = new ConsoleArray(Console.WindowWidth,Console.WindowHeight);
    }
}
