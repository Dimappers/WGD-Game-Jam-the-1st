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

namespace WGDGameJam
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        HeadPiece head;
        MapManager mapManager;
        int timeSinceLastJump = 0;

        KeyboardState oldState;
        KeyboardState newState;

        DirectionToMove direction;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
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
            oldState = Keyboard.GetState();
            newState = oldState;
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
            Texture2D headTexture = Content.Load<Texture2D>("drawing//Cow_Head");
            Texture2D mainTexture = Content.Load<Texture2D>("drawing//Cow_Middle");
            Texture2D tailTexture = Content.Load<Texture2D>("drawing//Cow_Bum");
            Texture2D grassTexture = Content.Load<Texture2D>("drawing//grass");
            Texture2D hedgeTexture = Content.Load<Texture2D>("drawing//hedge");
            Texture2D foodTexture = Content.Load<Texture2D>("drawing//food");

            head = new HeadPiece(headTexture, mainTexture, tailTexture, this);
            CowPiece tail = new CowPiece(mainTexture, tailTexture);
            CowPiece a = new CowPiece(mainTexture, tailTexture);
            CowPiece b = new CowPiece(mainTexture, tailTexture);
            CowPiece c = new CowPiece(mainTexture, tailTexture);
            CowPiece d = new CowPiece(mainTexture, tailTexture);
            head.AttachPiece(tail);
            head.AttachPiece(a);
            head.AttachPiece(b);
            head.AttachPiece(c);
            head.AttachPiece(d);

            mapManager = new MapManager(100, grassTexture, hedgeTexture, foodTexture, head);

            // TODO: use this.Content to load your game content here
        }

        public MapManager GetMapManager()
        {
            return mapManager;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            timeSinceLastJump += gameTime.ElapsedGameTime.Milliseconds;

            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right)) { direction = DirectionToMove.right; }
            else if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left)) { direction = DirectionToMove.left; }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) { direction = DirectionToMove.up; }
            else if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) { direction = DirectionToMove.down; }

            oldState = newState;

            if (timeSinceLastJump > 500)
            {
                head.Update(gameTime, direction, new Point(-1,-1));
                timeSinceLastJump = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            mapManager.Draw(spriteBatch);
            head.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
