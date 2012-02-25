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

        GameMode gameMode;

        HeadPiece head;
        MapManager mapManager;
        int timeSinceLastJump = 0;

        KeyboardState oldState;
        KeyboardState newState;

        DirectionToMove direction;

        public int score;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";

            gameMode = GameMode.frontScreen;
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
            score = 0;    
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MapTextures mapTexs = new MapTextures();
            Texture2D headTexture = Content.Load<Texture2D>("drawing//Cow_Head");
            Texture2D mainTexture = Content.Load<Texture2D>("drawing//Cow_Middle");
            Texture2D tailTexture = Content.Load<Texture2D>("drawing//Cow_Bum");
            mapTexs.grassTexture = Content.Load<Texture2D>("drawing//grass");
            mapTexs.hedgeTexture = Content.Load<Texture2D>("drawing//hedge");
            mapTexs.hedgeVertTexture = Content.Load<Texture2D>("drawing/hedge_v");
            mapTexs.foodTexture = Content.Load<Texture2D>("drawing//food");
            mapTexs.hedgeCornerTexturetl = Content.Load<Texture2D>("drawing//hedge_corner_tl");
            mapTexs.hedgeCornerTexturetr = Content.Load<Texture2D>("drawing//hedge_corner_tr");
            mapTexs.hedgeCornerTexturebl = Content.Load<Texture2D>("drawing//hedge_corner_bl");
            mapTexs.hedgeCornerTexturebr = Content.Load<Texture2D>("drawing//hedge_corner_br");
            mapTexs.hedgeCrossTexture = Content.Load<Texture2D>("drawing//hedge_cross");
            mapTexs.foodTexture = Content.Load<Texture2D>("drawing//mushroom");
            font = Content.Load<SpriteFont>("DefaultFont");

            head = new HeadPiece(headTexture, mainTexture, tailTexture, this);
            CowPiece tail = new CowPiece(mainTexture, tailTexture, this);
            CowPiece a = new CowPiece(mainTexture, tailTexture, this);
            CowPiece b = new CowPiece(mainTexture, tailTexture, this);
            CowPiece c = new CowPiece(mainTexture, tailTexture, this);
            CowPiece d = new CowPiece(mainTexture, tailTexture, this);
            head.AttachPiece(tail);
            /*head.AttachPiece(a);
            head.AttachPiece(b);
            head.AttachPiece(c);
            head.AttachPiece(d);*/

            mapManager = new MapManager(20, mapTexs, head, this);

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
            newState = Keyboard.GetState();
            if (gameMode == GameMode.frontScreen)
            {
                if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
                {
                    gameMode = GameMode.mainGame;
                }
            }
            else if (gameMode == GameMode.mainGame)
            {
                timeSinceLastJump += gameTime.ElapsedGameTime.Milliseconds;


                if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right)) { direction = DirectionToMove.right; }
                else if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left)) { direction = DirectionToMove.left; }
                else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) { direction = DirectionToMove.up; }
                else if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) { direction = DirectionToMove.down; }

                if (timeSinceLastJump > 500)
                {
                    head.Update(gameTime, direction, new Point(-1, -1));
                    timeSinceLastJump = 0;
                }

                if (head.isDead())
                {
                    gameMode = GameMode.endScreen;
                }
            }

            oldState = newState;
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
            if (gameMode == GameMode.frontScreen)
            {
                spriteBatch.DrawString(font, "Magic Mooshrooms", new Vector2(50, 200), Color.IndianRed);
                spriteBatch.DrawString(font, "By Kim, Tom and Mark", new Vector2(50, 300), Color.Indigo);
            }
            else if (gameMode == GameMode.mainGame)
            {
                mapManager.Draw(spriteBatch, this);
                head.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.Black);
            }
            else
            {
                GraphicsDevice.Clear(Color.PowderBlue);
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(200, 200), Color.Red);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(200, 300), Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void addToScore() {score++;}
    }

    public struct MapTextures
    {
        public Texture2D grassTexture;
        public Texture2D hedgeTexture;
        public Texture2D hedgeVertTexture;
        public Texture2D hedgeCornerTexturetl;
        public Texture2D hedgeCornerTexturetr;
        public Texture2D hedgeCornerTexturebl;
        public Texture2D hedgeCornerTexturebr;
        public Texture2D foodTexture;
        public Texture2D hedgeCrossTexture;
    }

    enum GameMode
    {
        frontScreen,
        mainGame,
        endScreen
    }
}
