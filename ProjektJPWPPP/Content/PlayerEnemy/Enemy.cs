using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enemy
{

    private int HealthPoints { get; set; }
    private Vector2 Position { get; set; }
    private Texture2D Texture { get; set; }


    public Enemy(Texture2D texture)
    {
        this.Texture = texture;
        this.Position = new Vector2(830, 150);
        this.HealthPoints = 100;
    }


    public void Update(GameTime gameTime)
    {

        if (HealthPoints < 0)
            HealthPoints = 0;
        if (HealthPoints > 100)
            HealthPoints = 100;
    }


    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;

    }

    public int getHealth()
    {
        return HealthPoints;
    }
    public void setHealth(int health)
    {
        this.HealthPoints = health;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        if (Texture != null)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
