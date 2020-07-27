using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleColorPreview
{
    class Program
    {
        static int[,] largeCharacter = 
        {
            {0,0,0,1,1,0,0,0},
            {0,0,1,1,1,1,0,0},
            {0,1,1,0,0,1,1,0},
            {0,1,1,0,0,0,0,0},
            {0,0,1,1,1,0,0,0},
            {0,0,0,1,1,1,0,0},
            {0,0,0,0,0,1,1,0},
            {0,1,1,0,0,1,1,0},
            {0,0,1,1,1,1,0,0},
            {0,0,0,1,1,0,0,0}
        };

        static int foregroundColor = 15;
        static int backgroundColor = 0;

        static void WriteCharacter(int row, int column, string character)
        {
            Console.CursorLeft = column;
            Console.CursorTop = row;
            Console.Write(character);
        }

        static void DrawBox(int top, int left, int width, int height)
        {
            WriteCharacter(top, left, "┌");
            WriteCharacter(top, left+width-1, "┐");
            WriteCharacter(top+height-1, left, "└");
            WriteCharacter(top+height-1, left+width-1, "┘");

            for(int a = 1; a < width-1; a++)
            {
                WriteCharacter(top, left+a, "─");
                WriteCharacter(top + height - 1, left+a, "─");
            }

            for(int a = 1; a < height-1; a++)
            {
                WriteCharacter(top+a, left, "│");
                WriteCharacter(top+a, left + width - 1, "│");
            }
            
        }

        static void DrawColorList(int offset)
        {
            DrawBox(2, offset, 4, 18);

            List<ConsoleColor> colors = new List<ConsoleColor>();

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                colors.Add(cc);
            }

            for (int a = 0; a < colors.Count; a++)
            {
                Console.CursorLeft = offset+1;
                Console.CursorTop = a + 3;
                Console.BackgroundColor = colors[a];
                if (a <= 13)
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(a.ToString("00"));
            }
        }

        static void DrawLargeCharacter()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            DrawBox(3, Console.BufferWidth / 2 - 6, 10, 12);

            for(int y = 0; y < 10; y++)
            {
                Console.SetCursorPosition(Console.BufferWidth / 2 - 5, 4+y);
                for(int x = 0; x < 8; x++)
                {
                    if (largeCharacter[y, x] == 0)
                        Console.BackgroundColor = (ConsoleColor)backgroundColor;
                    else
                        Console.BackgroundColor = (ConsoleColor)foregroundColor;

                    Console.Write(" ");
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        static void UpdateForegroundColor(int dir)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(Console.BufferWidth - 3, 3 + foregroundColor);
            Console.Write(" ");

            if (foregroundColor + dir >= 0 && foregroundColor + dir < 16)
            {
                foregroundColor += dir;
            }

            Console.SetCursorPosition(Console.BufferWidth - 3, 3 + foregroundColor);
            Console.Write("←");

            DrawLargeCharacter();
            DrawCombinedColorValue();
        }

        static void UpdateBackgroundColor(int dir)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(2, 3 + backgroundColor);
            Console.Write(" ");

            if (backgroundColor + dir >= 0 && backgroundColor + dir < 16)
            {
                backgroundColor += dir;
            }

            Console.SetCursorPosition(2, 3 + backgroundColor);
            Console.Write("→");

            DrawLargeCharacter();
            DrawCombinedColorValue();
        }

        static void DrawCombinedColorValue()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.BufferWidth/2-9, 15);
            Console.Write("Byte Value: 0x" + (((backgroundColor & 0xf) << 4) | (foregroundColor & 0xf)).ToString("X2"));
        }

        static void DrawControls()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.BufferWidth / 2 - 16, 17);
            Console.Write("Change background color: W↑ S↓");
            Console.SetCursorPosition(Console.BufferWidth / 2 - 16, 18);
            Console.Write("Change foreground color: E↑ D↓");
            Console.SetCursorPosition(Console.BufferWidth / 2 - 16, 19);
            Console.Write("Quit: Q");
        }

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(4, 1);
            Console.Write("BG");

            Console.CursorLeft = Console.BufferWidth / 2 - 12;
            Console.Write("Console Color Preview");
            DrawControls();

            DrawColorList(3);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.BufferWidth-6, 1);
            Console.Write("FG");
            DrawColorList(Console.BufferWidth - 7);

            

            DrawLargeCharacter();

            UpdateForegroundColor(0);
            UpdateBackgroundColor(0);

            bool quit = false;

            while(!quit)
            {
                Thread.Sleep(5);

                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);

                    switch(cki.KeyChar)
                    {
                        case 'q': quit = true; break;

                        case 'w': UpdateBackgroundColor(-1); break;
                        case 's': UpdateBackgroundColor(1); break;

                        case 'e': UpdateForegroundColor(-1); break;
                        case 'd': UpdateForegroundColor(1); break;

                        default: break;
                    }
                }
            }
        }
    }
}
