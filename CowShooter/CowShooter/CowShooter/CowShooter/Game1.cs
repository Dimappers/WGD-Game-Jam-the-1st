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

namespace CowShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background;

        WallManager wallManager;

        Catapult catapult;
        CowManager cowManager;
        CollisionManager collisionManager;
        VillagerManager villagerManager;
        MeatStore meatStore;

        MouseState currentMouseState, oldMouseState;
        KeyboardState currentKeyboardState, oldKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 450;
            collisionManager = new CollisionManager(400.0f);
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
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            oldMouseState = currentMouseState;
            oldKeyboardState = currentKeyboardState;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create cow manager
            cowManager = new CowManager(collisionManager, Content.Load<Texture2D>("art//healthBar1"), Content.Load<Texture2D>("art//healthBar2"));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("art//bg");
            Texture2D wallTexture = Content.Load<Texture2D>("art//Wall_Block");

            meatStore = new MeatStore(Content.Load<SpriteFont>("ScoreFont"), Content.Load<Texture2D>("art//storebackground"));


            cowManager.AddTexture(typeof(Cow), Content.Load<Texture2D>("art//Cow_Piece"));
            cowManager.AddTexture(typeof(Bull), Content.Load<Texture2D>("art//Cow_Piece_bull"));
            cowManager.AddTexture(typeof(Meat), Content.Load<Texture2D>("art//meat"));

            catapult = new Catapult(Content.Load<Texture2D>("art//catapult"), Content.Load<Texture2D>("art//line"), Content.Load<Texture2D>("art//ammo"), collisionManager); 
            wallManager = new WallManager(wallTexture);
            cowManager.SetWallManager(wallManager);
            Texture2D villagerTexture = Content.Load<Texture2D>("art//villager");
            villagerManager = new VillagerManager(villagerTexture, cowManager, collisionManager, meatStore);

            meatStore.addTexture(typeof(NewTowerStoreItem), wallTexture);
            meatStore.addTexture(typeof(NewVillagerStoreItem), villagerTexture);
            NewTowerStoreItem towerStoreItem = new NewTowerStoreItem(wallManager);
            NewVillagerStoreItem villagerItem = new NewVillagerStoreItem(villagerManager);
            //NewTowerStoreItem towerItem2 = new NewTowerStoreItem(wallManager);
            meatStore.addStoreItem(towerStoreItem);
            meatStore.addStoreItem(villagerItem);
            // TODO: use this.Content to load your game content here
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
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            // TODO: Add your update logic here

            cowManager.Update(gameTime);
            catapult.Update(gameTime);
            collisionManager.checkCollision();
            villagerManager.Update(gameTime);

            if (currentKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                meatStore.toggleStore();
            }
            meatStore.Update(currentMouseState, oldMouseState);
            oldKeyboardState = currentKeyboardState;
            oldMouseState = currentMouseState;

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
            cowManager.Draw(spriteBatch);
            catapult.Draw(gameTime, spriteBatch);
            wallManager.Draw(spriteBatch);
            villagerManager.Draw(spriteBatch);
            meatStore.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
