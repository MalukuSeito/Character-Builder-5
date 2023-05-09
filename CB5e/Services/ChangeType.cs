namespace CB5e.Services
{
	[Flags]
	public enum ChangeType
	{
		None = 0,
		Full = 1,
		Features = 2,
		PlayerInfo= 4,
		Portrait = 8,
		Inventory = 16,
		Journal = 32,
		Spells = 64,
		Resources = 128,
		Spellslots = 256,
		PlayData = 512,
		DataFiles = 1024,
		FormsCompanions = 2048
	}
}
