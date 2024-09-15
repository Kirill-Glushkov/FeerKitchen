using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform _prefab;
    public Sprite _sprite;
    public string _obstacleName;
}
