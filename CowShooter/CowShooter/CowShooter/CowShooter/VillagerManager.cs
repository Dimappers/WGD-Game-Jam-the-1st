using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CowShooter
{
    class VillagerManager
    {
        Texture2D villagerTexture;

        bool areVillagersRetreating;

        List<Villager> villagers;
        CowManager cowManager;
        CollisionManager collisionManager;

        KeyboardState oldState, newState;
        int villagersOut = 10; //work around should at least be length of arry

        public VillagerManager(Texture2D villagerTexture, CowManager cowManager, CollisionManager collisionManager)
        {
            this.villagerTexture = villagerTexture;
            villagers = new List<Villager>();

            for (int i = 0; i < 10; ++i)
            {
                Villager newVillager = new Villager(villagerTexture, cowManager, this, new Vector2(650 + i * 32, 400));
                villagers.Add(newVillager);
                collisionManager.addOther(newVillager);
            }
            this.cowManager = cowManager;
            this.collisionManager = collisionManager;
            oldState = Keyboard.GetState();
            newState = oldState;

            villagersOut = 10;
        }

        public void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (villagersOut < villagers.Count)
                {
                    villagers[villagersOut].setSeekingFood(true);
                    villagersOut++;
                }
            }
            else if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                foreach (Villager villager in villagers)
                {
                    villager.setSeekingFood(false);
                }
            }

            List<Villager> deadVillagers = new List<Villager>();
            foreach (Villager villager in villagers)
            {
                villager.Update(gameTime);
                if (villager.getIsDead())
                {
                    deadVillagers.Add(villager);
                }
            }

            foreach(Villager deadVillager in deadVillagers)
            {
                villagers.Remove(deadVillager);
                collisionManager.removeOther(deadVillager);
                villagersOut--;
            }
            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Villager villager in villagers)
            {
                villager.Draw(spriteBatch);
            }
        }

        public void notifyReturn()
        {
            --villagersOut;
        }
    }
}
