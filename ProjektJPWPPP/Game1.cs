using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

// ---------------------------------------------------PROJEKT ZROBIONY ZA POMOCĄ FRAMEWORKU MONOGAME------------------------------------

namespace ProjektJPWPPP
{
    public class Game1 : Game
    {
       
        Texture2D buttonTexture, playerTexture, enemyTexture;
        SpriteFont ButtonFont, MessageFont;
        Button startButton, button1, button2;
        Player player;
        Enemy enemyLevel1;
        int _gameState, difficulty, number1, number2;
        Textbox wynikTextbox;
        KeyboardState keyboardState = Keyboard.GetState();
        KeyboardState previousKeyboardState;
        Color tlo;

        private float _timer;     // Timer 
        private bool _timerRunning, _czyWalka, _akcja;
        private Random _random; //liczba potrzebna do losowan dzialan


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

     
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280; 
            _graphics.PreferredBackBufferHeight = 1024; 
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            
            IsMouseVisible = true;
            _gameState = 0;
            tlo = Color.CornflowerBlue;
            _random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _czyWalka = false;




           
            
            buttonTexture = Content.Load<Texture2D>("Controls/button"); 
            ButtonFont = Content.Load<SpriteFont>("Fonts/Button"); 
            MessageFont = Content.Load<SpriteFont>("Fonts/Message"); 
            startButton = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2)-81, (_graphics.PreferredBackBufferHeight / 2) - 31), "Start", ButtonFont, Color.Black);
            button1 = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512), "Średni", ButtonFont, Color.Black);
            button2 = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 768), "Trudny", ButtonFont, Color.Black);

            playerTexture= Content.Load<Texture2D>("PlayerEnemy/gracz_placeholder");
            player = new Player(playerTexture);

            enemyTexture= Content.Load<Texture2D>("PlayerEnemy/przeciwnik1_placeholder");
            enemyLevel1= new Enemy(enemyTexture);

            wynikTextbox = new Textbox(GraphicsDevice, ButtonFont, new Rectangle(830, (_graphics.PreferredBackBufferHeight / 2) + 250, 200, 30), Color.White, Color.Black, 42, Color.Black); // Target number is 42


        }


        MouseState currentMouseState;
        MouseState previousMouseState;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here

            currentMouseState = Mouse.GetState();
            startButton.Update(currentMouseState, previousMouseState);
            button1.Update(currentMouseState, previousMouseState);
            button2.Update(currentMouseState, previousMouseState);
            player.Update(gameTime);
            enemyLevel1.Update(gameTime);

            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();



            wynikTextbox.Update(keyboardState, previousKeyboardState);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                wynikTextbox.Activate();
            }

            // Check if the number entered is valid
            if (!wynikTextbox.IsValid && !wynikTextbox.getisActive())
            {
                Console.WriteLine("Niepoprawny wynik");
            }
            else if (wynikTextbox.IsValid)
            {
                Console.WriteLine("Poprawny wynik");
            }


            if (_gameState == 0) //menu startowe
            {
                if(startButton.IsPressed)
                {
                    startButton.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81,256));
                    startButton.setText("Łatwy");
                    tlo = Color.Beige;
                    _gameState = 1;
                }
            }
            else if(_gameState == 1) //poziom trudnosci
            {
                if (startButton.IsPressed)
                {
                    difficulty = 0;
                    _gameState = 2;
                }
                else if (button1.IsPressed)
                {
                    difficulty = 1;
                    _gameState = 2;
                }
                else if (button2.IsPressed)
                {
                    difficulty = 2;
                    _gameState = 2;
                }
                
            }
            else if(_gameState == 2) //walka
            {
                startButton.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 150));
                startButton.setText("Atak");
                button1.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 250));
                button1.setText("Leczenie");
                button2.setPosition(new Vector2(1150, _graphics.PreferredBackBufferHeight-50));
                button2.setText("Menu");
                if (button2.IsPressed)
                {
                    startButton.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 256));
                    startButton.setText("Kontynuuj");
                    button1.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512));
                    button1.setText("Zamknij program");
                    _gameState = 3;
                }
                else if(startButton.IsPressed)
                {
                    _czyWalka = true;
                    number1=_random.Next(0, 50);
                    number2 = _random.Next(0, 100-number1);
                    wynikTextbox.setTargetNumber(number1 + number2);
                    _akcja = true;
                }
                else if(button1.IsPressed)
                {
                    _czyWalka = true;
                    number1 = _random.Next(0, 50);
                    number2 = _random.Next(0, 100 - number1);
                    wynikTextbox.setTargetNumber(number1 + number2);
                    _akcja = false;
                }

                else if(_czyWalka)
                {
                    _timerRunning = true;
                    if(keyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (wynikTextbox.getIsValid())
                        {
                            if (_akcja) enemyLevel1.TakeDamage(20);
                            else player.Heal(50);
                        }
                        else
                        {

                        }
                        _timerRunning = false;
                        wynikTextbox.setTargetNumber(99999999);
                        wynikTextbox.Clear();
                        _czyWalka = false;
                        player.TakeDamage(10);
                    }
                    
                    
                }
            }
            else if(_gameState == 3) //menu
            {
                _timerRunning = false;
                if (startButton.IsPressed)
                {
                    _gameState=2;
                    startButton.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 150));
                    startButton.setText("Atak");
                    button1.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 250));
                    button1.setText("Leczenie");
                    button2.setPosition(new Vector2(1150, _graphics.PreferredBackBufferHeight - 50));
                    button2.setText("Menu");
                }

                if (button1.IsPressed)
                {
                    Exit();
                }
            }

            previousMouseState = currentMouseState;

            if (_timerRunning)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds; // Zwiekszenie timera
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(tlo);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if(_gameState == 0)
            {
                _spriteBatch.DrawString(MessageFont, "Pokonaj matematykę!", new Vector2((_graphics.PreferredBackBufferWidth / 2)-160, 40), Color.Black);
                startButton.Draw(_spriteBatch);

            }
            else if(_gameState == 1)
            {
                _spriteBatch.DrawString(MessageFont, "Wybierz poziom trudności", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 190, 40), Color.Black);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch);
                button2.Draw(_spriteBatch);
            }
            else if (_gameState == 2)
            {
                _spriteBatch.DrawString(MessageFont, "Poziom 1 - dodawanie", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                _spriteBatch.DrawString(MessageFont, "Zdrowie: "+player.getHealth(), new Vector2(50, (_graphics.PreferredBackBufferHeight / 2)+50), Color.Black);
                _spriteBatch.DrawString(MessageFont, "Zdrowie: " + enemyLevel1.getHealth(), new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 50), Color.Black);
                player.Draw(_spriteBatch);
                enemyLevel1.Draw(_spriteBatch);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch );
                button2.Draw(_spriteBatch);

                
                wynikTextbox.Draw(_spriteBatch);

                _spriteBatch.DrawString(MessageFont, $"Czas: {_timer:F2} sekund", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 900), Color.Black);

                if(_czyWalka) _spriteBatch.DrawString(ButtonFont, $"{number1} + {number2} = {wynikTextbox.getTargetNumber()}", new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 150), Color.Black);
                    

            }
            else if (_gameState == 3)
            {
                _spriteBatch.DrawString(MessageFont, "Menu", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 100, 40), Color.Black);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch);
                button2.Draw(_spriteBatch);



            }



            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
