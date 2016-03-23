using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombiePong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, spritesheet;

        Sprite paddle1, paddle2, ball;
        Random rand = new Random();
        const int HEIGHT = 768;
        float speed = 300f;
        int points = 0;
        

        List<Sprite> zombies = new List<Sprite>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("background");
            spritesheet = Content.Load<Texture2D>("spritesheet");

            paddle1 = new Sprite(new Vector2(20, 20), spritesheet, new Rectangle(0, 516, 25, 150), Vector2.Zero);
            paddle2 = new Sprite(new Vector2(970, 20), spritesheet, new Rectangle(32, 516, 25, 150), Vector2.Zero);
            ball = new Sprite(new Vector2(700, 350), spritesheet, new Rectangle(76, 510, 40, 40), new Vector2(-300, 0));

            SpawnZombie(new Vector2(400, 400), new Vector2(-20, 0));
            SpawnZombie(new Vector2(580, 520), new Vector2(20, 0));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SpawnZombie(Vector2 location, Vector2 velocity)
        {
            Sprite zombie = new Sprite(location, spritesheet, new Rectangle(0, 25, 160, 150), velocity);

            for (int i = 1; i < 10; i++)
            {
                zombie.AddFrame(new Rectangle(i * 165, 25, 160, 150));
            }

            zombies.Add(zombie);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState ms = Mouse.GetState();

            paddle1.Location = new Vector2(paddle1.Location.X, ms.Y);

            if (ball.Location.Y >= HEIGHT - 16)
                ball.Velocity = new Vector2(ball.Velocity.X, ball.Velocity.Y * -1);

            if (ball.Location.Y <= 0)
                ball.Velocity = new Vector2(ball.Velocity.X, ball.Velocity.Y * -1);

            if (paddle1.Location.Y <= 0)
                paddle1.Location = new Vector2(paddle1.Location.X, 0);
            if (paddle1.Location.Y >= HEIGHT)
                paddle1.Location = new Vector2(paddle1.Location.X, HEIGHT);

            

            //paddle2.Location = new Vector2(paddle2.Location.X, ball.Center.Y);

            if (ball.IsBoxColliding(paddle1.BoundingBoxRect) && ball.Location.Y != paddle1.Center.Y)
            {
                ball.Velocity = new Vector2(ball.Velocity.X * -1.000000000036f, (float)Math.Cos((float)Math.Abs(ball.Location.Y - paddle1.Center.Y)) * yayyya100);
                speed *= 1.1f;
                Vector2 velocity = ball.Velocity;
                velocity.Normalize();
                velocity *= speed;
                ball.Velocity = velocity;
            }


            if (ball.IsBoxColliding(paddle2.BoundingBoxRect) && ball.Location.Y != paddle2.Center.Y)
            {
                ball.Velocity = new Vector2(ball.Velocity.X * -1.000000000036f, (float)Math.Cos(ball.Location.Y - paddle2.Center.Y) * 100);
                speed *= 1.1f;
                Vector2 velocity = ball.Velocity;
                velocity.Normalize();
                velocity *= speed;
                ball.Velocity = velocity;
            }

            Window.Title = "Points: " + points;

            speed = Math.Min(750, speed);


            /*PADDLE 2 AI*/

            /*
            if (paddle2.Location.Y <= 0)
                paddle2.Location = new Vector2(paddle2.Location.X, 0);
            if (paddle2.Location.Y >= HEIGHT)
                paddle2.Location = new Vector2(paddle2.Location.X, HEIGHT);
            paddle2.Location = new Vector2(paddle2.Location.X, paddle2.Location.Y);
            
            if (paddle2.Location.Y >= 0 && paddle2.Location.Y <= 650) 
                paddle2.Location = new Vector2(paddle2.Location.X, paddle2.Location.Y + 2);
            */

            if (paddle2.Location.Y <= 0 || paddle2.BoundingBoxRect.Bottom >= 650)
                paddle2.Velocity = Vector2.Zero;

            float diff = paddle2.Center.Y - ball.Center.Y;

            if (Math.Abs(diff) > 40)
            {
                if (diff > 0)
                {
                    paddle2.Velocity = new Vector2(0, rand.Next(60, 90) * -1);
                }
                else
                {
                    paddle2.Velocity = new Vector2(0, 90);
                }
            }

            paddle2.Update(gameTime);

            //if (ball.Location.X <= 0)
            //    ball.Velocity = ball.Velocity * -1;

         
            
            
            


            // TODO: Add your update logic here

            if (ball.Location.X >= 1024)
            {
                ball.Location = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2); points += 10;

            }
            if (ball.Location.X <= 0)
                ball.Location = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);





            

            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombies[i].IsBoxColliding(ball.BoundingBoxRect))
                {
                    points += 5;
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.000000000036f, (float)Math.Cos(ball.Location.Y - zombies[i].Center.Y) * -100);
                    speed *= 1.1f;
                    Vector2 velocity = ball.Velocity;
                    velocity.Normalize();
                    velocity *= speed;
                    ball.Velocity = velocity;
                    zombies[i].Location = new Vector2(rand.Next(400, 800), rand.Next(20, 600));
                }

                zombies[i].Update(gameTime);

                // Zombie logic goes here.. 
                zombies[i].FlipHorizontal = zombies[i].Velocity.X > 0;

                
                if (zombies[i].Location.X >= 800 || zombies[i].Location.X <= 100)
                {
                    zombies[i].Velocity = new Vector2(zombies[i].Velocity.X*-1, zombies[i].Velocity.Y);
                }

            }
            ball.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
