using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Button
{
    private Texture2D texture;
    private Vector2 position;
    private Rectangle bounds;
    private string text;
    private SpriteFont font;
    private Color color;

    public bool IsHovered { get; private set; }
    public bool IsPressed { get; private set; }

    public Button(Texture2D texture, Vector2 position, string text, SpriteFont font, Color color)
    {
        this.texture = texture;
        this.position = position;
        this.text = text;
        this.font = font;
        this.color = color;
        this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }
    public void setPosition(Vector2 position)
    {
        this.position = position;
    }
    public void setText(string text)
    {
        this.text = text;
    }

    public void Update(MouseState mouseState, MouseState previousMouseState)
    {
        bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        Point mousePosition = mouseState.Position;

        // Sprawdz czy kursor jest nad przyciskiem
        IsHovered = bounds.Contains(mousePosition);

      
        if (IsHovered && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
        {
            IsPressed = true;
        }
        else
        {
            IsPressed = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
       
        spriteBatch.Draw(texture, position, IsHovered ? Color.Gray : Color.White);

       
        Vector2 textSize = font.MeasureString(text);
        Vector2 textPosition = position + new Vector2((bounds.Width - textSize.X) / 2, (bounds.Height - textSize.Y) / 2);
        spriteBatch.DrawString(font, text, textPosition, color);
    }
}