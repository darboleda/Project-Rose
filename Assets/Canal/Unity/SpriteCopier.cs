using UnityEngine;
using System.Collections;

namespace Canal.Unity
{
    [ExecuteInEditMode]
    public class SpriteCopier : Behavior {
        public SpriteRenderer spriteToCopy;
        public SpriteRenderer targetSprite;

        void LateUpdate()
        {
            targetSprite.sprite = spriteToCopy.sprite;
        }
    }
}
