using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TetrisGame.Systems
{
    internal class GameTimer : IUpdatable
    {
        public static List<GameTimer> Timers { get; set; } = new List<GameTimer>();
        public int Interval { get; set; }

        public int Current { get; set; } = 0;

        private bool Enabled { get; set; } = false;

        private readonly bool loop = false;

        public event EventHandler<EventArgs> Tick;

        public GameTimer(int interval, bool loop)
        {
            Interval = interval;
            this.loop = loop;
            Timers.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                if (Current == Interval)
                {
                    Tick?.Invoke(this, new EventArgs());
                    if (loop)
                    {
                        Current = 0;
                    }
                    else
                    {
                        Enabled = false;
                    }
                }
                Current++;
            }
        }

        public void Start()
        {
            Enabled = true;
        }
        public void Stop()
        {
            Enabled = false;
        }

        public void Reset()
        {
            Current = 0;
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Unregister()
        {
            Timers.Remove(this);
        }
    }
}
