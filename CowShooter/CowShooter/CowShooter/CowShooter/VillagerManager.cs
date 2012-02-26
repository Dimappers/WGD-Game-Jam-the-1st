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

        Queue<Villager> villagers;
        List<Villager> outVillagers;
        List<Villager> justReturnedVillagers;
        CowManager cowManager;
        CollisionManager collisionManager;

        KeyboardState oldState, newState;
        MeatStore meatStore;

        public VillagerManager(Texture2D villagerTexture, CowManager cowManager, CollisionManager collisionManager, MeatStore meatStore)
        {
            this.villagerTexture = villagerTexture;
            villagers = new Queue<Villager>();
            outVillagers = new List<Villager>();
            justReturnedVillagers = new List<Villager>();
            for (int i = 0; i < 10; ++i)
            {
                Villager newVillager = new Villager(villagerTexture, cowManager, this, new Vector2(650 + i * 8, 400 + (16 *(i%2))));
                villagers.Enqueue(newVillager);
                collisionManager.addOther(newVillager);
            }
            this.cowManager = cowManager;
            this.collisionManager = collisionManager;
            oldState = Keyboard.GetState();
            newState = oldState;
            this.meatStore = meatStore;
        }

        public void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (villagers.Count != 0)
                {
                    Villager outVillager = villagers.Dequeue();
                    outVillager.setSeekingFood(true);
                    outVillagers.Add(outVillager);
                }
            }
            else if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                foreach (Villager villager in outVillagers)
                {
                    villager.setSeekingFood(false);
                }
            }

            List<Villager> deadVillagers = new List<Villager>();
            foreach(Villager villager in outVillagers)
            {
                villager.Update(gameTime);
                if (villager.getIsDead())
                {
                    deadVillagers.Add(villager);
                }
            }

            foreach(Villager deadVillager in deadVillagers)
            {
                outVillagers.Remove(deadVillager);
                collisionManager.removeOther(deadVillager);
            }

            foreach (Villager villager in justReturnedVillagers)
            {
                outVillagers.Remove(villager);
                villagers.Enqueue(villager);
            }
            justReturnedVillagers.Clear();
            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Villager villager in villagers)
            {
                villager.Draw(spriteBatch);
            }
            foreach (Villager villager in outVillagers)
            {
                villager.Draw(spriteBatch);
            }
        }

        public void notifyReturn(Villager villager)
        {
            justReturnedVillagers.Add(villager);
        }

        public void DropOffMeat(Meat meat)
        {
            meatStore.addMeat(meat.value);
        }
    }
}
