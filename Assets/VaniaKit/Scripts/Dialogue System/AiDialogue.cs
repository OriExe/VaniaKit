using UnityEngine;

namespace VaniaKit.DialogueSystem
{
    public class AiDialogue : MonoBehaviour
    {
        [SerializeField] protected DialogueDataList[] allDialogue;
        [SerializeField] protected int dialogueListIndex;
        private int currentDialogueIndex = 0;
        protected virtual void onDialogueChangedTo(DialogueData dialogueData)
        {
            Debug.Log("Character " + dialogueData.speakingCharacterIndex + " has said " + dialogueData.dialogue);
            Debug.Log($"Conversation {dialogueListIndex} is being had");
        }
        protected virtual void onDialogueStart()
        {
            Debug.Log("Dialogue system has started with" + gameObject.name);
        }
        protected virtual void onDialogueEnd()
        {
            Debug.Log("Dialogue system has ended with" + gameObject.name);
        }

        protected void triggerDialogue()
        {
            if (currentDialogueIndex <= 0) //If 0 then the system has just started
            {
                onDialogueStart();
            }
            else if  (currentDialogueIndex >= allDialogue[dialogueListIndex].DialogueList.Length) //if at the end of the list (the final piece of dialogue in the converstation)
            {
                onDialogueEnd();
                currentDialogueIndex = 0;
                return;
            }
            onDialogueChangedTo(allDialogue[dialogueListIndex].DialogueList[currentDialogueIndex]); //Sends the dialogue that should be said and by who
            currentDialogueIndex++;
        }
    }

    /// <summary>
    /// Data that holds the dialogue to be said and the character that is speaking said dialogue
    /// </summary>
    [System.Serializable]
    public class DialogueData
    {
        public string dialogue;
        public int speakingCharacterIndex;
    }

    /// <summary>
    /// Stores a list of the dialogue options available by an npc. Allowing for the same npc to have multiple conversations that only occur on certain conditions
    /// </summary>
    [System.Serializable]
    public class DialogueDataList
    {
        public DialogueData[] DialogueList;
    }
}