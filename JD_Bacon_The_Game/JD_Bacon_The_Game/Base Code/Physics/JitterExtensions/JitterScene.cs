using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using Jitter.Collision;
using Jitter;

namespace JD_Bacon_The_Game
{

    public enum BodyTag { DrawMe, DontDrawMe }

    public abstract class JitterScene
    { 
        public JDBaconTheGame Demo { get; private set; }

        public JitterScene(JDBaconTheGame demo)
        {
            this.Demo = demo;
        }

        public abstract void Build();

        private QuadDrawer quadDrawer = null;
        protected RigidBody ground = null;

        public void AddGround()
        {
            ground = new RigidBody(new BoxShape(new JVector(1000, 20, 1000)));
            ground.Position = new JVector(0, -10, 0);
            ground.Tag = BodyTag.DontDrawMe;
            ground.IsStatic = true;
            Demo.World.AddBody(ground);
            ground.Material.KineticFriction = 0.0f;

            quadDrawer = new QuadDrawer(Demo,100);
            Demo.Components.Add(quadDrawer);
        }

        public void RemoveGround()
        {
            Demo.World.RemoveBody(ground);
            Demo.Components.Remove(quadDrawer);
            quadDrawer.Dispose();
        }
        
        public virtual void Draw() { }

    }
}
