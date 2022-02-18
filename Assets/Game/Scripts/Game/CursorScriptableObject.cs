using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Cursor", menuName = "ScriptableObjects/Cursor", order = 1)]
public class CursorScriptableObject : ScriptableObject
{
    [SerializeField]
    public List<Vector2Int> cursorParts;
}
