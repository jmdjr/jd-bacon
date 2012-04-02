using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.LinearMath;

namespace JD_Bacon_The_Game
{
    public class EmptyScene : JitterScene
    {
        public EmptyScene(JDBaconTheGame demo)
            : base(demo)
        {

        }

        public override void Build()
        {
            AddGround();
        }
    }

}
