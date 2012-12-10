using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ScreenBuffer
{
    //initiate important variables
    public char[,] screenBufferArray; //main buffer array
    public string screenBuffer; //buffer as string (used when drawing)
    public Char[] arr; //temporary array for drawing string
    private static int height;
    private static int width;

    public static void DefineScreenDimensions(int h, int w)
    {
        height = h;
        width = w;
    }

    private static ScreenBuffer instance;

    public static ScreenBuffer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScreenBuffer();
            }
            return instance;
        }
    }

    private ScreenBuffer()
    {
        if (height == 0)
        {
            height = 80;
        }
        if (width == 0)
        {
            width = 80;
        }
        this.screenBufferArray = new char[height, width];
    }

    //this method takes a string, and a pair of coordinates and writes it to the buffer
    public void Draw(string text, int x, int y)
    {
        //split text into array
        arr = text.ToCharArray(0, text.Length);
        //iterate through the array, adding values to buffer 
        int i = 0;
        foreach (char c in arr)
        {
            if (c == '\n') ++y;
            screenBufferArray[y, x + i] = c;
            ++i;
        }
    }

    public void DrawScreen()
    {
        screenBuffer = "";
        //iterate through buffer, adding each value to screenBuffer
        for (int iy = 0; iy < height - 1; iy++)
        {
            for (int ix = 0; ix < width; ix++)
            {
                screenBuffer += screenBufferArray[iy, ix];
            }
        }
        //set cursor position to top left and draw the string
        Console.SetCursorPosition(0, 0);
        Console.Write(screenBuffer);
        screenBufferArray = new char[height, width];
        //note that the screen is NOT cleared at any point as this will simply overwrite the existing values on screen. Clearing will cause flickering again.
    }

}
