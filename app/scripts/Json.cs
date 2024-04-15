// Json.cs
//
// Defines helpful functions for reading from JSON files.

using Godot;
class Json : Node {
	// Given a filePath, read the JSON from the file and return the 
	// JSON as a dictionary.
	public static Godot.Collections.Dictionary ReadJSON(string filePath) {
		File file = new File();
		file.Open(filePath, File.ModeFlags.Read);
		string rawJSON = file.GetAsText();
		file.Close();

		JSONParseResult jsonResult = JSON.Parse(rawJSON);
		var dict = jsonResult.Result as Godot.Collections.Dictionary;
		
		return dict;
	}
}
