using UnityEngine;

using System.Collections.Generic;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class PlayerInput : Behavior {
        public MovementPhysics Physics;

        public void Update()
        {
            float move = Input.GetAxisRaw("Horizontal");
            Physics.WalkDirection = (move == 0 ? 0 : Mathf.Sign(move));
        }
    }
}
