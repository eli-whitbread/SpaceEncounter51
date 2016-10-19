// Generate a prefab from the selection

@MenuItem ("Project Tools / Make Prefab")
static function LogSelectedTransformName () 
{
    CreatePrefab();
}

// Validate the menu item defined by the function above.
// The menu item will be disabled if this function returns false.
@MenuItem ("Project Tools / Make Prefab", true)
static function ValidateLogSelectedTransformName () 
{
    // Return false if no transform is selected.
    return Selection.activeTransform != null;
}

static function CreatePrefab()
{
	var selectedObjects : GameObject[] = Selection.gameObjects;
	for(var go : GameObject in selectedObjects)
	{
		var name : String = go.name;	// Store the name of our selection
		var localPath : String = "Assets/Prefabs/" + name + ".prefab";	// Create the path for the prefab	
		
		// Determine whether prefab already exists
		if(AssetDatabase.LoadAssetAtPath(localPath, GameObject))
		{
			//Check for user choice
			if(EditorUtility.DisplayDialog("Caution with Prefab: " + go.name, "Prefab " + go.name + " already exists. Do you want to overwrite?", "Yes", "No"))
			{
				CreateNew(go, localPath);		// Create new prefab
			}
		}
		else
		{
			// Create new Prefab
			CreateNew(go, localPath);	
		}
	}	
}

// Function Creates a new Prefab
static function CreateNew(selectedObject : GameObject, localPath : String)
{
	var prefab : Object = PrefabUtility.CreateEmptyPrefab(localPath);	// Store prefab
	PrefabUtility.ReplacePrefab(selectedObject, prefab);				// Set Prefab to prefab
	
	AssetDatabase.Refresh();		// Refresh database
	
	DestroyImmediate(selectedObject);		// Remove the selected object
	var clone : GameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;		// Replace the object with a prefab
}