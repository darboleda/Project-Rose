using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpriteCopier : MonoBehaviour {
    public SpriteRenderer spriteToCopy;
    public SpriteRenderer targetSprite;

	void FixedUpdate()
    {
        targetSprite.sprite = spriteToCopy.sprite;
    }
}
