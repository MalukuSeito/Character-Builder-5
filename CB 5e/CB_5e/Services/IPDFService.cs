using Character_Builder;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.Services
{
    public interface IPDFService
    {
        bool PreserveEdit { get; set; }
        bool IncludeResources { get; set; }
        bool IncludeLog { get; set; }
        bool IncludeSpellbook { get; set; }
        bool IncludeActions { get; set; }
        bool IncludeMonsters { get; set; }
        bool IgnoreMagicItems { get; set; }
        bool AutoExcludeActions { get; set; }
        bool ForceAttunedAndOnUseItemsOnSheet { get; set; }
        bool ForceAttunedItemsInSpellbook { get; set; }
        bool Duplex { get; set; }
        bool DuplexWhite { get; set; }
        bool SwapScoreAndMod { get; set; }
        String APFormat { get; set; }
        bool MundaneEquipmentInSpellbook { get; set; }
        bool OnlyFeatureTitles { get; set; }
        bool EquipmentKeywords { get; set; }
        bool EquipmentStats { get; set; }
        int BonusSpellsAreResources { get; set; }
        Task ExportPDF(PDF Exporter, BuilderContext context);
    }
}
