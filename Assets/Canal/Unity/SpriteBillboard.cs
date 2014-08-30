using UnityEngine;
using System.Collections;

namespace Canal.Unity
{
    public class SpriteBillboard : Behavior {
        public Transform facingRoot;
        public Transform rotationTarget;
        public Transform lookTarget;

        void Update()
        {
            Vector3 direction = facingRoot.InverseTransformPoint(lookTarget.position) - facingRoot.localPosition;
            if(direction.z > 0)
            {
                rotationTarget.rotation = Quaternion.FromToRotation(Vector3.forward, facingRoot.TransformDirection(direction));
                Vector3 angles = rotationTarget.eulerAngles;
                angles.z = 0;
                rotationTarget.eulerAngles = angles;
            }
            else if (direction.z < 0)
            {
                rotationTarget.rotation = Quaternion.FromToRotation(Vector3.forward, -facingRoot.TransformDirection(direction));
                Vector3 angles = rotationTarget.eulerAngles;
                angles.z = 0;
                rotationTarget.eulerAngles = angles;
            }
        }
    }
}
