using Character_Builder;
using OGL.Base;

namespace CB5e
{
	public readonly record struct JournalStats(
		int Level,
		List<JournalPossession> Possessions,
		List<JournalBoon> Boons,
		int XP,
		int AP,
		int Renown,
		int Downtime,
		int MagicItems,
		int T1TP,
		int T2TP,
		int T3TP,
		int T4TP,
		Price Money,
		int CommonCount,
		int UncommonCount,
		int ConsumableCount);
}
