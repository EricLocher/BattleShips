using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private static Mouse _instance;
    public static Mouse Instance { get { return _instance; } }

    [SerializeField] CursorScriptableObject _defaultCursor;
    public static CursorScriptableObject _currentCursor;
    public static Vector2 mousePosition;

    public static Ship selectedShip = null;
    public static Board hoveringOver = null;

    public delegate void OnMouseClick();
    public static event OnMouseClick OnMouseClickEvent;

    //Temp
    [SerializeField] List<Ship> shipList;


    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _currentCursor = _defaultCursor;
    }

    void Update()
    {
        UpdateMousePosition();
        if (hoveringOver != null) { mousePosition = new Vector2(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y)); }


        if (Input.GetMouseButtonDown(0)) {
            OnMouseClickEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedShip = Instantiate(shipList[0]);
        }

        if (selectedShip != null) {
            selectedShip.transform.position = mousePosition;
        }

    }

    void UpdateMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }



}
