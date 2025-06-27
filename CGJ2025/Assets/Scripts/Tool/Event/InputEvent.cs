using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example Struct
/// </summary>
public struct AppleData
{
    public string name ;
    public int number;
    public int price ;
    public AppleData(string name, int num, int price)
    {
        this.name = name;
        this.number = num;
        this.price = price;
    }
}
