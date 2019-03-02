using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public abstract class PDFBase
    {
        private const string APTotal = "{0}";
        private const string APLevel = "{2}/{1}";
        private const string AP = "{1}";
        private const string APMax = "{1} of {3}";
        private const string APMaxLevel = "{2}/{1} of {3}";
        private const string Full = "{2}/{1} of {3}, Total {0}";
        private const string Dot = "{2}.{1}";
        public const int None = 0;
        public const int ExceptAtWillAndCharges = 1;
        public const int All = 2;
        public static readonly string[] APFormats = new string[] { APTotal, APLevel, AP, APMax, APMaxLevel, Full, Dot };
        public bool PreserveEdit { get; set; }
        public bool IncludeResources { get; set; }
        public bool IncludeLog { get; set; }
        public bool IncludeSpellbook { get; set; }
        public bool IncludeActions { get; set; }
        public bool IncludeMonsters { get; set; }
        public bool IgnoreMagicItems { get; set; }
        public bool AutoExcludeActions { get; set; } = true;
        public bool ForceAttunedAndOnUseItemsOnSheet { get; set; } = false;
        public bool ForceAttunedItemsInSpellbook { get; set; } = false;
        public bool Duplex { get; set; } = true;
        public bool DuplexWhite { get; set; } = false;
        public bool SwapScoreAndMod { get; set; } = true;
        public bool MundaneEquipmentInSpellbook { get; set; } = true;
        public bool OnlyFeatureTitles { get; set; } = false;
        public bool EquipmentKeywords { get; set; } = false;
        public bool EquipmentStats { get; set; } = true;
        public int BonusSpellsAreResources { get; set; } = ExceptAtWillAndCharges;
        public String APFormat { get; set; } = Dot;
        public abstract Task<IPDFEditor> CreateEditor(string file);
        public abstract IPDFSheet CreateSheet();
    }
}
