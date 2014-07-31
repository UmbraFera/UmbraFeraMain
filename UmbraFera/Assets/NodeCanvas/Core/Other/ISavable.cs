namespace NodeCanvas{

	interface ISavable{

		string Save();
		bool Load();
		string saveKey{get;}
	}
}