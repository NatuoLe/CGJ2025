using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using ThGold.Common;
using UnityEngine;

public class DialougueManager : MonoSingleton<DialougueManager>
{
    public DialogueTreeController DialogueTreeController;

    private Dictionary<string, DialogueTree> treeDic;

    async UniTask Start()
    {
        treeDic = await LoadAllDialogueTrees();
    }

    static async UniTask<Dictionary<string, DialogueTree>> LoadAllDialogueTrees()
    {
        Dictionary<string, DialogueTree> allConfigs = new Dictionary<string, DialogueTree>();
        /*string[] filePaths =
            Directory.GetFiles(Application.dataPath + "/Resources/Dialogues/Trees", "*.asset");
        foreach (string filePath in filePaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Debug.Log("Path" + filePath + "Name" + fileName);
            ResourceRequest handle =
                Resources.LoadAsync<DialogueTree>("Dialogues/Trees/" +
                                                  fileName);
            await handle.ToUniTask();
            DialogueTree tree = (DialogueTree) handle.asset;
            if (tree != null)
            {
                allConfigs.Add(fileName, tree);
            }
        }*/
// 改为使用 Resources.LoadAll 直接加载
        DialogueTree[] trees = Resources.LoadAll<DialogueTree>("Dialogues/Trees");
        foreach (var tree in trees)
        {
            allConfigs.Add(tree.name, tree);
        }

        return allConfigs;
    }

    public void StartDialogue(string key)
    {
        StartDialogue(key, DialogueTreeController, null);
    }

    public void StartDialogue(string key, IDialogueActor instigator, Action<bool> callback)
    {
        DialogueTreeController.StartDialogue(GetDialogueTreeByKey(key), instigator, callback);
    }

    public DialogueTree GetDialogueTreeByKey(string key)
    {
        if (treeDic.TryGetValue(key, out DialogueTree tree))
        {
            return tree;
        }

        Debug.Log("[dialogue Key]" + key + "不存在");
        return null;
    }

    #region GM

    [Button("StartDialogue")]
    public void StartDialogue()
    {
        DialogueTreeController.StartDialogue();
    }

    #endregion
}