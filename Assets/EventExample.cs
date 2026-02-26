using UnityEngine;
using UnityEditor;

public class EventExample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vaniakit.Events.EventManager.saveEvent("GoogleEvent");    
    }
    
}
[CustomEditor(typeof(EventExample))]
public class FastTravelPointEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
            
         if (GUILayout.Button("Save Game")) //Saves this point in a json file when this button is pressed
         {
             Vaniakit.SaveSystem.SaveSystem.SaveAllData();
         }
    }
}