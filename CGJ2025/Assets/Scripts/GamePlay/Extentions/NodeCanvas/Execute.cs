using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using NUnit.Framework;

#if UNITY_EDITOR
[Category("Dialogue")]
[Description(
    "You can use a variable inline with the text by using brackets likeso:\n[myVarName] or [Global/myVarName].\nThe bracket will be replaced with the variable value ToString")]
[ParadoxNotion.Design.Icon("Dialogue")]
#endif
public class Execute : ActionTask<IDialogueActor>
{
    public string dialogueKey;
    public ExcuteType Type;

    protected override void OnExecute()
    {
        /*if (Type == ExcuteType.AllowHoldF)
        {
            GameManager.Instance.AllowHoldF = true;
        }

        if (Type == ExcuteType.GotoNext)
        {
            GameManager.Instance.GoToNode2();
        }

        if (Type == ExcuteType.StartDialogue)
        {
            EndAction();
            DialougueManager.Instance.StartDialogue(dialogueKey);
            return;
        }*/

        EndAction();
    }
}

public enum ExcuteType
{
    AllowHoldF,
    GotoNext,
    StartDialogue,
}