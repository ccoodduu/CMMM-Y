using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyTexture : MonoBehaviour
{
    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (TextureLoader.textures.ContainsKey(spriteRenderer.sprite.name))
                spriteRenderer.sprite = TextureLoader.textures[spriteRenderer.sprite.name];
        }

        var image = GetComponent<Image>();
        if (image != null)
        {
            if (TextureLoader.textures.ContainsKey(image.sprite.name))
                image.sprite = TextureLoader.textures[image.sprite.name];
        }
    }
}
