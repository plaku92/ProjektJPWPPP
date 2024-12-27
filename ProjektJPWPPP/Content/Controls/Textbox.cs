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

        // Create background texture
        backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
        backgroundTexture.SetData(new[] { Color.White });

        // Create border texture
        borderTexture = new Texture2D(graphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.White });

        // Initialize text and state
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
                    // Handle backspace
                    if (key == Keys.Back && text.Length > 0)
                    {
                        text.Length--; // Remove last character
                    }
                    // Handle Enter to validate input
                    else if (key == Keys.Enter)
                    {
                        ValidateInput();
                        isActive = false; // Deactivate after validation
                    }
                    else
                    {
                        // Add character if it's numeric or valid
                        char c = ConvertKeyToChar(key, keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift));
                        if (IsNumericOrValid(c)) text.Append(c);
                    }
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border
        Rectangle borderRectangle = new Rectangle(bounds.X - borderWidth, bounds.Y - borderWidth, bounds.Width + borderWidth * 2, bounds.Height + borderWidth * 2);
        spriteBatch.Draw(borderTexture, borderRectangle, borderColor);

        // Draw background
        spriteBatch.Draw(backgroundTexture, bounds, backgroundColor);

        // Draw the text inside the textbox
        spriteBatch.DrawString(font, text.ToString(), new Vector2(bounds.X + 5, bounds.Y + 5), textColor);
    }

    public void Activate() => isActive = true;
    public void Deactivate() => isActive = false;

    private void ValidateInput()
    {
        if (int.TryParse(text.ToString(), out int enteredNumber))
        {
            IsValid = enteredNumber == targetNumber; // Check if the number matches the target
        }
        else
        {
            IsValid = false; // Invalid input
        }
    }

    private char ConvertKeyToChar(Keys key, bool shift)
    {
        // Map number keys
        if (key >= Keys.D0 && key <= Keys.D9)
        {
            return (char)(key - Keys.D0 + '0');
        }
        if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
        {
            return (char)(key - Keys.NumPad0 + '0');
        }
        return '\0'; // Unsupported characters
    }

    private bool IsNumericOrValid(char c)
    {
        return char.IsDigit(c); // Only allow numeric characters
    }

    public void Clear()
    {
        text.Clear();    // Clear the current text
        IsValid = false; // Reset validation status
    }
}
