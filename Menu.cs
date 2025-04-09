using System;

public class Menu
{
    private int _indexValgt;
    private string[] _menuPunkter;
    private string _prompt;

    public Menu(string prompt, string[] menuPunkter)
    {
        _indexValgt = 0;
        _menuPunkter = menuPunkter;
        _prompt = prompt;
    }

    private void VisMenuPunkter()
    {
        Console.WriteLine(_prompt);
        for (int i = 0; i < _menuPunkter.Length; i++)
        {
            string valgtMenuPunkt = _menuPunkter[i];
            string prefix;

            if (i == _indexValgt)
            {
                prefix = "*";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                prefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{prefix} << {valgtMenuPunkt} >>");
        }
        Console.ResetColor();
    }

    public int Kør()
    {
        ConsoleKey tastInput;
        do
        {
            Console.Clear();
            VisMenuPunkter();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            tastInput = keyInfo.Key;

            if (tastInput == ConsoleKey.UpArrow)
            {
                _indexValgt--;
                if (_indexValgt == -1)
                {
                    _indexValgt = _menuPunkter.Length - 1;
                }
            }
            else if (tastInput == ConsoleKey.DownArrow)
            {
                _indexValgt++;
                if (_indexValgt == _menuPunkter.Length)
                {
                    _indexValgt = 0;
                }
            }
        } while (tastInput != ConsoleKey.Enter);

        return _indexValgt;
    }

}
