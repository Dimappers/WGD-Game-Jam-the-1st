﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Bull : Cow
    {
        const int health = 3;
        public Bull(CowManager manager, WallManager wallManager, CowStack cowStack, Texture2D h1Texture, Texture2D h2Texture) : base(manager, wallManager, cowStack, h1Texture, h2Texture, health){ }
    }
}
