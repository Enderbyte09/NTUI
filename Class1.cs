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

    public class  Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }
        public override bool Equals(object obj)
        {
            Position tp = (Position)obj;
            return tp.x == x && tp.y == y ;
        }
    }
    public class Size2d : Position
    {
        public Size2d(int x, int y) : base(x, y) 
        {
            this.x = x;
            this.y = y;
        }
        public static Size2d FromPosition(Position p)
        {
            return new Size2d(p.x, p.y);
        }
        public int GetArea()
        {
            return this.x * this.y;
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
        public void PrintAt(Position p)
        {
            ConsoleColor oldbg = Console.BackgroundColor;
            ConsoleColor oldfg = Console.ForegroundColor;
            Console.CursorLeft = p.x; Console.CursorTop = p.y;
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
        public Size2d Size;
        public ConsoleArray (Size2d size)
        {
            this.Size = size;
            int xsize = size.x; int ysize = size.y;
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
        public WriteableCharacter Get(Position p)
        {
            return Get(p.x,p.y);
        }
        public void Set(int x,int y,WriteableCharacter d) { raw[x][y] = d;}
        public void Set(Position p, WriteableCharacter d)
        {
            Set(p.x,p.y,d);
        }
    }

    public class ConsoleScreen
    {
        private ConsoleArray RawScreen;

        public ConsoleScreen(Size2d s)
        {
            RawScreen = new ConsoleArray(s);
        }
        public void AddChar(char c)
        {

        }
    }

    public static class Master
    {
        public static ConsoleArray Screen = new ConsoleArray(new Size2d(Console.WindowWidth,Console.WindowHeight));
    }
}
