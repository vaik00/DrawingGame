using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

   public void OnMouseDown()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
