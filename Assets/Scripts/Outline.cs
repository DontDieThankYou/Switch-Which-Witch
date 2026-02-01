using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Outline : MonoBehaviour
{
    [SerializeField] Material colorBlockShader;
    public Color defaultHighlightColor;
    SpriteRenderer spriteRenderer;
    GameObject outline;
    SpriteRenderer outlineRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        outline = new GameObject("spriteOutline");
        outlineRenderer = outline.AddComponent<SpriteRenderer>();
        outline.AddComponent<Billboard>();
        outline.transform.position = transform.position;
        outline.transform.parent = transform;
        outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        outlineRenderer.material = colorBlockShader;
        outlineRenderer.sprite = spriteRenderer.sprite;
        outlineRenderer.enabled = false;
    }

    void Update()
    {
        if (outlineRenderer.sprite == null)
        {
            outlineRenderer.sprite = spriteRenderer.sprite;
        }
    }

    public void Activate()
    {
        Activate(defaultHighlightColor);
    }
    public void Activate(Color color, float scale = 1.2f)
    {
        outlineRenderer.transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale * 0.9f, transform.localScale.z * scale);
        outlineRenderer.color = color;
        outlineRenderer.enabled = true;
    }
    public void Deactivate()
    {
        outlineRenderer.enabled = false;
    }
}
