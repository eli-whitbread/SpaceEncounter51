// generate folders in our project

import System.IO;

@MenuItem("Project Tools / Make Folders") // In quotation, provide space, and then the shortcut notation i.e. '#&_g' (Shift + Alt + g)
// menuItem read the first static function
static function MakeFolder()
{
		GenerateFolders();
}

static function GenerateFolders()
{
	var projectPath : String = Application.dataPath + "/";	// Store the Path for the folders
	
	// Creating the folders
	Directory.CreateDirectory(projectPath + "Audio");
	Directory.CreateDirectory(projectPath + "Materials");
	Directory.CreateDirectory(projectPath + "Meshes");
	Directory.CreateDirectory(projectPath + "Fonts");
	Directory.CreateDirectory(projectPath + "Textures");
	Directory.CreateDirectory(projectPath + "Resources");
	Directory.CreateDirectory(projectPath + "Scripts");
	Directory.CreateDirectory(projectPath + "Shaders");
	Directory.CreateDirectory(projectPath + "Packages");
	Directory.CreateDirectory(projectPath + "Prefabs");
	Directory.CreateDirectory(projectPath + "Scenes");
	
	AssetDatabase.Refresh();
}