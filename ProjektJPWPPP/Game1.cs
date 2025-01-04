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
       
        Texture2D buttonTexture, playerTexture, enemyTexture ,backgroundTexture;
        SpriteFont ButtonFont, MessageFont;
        Button startButton, button1, button2;
        Player player;
        Enemy enemy;
        int _gameState, difficulty, level, number1, number2, liczbaOdpowiedzi, liczbaPoprawnychOdpowiedzi;
        Textbox wynikTextbox;
        KeyboardState keyboardState = Keyboard.GetState();
        KeyboardState previousKeyboardState;
        Color tlo;
        bool czyWygrana;

        private float _timer;     // Timer 
        private bool _timerRunning, _czyWalka, _akcja;
        private Random _random; //liczba potrzebna do losowan dzialan

        //---poruszanie sie
        private Texture2D characterTexture;
        private Rectangle characterRectangle;

        private Texture2D Door1Texture;
        private Rectangle Door1Rectangle;
        private Texture2D Door2Texture;
        private Rectangle Door2Rectangle;
        private Texture2D Door3Texture;
        private Rectangle Door3Rectangle;
        private Texture2D Door4Texture;
        private Rectangle Door4Rectangle;

        private Vector2 characterVelocity;
        private float speed = 200f;
        //---

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
            liczbaOdpowiedzi = liczbaPoprawnychOdpowiedzi = 0;
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

            backgroundTexture= Content.Load<Texture2D>("Doors/MathBackground");

            // Load character texture
            characterTexture = Content.Load<Texture2D>("PlayerEnemy/gracz_small");
            characterRectangle = new Rectangle((_graphics.PreferredBackBufferWidth / 2)-85, (_graphics.PreferredBackBufferHeight / 2)-91, 191, 182);

            // Load obstacle texture
            Door1Texture = Content.Load<Texture2D>("Doors/dodawanie");
            Door1Rectangle = new Rectangle(200, 200, 72, 97);
            Door2Texture = Content.Load<Texture2D>("Doors/odejmowanie");
            Door2Rectangle = new Rectangle(200, 800, 72, 97);
            Door3Texture = Content.Load<Texture2D>("Doors/mnozenie");
            Door3Rectangle = new Rectangle(1000, 200, 72, 97);
            Door4Texture = Content.Load<Texture2D>("Doors/dzielenie");
            Door4Rectangle = new Rectangle(1000, 800, 72, 97);




            buttonTexture = Content.Load<Texture2D>("Controls/button"); 
            ButtonFont = Content.Load<SpriteFont>("Fonts/Button"); 
            MessageFont = Content.Load<SpriteFont>("Fonts/Message"); 
            startButton = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2)-81, (_graphics.PreferredBackBufferHeight / 2) - 31), "Start", ButtonFont, Color.Black);
            button1 = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512), "Średni", ButtonFont, Color.Black);
            button2 = new Button(buttonTexture, new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 768), "Trudny", ButtonFont, Color.Black);

            playerTexture= Content.Load<Texture2D>("PlayerEnemy/gracz_placeholder");
            player = new Player(playerTexture);

            enemyTexture= Content.Load<Texture2D>("PlayerEnemy/przeciwnik1_placeholder");
            enemy= new Enemy(enemyTexture);

            wynikTextbox = new Textbox(GraphicsDevice, ButtonFont, new Rectangle(830, (_graphics.PreferredBackBufferHeight / 2) + 250, 200, 30), Color.White, Color.Black, 42, Color.Black); // Target number is 42


        }


        MouseState currentMouseState;
        MouseState previousMouseState;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(_gameState==4)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                characterVelocity = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.W)) characterVelocity.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.S)) characterVelocity.Y += 1;
                if (keyboardState.IsKeyDown(Keys.A)) characterVelocity.X -= 1;
                if (keyboardState.IsKeyDown(Keys.D)) characterVelocity.X += 1;

                // Normalize diagonal movement
                if (characterVelocity.Length() > 1)
                    characterVelocity.Normalize();

                // Apply speed and elapsed time
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 moveAmount = characterVelocity * speed * deltaTime;

                // Update character position
                characterRectangle.X += (int)moveAmount.X;
                characterRectangle.Y += (int)moveAmount.Y;

                // Collision detection
                if (characterRectangle.Intersects(Door1Rectangle))
                {
                    level = 1;
                    _gameState = 1;
                    characterRectangle.X = (_graphics.PreferredBackBufferWidth / 2) - 191;
                }
                if (characterRectangle.Intersects(Door2Rectangle))
                {
                    level = 2;
                    _gameState = 1;
                    characterRectangle.X = (_graphics.PreferredBackBufferWidth / 2) - 191;
                }
                if (characterRectangle.Intersects(Door3Rectangle))
                {
                    level = 3;
                    _gameState = 1;
                    characterRectangle.X = (_graphics.PreferredBackBufferWidth / 2) - 191;
                }
                if (characterRectangle.Intersects(Door4Rectangle))
                {
                    level = 4;
                    _gameState = 1;
                    characterRectangle.X = (_graphics.PreferredBackBufferWidth / 2) - 191;
                }
            }
           
            //------------------------------------------------------------------------------------------------------------
            // TODO: Add your update logic here

            currentMouseState = Mouse.GetState();
            startButton.Update(currentMouseState, previousMouseState);
            button1.Update(currentMouseState, previousMouseState);
            button2.Update(currentMouseState, previousMouseState);
            player.Update(gameTime);
            enemy.Update(gameTime);

            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();



            wynikTextbox.Update(keyboardState, previousKeyboardState);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                wynikTextbox.Activate();
            }

           


            if (_gameState == 0) //menu startowe
            {
                if(startButton.IsPressed)
                {
                    startButton.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81,256));
                    startButton.setText("Łatwy");
                    tlo = Color.Beige;
                    _gameState = 4;
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
            else if(_gameState == 2)
            {
                startButton.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 150));
                startButton.setText("Atak");
                button1.setPosition(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 250));
                button1.setText("Leczenie");
                button2.setPosition(new Vector2(1150, _graphics.PreferredBackBufferHeight-50));
                button2.setText("Menu");
                if (button2.IsPressed) // jezeli gracz wybierze przycisk menu
                {
                    startButton.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 256));
                    startButton.setText("Kontynuuj");
                    button1.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512));
                    button1.setText("Zamknij program");
                    _gameState = 3;
                }
                else if (startButton.IsPressed || button1.IsPressed)
                {
                    if (button1.IsPressed) _akcja = false;
                    else _akcja = true;

                    _czyWalka = true;
                    if (level == 1)
                    {
                        if (difficulty == 1)
                        {
                            number1 = _random.Next(0, 100);
                            number2 = _random.Next(0, 100 - number1);
                        }
                        else if (difficulty == 2)
                        {
                            number1 = _random.Next(0, 1000);
                            number2 = _random.Next(0, 1000 - number1);
                        }
                        else
                        {
                            number1 = _random.Next(0, 20);
                            number2 = _random.Next(0, 20 - number1);
                        }
                        wynikTextbox.setTargetNumber(number1 + number2);
                    }// generowanie dzialan zaleznych od poziomu
                    if (level == 2)
                    {
                        if (difficulty == 1)
                        {
                            number1 = _random.Next(0, 100);
                            number2 = _random.Next(0,  number1);
                        }
                        else if (difficulty == 2)
                        {
                            number1 = _random.Next(0, 1000);
                            number2 = _random.Next(0,  number1);
                        }
                        else
                        {
                            number1 = _random.Next(0, 20);
                            number2 = _random.Next(0, number1);
                        }
                        wynikTextbox.setTargetNumber(number1 - number2);
                    }
                    if (level == 3)
                    {
                        if (difficulty == 1)
                        {
                            number1 = _random.Next(0, 10);
                            number2 = _random.Next(0, 10);
                        }
                        else if (difficulty == 2)
                        {
                            number1 = _random.Next(0, 31);
                            number2 = _random.Next(0, 31);
                        }
                        else
                        {
                            number1 = _random.Next(0, 10);
                            number2 = _random.Next(0, 10-number1+1);
                        }
                        wynikTextbox.setTargetNumber(number1 * number2);
                    }
                    if (level == 4)
                    {
                        if (difficulty == 1)
                        {
                            number2 = _random.Next(1, 10);
                            number1 = number2 * _random.Next(0, 25); 
                        }
                        else if (difficulty == 2)
                        {
                            number2 = _random.Next(1, 10); 
                            number1 = number2 * _random.Next(0, 10); 
                        }
                        else
                        {
                            number2 = _random.Next(1, 5); 
                            number1 = number2 * _random.Next(0, 10);
                        }

                        wynikTextbox.setTargetNumber(number1 / number2);
                    }// dzielenie ktorego wynikiem jest liczba calkowita


                }

                else if (_czyWalka)
                {
                    _timerRunning = true;
                    if (keyboardState.IsKeyDown(Keys.Enter)) //po nacisnieciu klawisza enter sprawdza czy wynik jest poprawny i wykonuje odpowiednia akcje
                    {
                        if (wynikTextbox.getIsValid())
                        {
                            liczbaPoprawnychOdpowiedzi++;
                            if (_akcja) enemy.TakeDamage(20);
                            else player.Heal(50);
                        }
                        liczbaOdpowiedzi++;
                        _timerRunning = false;
                        wynikTextbox.setTargetNumber(99999999);
                        wynikTextbox.Clear();
                        _czyWalka = false;
                        player.TakeDamage(10);
                    }


                }

                if(enemy.getHealth()==0||player.getHealth()==0) //jeżeli gracz wygra/przegra
                {
                    if (player.getHealth() == 0)
                    {
                        czyWygrana = false;
                    }
                    else czyWygrana = true;
                    enemy.setHealth(100);
                    player.Heal(100);
                    
                    _gameState = 5;
                    button1.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512));
                    button1.setText("Kontynuuj");
                }
            }
            else if(_gameState == 3) 
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
            else if (_gameState == 5)
            {
                if(button1.IsPressed)
                {
                    _gameState = 4;
                    startButton.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 256));
                    startButton.setText("Łatwy");
                    button1.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 512));
                    button1.setText("Średni");
                    button2.setPosition(new Vector2((_graphics.PreferredBackBufferWidth / 2) - 81, 768));
                    button2.setText("Trudny");
                    _timer = 0;
                    liczbaOdpowiedzi = 0;
                    liczbaPoprawnychOdpowiedzi = 0;
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

            /* stany gry:
               0 - ekran startowy
               1 - wybor poziomu trudnosci - poziomy trudnosci roznia sie maksymalnymi liczbami jakie moga sie wylosowac
               2 - ekran walki
               3 - ekran menu pauzy
               4 - ekran wyboru poziomow za pomoca poruszania sie postacia
               5 - ekran z wynikami po wygraniu/przegraniu walki
            */
            _spriteBatch.Begin();
            if (_gameState == 0)
            {
                _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                _spriteBatch.DrawString(MessageFont, "Pokonaj matematykę!", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 280, 40), Color.Black);
                startButton.Draw(_spriteBatch);

            }
            else if (_gameState == 1)
            {
                _spriteBatch.DrawString(MessageFont, "Wybierz poziom trudności", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 340, 40), Color.Black);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch);
                button2.Draw(_spriteBatch);
            }
            else if (_gameState == 2)
            {
                if (level == 1) _spriteBatch.DrawString(MessageFont, "Poziom 1 - dodawanie", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                else if (level == 2) _spriteBatch.DrawString(MessageFont, "Poziom 2 - odejmowanie", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                else if (level == 3) _spriteBatch.DrawString(MessageFont, "Poziom 3 - mnożenie", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                else if (level == 4) _spriteBatch.DrawString(MessageFont, "Poziom 4 - dzielenie", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                _spriteBatch.DrawString(MessageFont, "Zdrowie: " + player.getHealth(), new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) + 50), Color.Black);
                _spriteBatch.DrawString(MessageFont, "Zdrowie: " + enemy.getHealth(), new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 50), Color.Black);
                player.Draw(_spriteBatch);
                enemy.Draw(_spriteBatch);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch);
                button2.Draw(_spriteBatch);


                wynikTextbox.Draw(_spriteBatch);

                _spriteBatch.DrawString(MessageFont, $"Czas: {_timer:F2} sekund", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 600, 900), Color.Black);

                if (_czyWalka)
                {
                    if (level == 1) _spriteBatch.DrawString(MessageFont, $"{number1} + {number2} = {wynikTextbox.getTargetNumber()}", new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 150), Color.Black);
                    else if (level == 2) _spriteBatch.DrawString(MessageFont, $"{number1} - {number2} = {wynikTextbox.getTargetNumber()}", new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 150), Color.Black);
                    else if (level == 3) _spriteBatch.DrawString(MessageFont, $"{number1} * {number2} = {wynikTextbox.getTargetNumber()}", new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 150), Color.Black);
                    else if (level == 4) _spriteBatch.DrawString(MessageFont, $"{number1} / {number2} = {wynikTextbox.getTargetNumber()}", new Vector2(830, (_graphics.PreferredBackBufferHeight / 2) + 150), Color.Black);
                }
            }
            else if (_gameState == 3)
            {
                _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                _spriteBatch.DrawString(MessageFont, "Menu", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 80, 40), Color.Black);
                startButton.Draw(_spriteBatch);
                button1.Draw(_spriteBatch);




            }
            else if (_gameState == 4)
            {
                // Narysuj postać
                _spriteBatch.Draw(characterTexture, characterRectangle, Color.Beige);

                // Narysuj drzwi
                _spriteBatch.Draw(Door1Texture, Door1Rectangle, Color.Beige);
                _spriteBatch.Draw(Door2Texture, Door2Rectangle, Color.Beige);
                _spriteBatch.Draw(Door3Texture, Door3Rectangle, Color.Beige);
                _spriteBatch.Draw(Door4Texture, Door4Rectangle, Color.Beige);
            }
            else if (_gameState==5)
            {
                _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                if (czyWygrana) _spriteBatch.DrawString(MessageFont, "Pokonałeś przeciwnika!", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 300, 40), Color.Black);
                else _spriteBatch.DrawString(MessageFont, "Nie udało ci się pokonać przeciwnika.", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 500, 40), Color.Black);
                _spriteBatch.DrawString(MessageFont, $"Liczba poprawnych odpowiedzi: {liczbaPoprawnychOdpowiedzi}/{liczbaOdpowiedzi}", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 500, 150), Color.Black);
                _spriteBatch.DrawString(MessageFont, $"Czas poświęcony na obliczenia: {_timer:F2}", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 550, 250), Color.Black);
                button1.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
