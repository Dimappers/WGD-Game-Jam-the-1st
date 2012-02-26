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
        const int maxVillagers = 10;

        Texture2D villagerTexture;

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

            this.collisionManager = collisionManager;
            this.cowManager = cowManager;

            for (int i = 0; i < maxVillagers; ++i)
            {
                CreateVillager();                
            }
            
            
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
                    Villager outVillager = null;
                    do
                    {
                        outVillager = villagers.Dequeue();
                    } while (outVillager.getIsDead());
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
                if (!villager.getIsDead())
                {
                    villager.Draw(spriteBatch);
                }
            }
            foreach (Villager villager in outVillagers)
            {
                villager.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            villagers = new Queue<Villager>();
            outVillagers = new List<Villager>();
            justReturnedVillagers = new List<Villager>();

            for (int i = 0; i < maxVillagers; ++i)
            {
                CreateVillager();
            }

            oldState = Keyboard.GetState();
            newState = oldState;
        }

        public bool PlayerHasLost()
        {
            int aliveVillagers = outVillagers.Count ;
            foreach (Villager inVillager in villagers)
            {
                if(!inVillager.getIsDead())   
                {
                    ++aliveVillagers;
                }
            }
            return aliveVillagers <= 0;
        }

        public void notifyReturn(Villager villager)
        {
            justReturnedVillagers.Add(villager);
        }

        public void DropOffMeat(Meat meat)
        {
            meatStore.addMeat(meat.value);
        }

        public bool CreateVillager()
        {
            if (villagers.Count + outVillagers.Count < maxVillagers)
            {
                int villagerCount = villagers.Count + outVillagers.Count;
                Villager newVillager = new Villager(villagerTexture, cowManager, this, new Vector2(650 + villagerCount * 8, 400 + (16 * (villagerCount % 2))));
                villagers.Enqueue(newVillager);
                collisionManager.addOther(newVillager);
                return true;
            }
            return false;
        }
    }
}
;
