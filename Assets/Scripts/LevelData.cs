using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/Level Data", order = 1)]
public class LevelData : ScriptableObject
{

    [SerializeField]
    public LevelDefinition[] levels;


}