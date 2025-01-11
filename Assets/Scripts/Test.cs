using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Data.Play;
using UnityEditor;
using UnityEngine;
using Util;

public class Test : MonoBehaviour
{
    public TextAsset textAsset;

    public MonsterData monsterData;
    
    public void Read()
    {
        monsterData = CsvReader.ReadData<MonsterData>(textAsset.text);
    }
}
