using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Color _color, _offsetColor;
    [SerializeField] GameObject _hover, _hit;

    public bool isDead = false;
    Bounds cellBounds;

    public GameObject occupyingGameObject = null;

    public void Init(bool isOffset)
    {
        if (isOffset) { _spriteRenderer.color = _offsetColor; }
        else { _spriteRenderer.color = _color; }

        cellBounds = new Bounds(transform.position, new Vector3(1, 1, 0));
    }
    
    void Update()
    {
        if (isDead) {
            _hover.SetActive(false);
            _hit.SetActive(true);
            return;
        }

        _hover.SetActive(false);
        foreach (Vector2Int cursorPart in Mouse._currentCursor.cursorParts) {
            if (cellBounds.Contains(new Vector2(Mouse.mousePosition.x + cursorPart.x, Mouse.mousePosition.y + cursorPart.y))) {
                Hover();
            }
        }
    }

    public void Hover()
    {
        _hover.SetActive(true);
    }

    public void Hit()
    {
        isDead = true;
    }
}
