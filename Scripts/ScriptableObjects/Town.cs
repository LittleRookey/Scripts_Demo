using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Town", menuName = "Litkey/Town")]
public class Town : ScriptableObject
{
    [SerializeField] private SpriteRenderer mapMarker;
    public Vector2 townPosition;
    // ���������� ��ǰ, �ϴ� �������� �� �Ҽ� ������ ���ϱ�
    public string townName;
}
