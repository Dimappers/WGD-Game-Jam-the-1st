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

        RenderTarget2D standardRenderTarget;

        GameMode gameMode;

        HeadPiece head;
        MapManager mapManager;

        int timeSinceLastJump = 0;

        KeyboardState oldState;
        KeyboardState newState;

        DirectionToMove direction;
        double totalMiliStart =-1;
        public int score;
        SpriteFont font;

        Random random = new Random();

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
            standardRenderTarget = new RenderTarget2D(GraphicsDevice, 1600, 1200);
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

            mapManager = new MapManager(40, mapTexs, head, this);

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

                int randomTime = 500 + (int)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 1000.0f) * 12 * score);

                if (timeSinceLastJump > randomTime)
                {
                    head.Update(gameTime, direction, new Point(-1, -1));
                    timeSinceLastJump = 0;
                    mapManager.MoveWalls();
                }

                if (head.isDead())
                {
                    gameMode = GameMode.endScreen;
                }

                oldState = newState;
                base.Update(gameTime);
            }
        
            else
            {
                if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
                {
                    gameMode = GameMode.mainGame;
                    reset();
                    //TODO: Write reset methods
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.DarkGreen);
            if (gameMode == GameMode.frontScreen)
            {
                GraphicsDevice.Clear(Color.Green);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Magic Mooshmooms", new Vector2(50, 100), Color.White);
                spriteBatch.DrawString(font, "Collect the mushrooms, but know your limits!", new Vector2(50, 200), Color.White);
                spriteBatch.DrawString(font, "By Kim, Tom and Mark", new Vector2(50, 500), Color.White);
                spriteBatch.DrawString(font, "Press space to start", new Vector2(50, 300), Color.White);
                spriteBatch.End();
            }
            else if (gameMode == GameMode.mainGame)
            {
                //Render the game to a render target
                GraphicsDevice.SetRenderTarget(standardRenderTarget);
                GraphicsDevice.Clear(Color.DarkGreen);

                spriteBatch.Begin();
                mapManager.Draw(spriteBatch, gameTime);
                head.Draw(gameTime, spriteBatch);
                

                spriteBatch.End();

                Texture2D screen = (Texture2D)standardRenderTarget;
              
                //Render the game on to the back buffer scaling as required
                GraphicsDevice.SetRenderTarget(null); //set the back buffer as our render target
                GraphicsDevice.Clear(Color.DarkGreen);
                spriteBatch.Begin();

                /*float scaleCentre = Math.Max(0.75f, 1 - (score) * 0.05f);
                float scaleFactor = 1.0f;
                float scaleConstant = 2.0f - scaleCentre;

                //Console.WriteLine(scaleCentre + ", " + scaleConstant);
                float cosElement = (float)Math.Cos((float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f * Math.Log(Math.Log(score + 2)));
                cosElement += scaleCentre;
                cosElement *= scaleFactor;
                float scale = (cosElement * scaleCentre) + 1.25f;*/
                //float cosElement = (float)Math.Cos((float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f * Math.Log(Math.Log(score + 2)));
                //float scale = (cosElement + 1.25f) * Math.Min(2.0f, (score * 0.05f) + 1.0f);

                //scale = cosElement + 1.25f * scaleCentre;
                /*float scaleCentre = Math.Max(0.75f, 1 - (score) * 0.05f);
                float scaleFactor = 1.0f;
                float scaleConstant = 2.0f - scaleCentre;

                Console.WriteLine(scaleCentre + ", " + scaleConstant);
                float cosElement = (float)Math.Cos((float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f * Math.Log(Math.Log(score + 2)));
                cosElement += scaleCentre;
                cosElement *= scaleFactor;
                float scale = cosElement * scaleCentre + scaleConstant;*/
                float scale = 1.0f;
                if (score > 8)
                {
                    if (totalMiliStart < 0)
                    {
                        totalMiliStart = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    float cosElement = (float)Math.Sin((float)(gameTime.TotalGameTime.TotalMilliseconds - totalMiliStart) / 1000.0f * Math.Log(Math.Log(score - 6)));
                    scale = (cosElement * 0.75f) + 1.25f;
                }

                float scaledWidth = 800 * scale;
                float scaledHeight = 600 * scale;
                
                Rectangle sourceRectangle = new Rectangle((int)((screen.Width - scaledWidth) * 0.5f), (int)((screen.Height - scaledHeight) * 0.5), (int)scaledWidth, (int)scaledHeight);
                spriteBatch.Draw(screen, new Rectangle(0,0, 800, 600), sourceRectangle, Color.White);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.Black);

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.Green);
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(200, 200), Color.White);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(200, 300), Color.White);
                spriteBatch.DrawString(font, "Press space to restart", new Vector2(200, 400), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void addToScore() {score++;}

        private void reset()
        {
            mapManager.reset();
            head.reset();
            score = 0;
        }
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
