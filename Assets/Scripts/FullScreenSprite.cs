using UnityEngine;

public class FullScreenSprite : MonoBehaviour
{
    public new SpriteRenderer renderer;
    float _originSize;
    [SerializeField]
    float _spriteScale = 1;
    [SerializeField]
    bool _scaleOnAwake;
    [SerializeField]
    bool _peserveAspect = true;

    private void Awake()
    {
        _originSize = Camera.main.orthographicSize;

        if (_scaleOnAwake)
            ScaleSprite(_spriteScale);
    }

    public void ScaleSprite(float spriteScale)
    {
        _spriteScale = spriteScale;
        ScaleSprite();
    }

    public void ScaleSprite()
    {
        float cameraHeight = _originSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = renderer.sprite.bounds.size;//texture.texelSize; //
        //Vector2 textureSize = 
        Vector2 scale = transform.localScale;

        if (_peserveAspect)
        {
            if (Camera.main.orthographicSize <= 5)
            {
                // Landscape (or equal)
                scale = Vector3.one * cameraSize.x / spriteSize.x;
            }
            else
            {
                // Portrait
                scale = Vector3.one * cameraSize.y / spriteSize.y;
            }
        }
        else
        {
            scale.x = cameraSize.x / spriteSize.x;
            scale.y = cameraSize.y / spriteSize.y;
        }

        transform.position = Vector2.zero; // Optional
        transform.localScale = scale * _spriteScale;
    }
}