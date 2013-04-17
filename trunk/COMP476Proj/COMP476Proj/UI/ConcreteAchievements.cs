﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace COMP476Proj
{
    // Since all of the concrete achievements are small, they can easily be kept together.
    // If this becomes unweildly, we can split it up

    // To create an achievement, create a new public class and override the Update
    // and IsAchieved methods. Make sure to call the base constructor with the name,
    // description, and score value.
    /// <summary>
    /// Achievement earned through playtime
    /// </summary>
    /// 
    public class Achievement_Playtime : Achievement
    {
        private int timePlayed;
        private const int maxTime = 2000;

        public Achievement_Playtime()
            : base("Easiest achievement ever", "Exist for 2 seconds", 1000)
        {
            timePlayed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            int time = gameTime.ElapsedGameTime.Milliseconds;
            timePlayed += time;
        }

        public override bool IsAchieved()
        {
            return timePlayed >= maxTime;
        }
    }

    /// <summary>
    /// Achievement earned by picking up lotion
    /// </summary>
    public class Achievement_Slick : Achievement
    {
        public Achievement_Slick(int i, int j)
            : base("Slick Rick " + i, "Picked up " + j + " Lotion Powerups", j)
        {
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            return DataManager.GetInstance().numberOfGrease >= Value;
        }
    }

    /// <summary>
    /// Achievement earned by picking up lotion
    /// </summary>
    public class Achievement_PowerUp : Achievement
    {
        public Achievement_PowerUp(int i, int j)
            : base("Powered Up " + i, "Picked up " + j + " Total Powerups", j)
        {
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            return DataManager.GetInstance().PowerUpCount >= Value;
        }
    }

    /// <summary>
    /// Achievement earned by picking up steroids
    /// </summary>
    public class Achievement_Mass : Achievement
    {
        public Achievement_Mass(int i, int j)
            : base("Roid Rage " + i, "Picked up " + j + " Steroid Powerups", j)
        {
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            return DataManager.GetInstance().numberOfSteroids >= Value;
        }
    }

    /// <summary>
    /// Achievement earned by picking up energy drinks
    /// </summary>
    public class Achievement_Speed : Achievement
    {
        public Achievement_Speed(int i, int j)
            : base("Speedy Streakzales " + i, "Picked up " + j + " Energy Drink Powerups", j)
        {
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            return DataManager.GetInstance().numberOfRedBull >= Value;
        }
    }

    /// <summary>
    /// Achievement earned by picking up Sneakers
    /// </summary>
    public class Achievement_Turn : Achievement
    {
        public Achievement_Turn(int i, int j)
            : base("Turn on a Dime " + i, "Picked up " + j + " Sneaker Powerups", j)
        {
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            return DataManager.GetInstance().numberOfSneakers >= Value;
        }
    }

    /// <summary>
    /// Achievement earned by pressing space
    /// </summary>
    public class Achievement_PressSpace : Achievement
    {
        public bool spacePressed = false;
        public Achievement_PressSpace() : base("Press Space", "Press the spacebar", 1000) { }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                spacePressed = true;
        }
        public override bool IsAchieved()
        {
            return spacePressed;
        }

    }

    /// <summary>
    /// Achievement earned by entering a room (given the room's trigger ID)
    /// </summary>
    public class Achievement_EnterRoom : Achievement
    {
        private int triggerID;
        public Achievement_EnterRoom(int triggerID, string roomName, int points)
            : base("Enter the " + roomName, "Enter the " + roomName, points)
        {
            this.triggerID = triggerID;
        }

        public override void Update(GameTime gameTime) { }

        public override bool IsAchieved()
        {
            foreach (Trigger trigger in Game1.world.map.triggers)
            {
                if (trigger.ID == triggerID)
                {
                    return trigger.hasTriggered();
                }
            }
            return false;
        }
    }

    public class Achievement_DoomHallway : Achievement
    {
        private bool leftSide = false;
        private bool rightSide = false;
        private int leftID, rightID;
        private bool complete = false;
        public Achievement_DoomHallway(int leftID, int rightID)
            : base("Traverse the hallway of DOOM", "Ran all the way through the top hallway", 2000)
        {
            this.leftID = leftID;
            this.rightID = rightID;
        }

        public override void Update(GameTime gameTime)
        {
            if (!complete)
                foreach (Trigger trigger in Game1.world.map.triggers)
                {
                    if (trigger.hasTriggered())
                    {
                        if (trigger.ID == leftID)
                        {
                            if (!rightSide)
                                leftSide = true;
                            else
                            {
                                rightSide = false;
                                complete = true;
                            }
                        }

                        else if (trigger.ID == rightID)
                        {
                            if (!leftSide)
                                rightSide = true;
                            else
                            {
                                leftSide = false;
                                complete = true;
                            }
                        }
                        else
                        {
                            leftSide = false;
                            rightSide = false;
                        }
                    }
                }
        }

        public override bool IsAchieved()
        {
            return complete;
        }
    }
}
