using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

public class Textbox
{
    private Texture2D backgroundTexture;
    private Texture2D borderTexture;
    private SpriteFont font;
    private Rectangle bounds;
    private Color backgroundColor;
    private Color textColor;
    private int borderWidth;
    private Color borderColor;
    private StringBuilder text;
    private bool isActive;

    public string Text => text.ToString();
    public bool IsValid { get;  set; } = false;

    public bool getIsValid()
    {
        return IsValid;
    }
    

    private int targetNumber;

    public Textbox(GraphicsDevice graphicsDevice, SpriteFont font, Rectangle bounds, Color backgroundColor, Color textColor, int targetNumber, Color borderColor, int borderWidth = 3)
    {
        this.font = font;
        this.bounds = bounds;
        this.backgroundColor = backgroundColor;
        this.borderColor = borderColor;
        this.borderWidth = borderWidth;
        this.textColor = textColor;
        this.targetNumber = targetNumber;

       
        backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
        backgroundTexture.SetData(new[] { Color.White });

       
        borderTexture = new Texture2D(graphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.White });

        
        text = new StringBuilder();
        isActive = false;
    }
    public bool getisActive()
    {
        return isActive;
    }

    public void setTargetNumber(int targetNumber)
    {
        this.targetNumber = targetNumber;
    }
    public int getTargetNumber()
    {
        return targetNumber;
    }
    public void Update(KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        if (isActive)
        {
            foreach (var key in keyboardState.GetPressedKeys())
            {
                if (previousKeyboardState.IsKeyUp(key))
                {
                    // Usuwanie cyfr za pomoca backspace
                    if (key == Keys.Back && text.Length > 0)
                    {
                        text.Length--; 
                    }
                    // Sprawdzenie poprawności wpisanej liczby
                    else if (key == Keys.Enter)
                    {
                        ValidateInput();
                        isActive = false; 
                    }
                    else
                    {
                        // Dodanie cyfry do textboxa
                        char c = ConvertKeyToChar(key, keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift));
                        if (IsNumericOrValid(c)) text.Append(c);
                    }
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        Rectangle borderRectangle = new Rectangle(bounds.X - borderWidth, bounds.Y - borderWidth, bounds.Width + borderWidth * 2, bounds.Height + borderWidth * 2);
        spriteBatch.Draw(borderTexture, borderRectangle, borderColor);

       
        spriteBatch.Draw(backgroundTexture, bounds, backgroundColor);

        
        spriteBatch.DrawString(font, text.ToString(), new Vector2(bounds.X + 5, bounds.Y + 5), textColor);
    }

    public void Activate() => isActive = true;
    public void Deactivate() => isActive = false;

    private void ValidateInput()
    {
        if (int.TryParse(text.ToString(), out int enteredNumber))
        {
            IsValid = enteredNumber == targetNumber; // Sprawdzenie czy wpisana liczba = wynik dzialania
        }
        else
        {
            IsValid = false; 
        }
    }

    private char ConvertKeyToChar(Keys key, bool shift)
    {
        
        if (key >= Keys.D0 && key <= Keys.D9)
        {
            return (char)(key - Keys.D0 + '0');
        }
        if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
        {
            return (char)(key - Keys.NumPad0 + '0');
        }
        return '\0';
    }

    private bool IsNumericOrValid(char c)
    {
        return char.IsDigit(c);
    }

    public void Clear()
    {
        text.Clear(); //funkcja klasy StringBuilder
        IsValid = false; 
    }
}
