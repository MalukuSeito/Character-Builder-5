using Character_Builder;

namespace CB5e
{
	public readonly record struct JournalInfo(
		JournalStats Before,
		JournalEntry Entry,
		JournalStats After,
		List<string> ItemsAdded,
		List<string> ItemTouched,
		List<string> ItemsRemoved);

}
