using UnityEngine;

public class Managers : MonoBehaviour
{
    
    static Managers instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      
        }
        else
        {
            Debug.Log("There is more than one Manager system, Destroying other managers!");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
