using System;
using UnityEngine;

[Serializable]
public class Answer
{
    [SerializeField] private string text;
    [SerializeField] private int link;
}

[Serializable]
public class Dialogue
{
    [SerializeField] private int character;
    [SerializeField] private string text;
    [SerializeField] private Sprite image;
    [SerializeField] private Answer[] answers;
}
