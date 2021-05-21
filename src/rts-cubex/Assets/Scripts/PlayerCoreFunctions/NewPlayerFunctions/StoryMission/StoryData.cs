using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="StoryDialogueData",menuName="StoryData",order=0)]
public class StoryData : ScriptableObject
{
    //List that contains all the Lost Alpha mission dialogue
    [SerializeField] List<string> dialogueLines;

    public List<string> DialogueLines {get { return dialogueLines; }}
}
