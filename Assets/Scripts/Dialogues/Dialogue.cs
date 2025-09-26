using System;
using UnityEngine;

[Serializable]
public class Answer
{
    [SerializeField] private string text;
    [SerializeField] private int link;
    [SerializeField] private int condition;
    [SerializeField] private int effect;

    public string GetText() { return text; }
    public int GetLink() { return link; }
    public int GetCondition() { return condition; }
    public int GetEffect() { return effect; }
}

[Serializable]
public class Dialogue
{
    [SerializeField] private string text;
    [SerializeField] private string character;
    [SerializeField] private Sprite image;
    [SerializeField] private Answer[] answers;

    public string GetCharacter() { return character; }
    public string GetText() { return text; }
    public Sprite GetImage() { return image; }
    public Answer[] GetAnswers() { return answers; }
}
