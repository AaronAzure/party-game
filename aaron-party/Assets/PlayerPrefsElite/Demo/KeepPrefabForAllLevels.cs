using UnityEngine;
using System.Collections;

/*
		1. Drag & drop SecureKeysManager prefab from Editor/PlayerPrefsElite/Prefab to Hierarchy tab
		2. Generate secure keys
		3. Create empty prefab in Project tab
		4. Drag & drop SecureKeysManager prefab from Hierarchy tab to new empty prefab
		5. Setup SecureKeysManager variable inside your first loaded scene
		6. Copy code from line 20 to line 24 inside Awake in your first loaded scene
*/

public class KeepPrefabForAllLevels : MonoBehaviour {

	public Transform SecureKeysManager;		// step 5

	void Awake(){

		// Add this code to first scene. Step 6
		if (GameObject.Find("SecureKeysManager")==null){
			Transform tmp = Instantiate (SecureKeysManager, Vector3.zero, Quaternion.identity) as Transform;
			tmp.name = "SecureKeysManager";
			DontDestroyOnLoad (tmp.gameObject);
		}
			
	}

}
