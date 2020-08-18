using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TEXTURE MUST BE SET TO TILED MODE FOR THIS TO WORK DUE TO INFINITE SCORLLING.
// MUST ALSO SET THE SPRITE WIDTH TO 1 WIDTH AHEAD AND 1 WIDTH BEHIND (This to give the parallax a buffer.)
public class ParallaxBackground : MonoBehaviour
{

    public Vector2 parallaxEffectMultiplier;
    public bool infiniteHorizontal = true;
    public bool infiniteVertical = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private float localScaleX;
    private float localScaleY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;    
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        localScaleX = transform.localScale.x;
        localScaleY = transform.localScale.y;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * localScaleX;
        textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * localScaleY;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y,
            0
        );
        lastCameraPosition = cameraTransform.position;
        if (infiniteHorizontal && Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
        if (infiniteVertical && Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetPositionY = (cameraTransform.position.x - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y + offsetPositionY);
        }
    }
}
