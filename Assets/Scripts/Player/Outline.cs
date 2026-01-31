using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Outline : MonoBehaviour
{
    [SerializeField] Material colorBlockShader;
    SpriteRenderer spriteRenderer;
    GameObject outline;
    SpriteRenderer outlineRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        outline = new GameObject("spriteOutline");
        outlineRenderer = outline.AddComponent<SpriteRenderer>();
        outline.transform.position = transform.position;
        outline.transform.parent = transform;
        outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        outlineRenderer.material = colorBlockShader;
        outlineRenderer.sprite = spriteRenderer.sprite;
        outlineRenderer.enabled = false;
    }

    public void Activate(Color color, float scale = 1.2f)
    {
        outlineRenderer.transform.localScale = transform.localScale * scale;
        outlineRenderer.color = color;
        outlineRenderer.enabled = true;
    }
    public void Deactivate()
    {
        outlineRenderer.enabled = false;
    }
}
