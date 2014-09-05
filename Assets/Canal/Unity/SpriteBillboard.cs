using UnityEngine;
using System.Collections;

namespace Canal.Unity
{
    public class SpriteBillboard : Behavior {
        public Transform facingRoot;
        public Transform rotationTarget;

        void LateUpdate()
        {
            Camera camera = Camera.main;
            Transform lookTarget = camera.transform;

            if (camera.orthographic)
            {
                Vector3 direction = facingRoot.InverseTransformPoint(lookTarget.position) - facingRoot.localPosition;
                Vector3 z = lookTarget.InverseTransformDirection(facingRoot.TransformDirection(direction));
                z.x = 0;
                z.y = 0;
                if(direction.z > 0)
                {
                    

                    rotationTarget.rotation = Quaternion.FromToRotation(Vector3.forward, lookTarget.TransformDirection(z));
                    Vector3 angles = rotationTarget.eulerAngles;
                    angles.z = 0;
                    rotationTarget.eulerAngles = angles;
                }
                else if (direction.z < 0)
                {
                    rotationTarget.rotation = Quaternion.FromToRotation(Vector3.forward, -lookTarget.TransformDirection(z));
                    Vector3 angles = rotationTarget.eulerAngles;
                    angles.z = 0;
                    rotationTarget.eulerAngles = angles;
                }

            }
            else
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
}
