using UnityEngine;

public enum EnumExample
{
    None = 0,
    Player = 1,
    NPC = 2,
}
public class MyComponent : MonoBehaviour 
{
    [Header("Attributes")]

    [SerializeField]
    [Tooltip("Max health")]
    private float _velocidad = 5f;

    [SerializeField]
    [Range(0f, 100f)]
    private int _vida = 100;

    [SerializeField]
    private int _range;


    [Header("Dialogue")]
    [SerializeField]
    [Tooltip("Speaks Dialogue")]
    [TextArea()]
    private string _battleCry;

    [SerializeField]
    private EnumExample _enum = EnumExample.Player;

}
