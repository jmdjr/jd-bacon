using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.LinearMath;

namespace JD_Bacon_The_Game
{
    public class EmptyScene : JitterScene
    {
        Shotgun gun;
        public EmptyScene(JDBaconTheGame demo)
            : base(demo)
        {

        }

        public override void Build()
        {
            AddGround();
            gun = new Shotgun(this.Demo);
            this.Demo.Components.Add(gun);

            gun.Build();
            gun.ShotgunBody.Position = new JVector(0, 15f, 0f);
        }
    }

}
