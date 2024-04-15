using Godot;
using System;


using GDictionary = Godot.Collections.Dictionary;

public class JournalManager : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private string[] m_journalEntries;
	private int m_size = 0;
	private int m_lastPulled = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GDictionary allEntries = Json.ReadJSON("res://data/journal_entries.json");
		int entryCount = (int)allEntries.Count;
		GD.Print(entryCount);
		m_journalEntries = new string[entryCount];

		for (int i = 0; i < entryCount; ++i) {
			GDictionary journalEntryDict = allEntries[i.ToString()] as GDictionary;
			m_journalEntries[i] = (string)journalEntryDict["entry"];
		}

		m_size = entryCount;
	}
	
	public string GetNextEntry() {
		if (m_lastPulled >= m_size) { return ""; }
		
		string returnString = m_journalEntries[m_lastPulled++];
		return returnString;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
