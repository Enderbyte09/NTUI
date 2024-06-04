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
        public ConsoleFormatSet Format;
        public WriteableCharacter(char character)
        {
            this.character = character;
            isempty = false;
        }
        public WriteableCharacter(char character,ConsoleFormatSet format)
        {
            this.character= character;
            isempty = false;
            this.Format = format;
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
            Console.CursorLeft = p.x; Console.CursorTop = p.y;
            Console.Write(GetChar());
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
        public void AddChar(char c,ConsoleFormatSet f)
        {
            RawScreen.Set(Utilities.GetCursor(), new WriteableCharacter(c,f));
        }

        public void AddChar(char c)
        {
            AddChar(c, ConsoleFormatSet.None);
        }
        public void AddString(string s,ConsoleFormatSet f)
        {

        }
    }

    public class ConsoleFormatSet
    {
        public ConsoleColor ForegroundColour;
        public ConsoleColor BackgroundColour;

        public bool IsActive = true;//If false, will be skipped

        public bool IsBlink = false;
        public bool IsBold = false;
        public bool IsItalic = false;
        public bool IsUnderline = false;
        public bool IsStrikethrough = false;
        public bool IsFaint = false;
        public bool IsInvert = false;
        public ConsoleFormatSet() { }
        public string ToPrefix()
        {
            if (!IsActive)
            {
                return "";
            }
            ///<summary>Get an ANSI prefix excluding colours to be printed</summary>
            string tr = "";
            if (IsBlink)
            {
                tr += Constants.ANSIESCAPE + "5m";
            }
            if (IsBold)
            {
                tr += Constants.ANSIESCAPE + "1m";
            }
            if (IsItalic)
            {
                tr += Constants.ANSIESCAPE + "3m";
            }
            if (IsUnderline)
            {
                tr += Constants.ANSIESCAPE + "4m";
            }
            if (IsStrikethrough)
            {
                tr += Constants.ANSIESCAPE + "9m";
            }
            if (IsFaint)
            {
                tr += Constants.ANSIESCAPE + "2m";
            }
            if (IsInvert)
            {
                tr += Constants.ANSIESCAPE + "7m";
            }
            return tr;
        }
        public static ConsoleFormatSet None = new ConsoleFormatSet()//This is an empty one designed to be used with default values
        {
            IsActive = false
        };
        public string ToSuffix()
        {
            if (!IsActive)
            {
                return "";
            }
            string tr = "";
            if (IsBlink || IsBold || IsItalic || IsUnderline || IsStrikethrough || IsFaint || IsInvert)
            {
                tr = Constants.ANSIESCAPE + "0m";
            }
            return tr;
        }
        public string GetPrintableChar(char c)
        {
            return $"{ToPrefix()}{c}{ToSuffix()}";
        }
        public void ExecutePrint(char c,Position p)
        {
            Console.CursorTop = p.y; Console.CursorLeft = p.x;
            if (IsActive)
            {
                ConsoleColor ofg = Console.ForegroundColor;
                ConsoleColor obg = Console.BackgroundColor;
                Console.ForegroundColor = ForegroundColour;
                Console.BackgroundColor = BackgroundColour;
                Console.Write(GetPrintableChar(c));
                Console.ForegroundColor = ofg;
                Console.BackgroundColor = obg;
            } else
            {
                Console.Write(c);
            }
        }
    }

    public static class Master
    {
        public static ConsoleArray Screen = new ConsoleArray(new Size2d(Console.WindowWidth,Console.WindowHeight));
        public static Dictionary<int,ConsoleFormatSet> Formats = new Dictionary<int,ConsoleFormatSet>();
        
    }
    public static class Utilities
    {
        public static Position GetCursor()
        {
            return new Position(Console.CursorLeft, Console.CursorTop);
        }
    }
    public static class Constants
    {
        public static readonly string ANSIESCAPE = "\x1b[";
    }
    public static class Colours
    {
        public static ConsoleFormatSet Get(int id)
        {
            return Master.Formats[id];
        }
        public static void AddFormat(int id, ConsoleFormatSet format)
        {
            Master.Formats[id] = format;
        }
    }
}
