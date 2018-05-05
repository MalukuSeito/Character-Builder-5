using CB_5e.Helpers;
using Character_Builder;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Character.Play
{
    public class PlayerFormsCompanionsViewModel : SubModel
    {
        public PlayerFormsCompanionsViewModel(PlayerModel parent) : base(parent, "Forms / Companions")
        {
            monsters = new List<MonsterInfo>();
            Dictionary<Monster, MonsterInfo> monstr = new Dictionary<Monster, MonsterInfo>(new MonsterInfo());
            var asa = Context.Player.GetFinalAbilityScores();
            foreach (FormsCompanionInfo fc in Context.Player.GetFormsCompanionChoices())
            {
                foreach (Monster m in fc.AppliedChoices(Context, asa))
                {
                    if (monstr.ContainsKey(m))
                    {
                        monstr[m].Sources.Add(fc.DisplayName);
                    }
                    else
                    {
                        monstr.Add(m, new MonsterInfo()
                        {
                            Monster = m,
                            Sources = new List<string>() { fc.DisplayName }
                        });
                    }
                }
            }
            monsters.AddRange(monstr.Values.OrderBy(s => s.Monster.Name));
            UpdateMonsters();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            monsters = new List<MonsterInfo>();
            Dictionary<Monster, MonsterInfo> monstr = new Dictionary<Monster, MonsterInfo>(new MonsterInfo());
            var asa = Context.Player.GetFinalAbilityScores();
            foreach (FormsCompanionInfo fc in Context.Player.GetFormsCompanionChoices())
            {
                foreach (Monster m in fc.AppliedChoices(Context, asa))
                {
                    if (monstr.ContainsKey(m))
                    {
                        monstr[m].Sources.Add(fc.DisplayName);
                    }
                    else
                    {
                        monstr.Add(m, new MonsterInfo()
                        {
                            Monster = m,
                            Sources = new List<string>() { fc.DisplayName }
                        });
                    }
                }
            }
            monsters.AddRange(monstr.Values.OrderBy(s => s.Monster.Name));
            UpdateMonsters();
        }

        private string monsterSearch;
        public string MonstersSearch
        {
            get => monsterSearch;
            set
            {
                SetProperty(ref monsterSearch, value);
                UpdateMonsters();
            }
        }
        private List<MonsterInfo> monsters;
        public ObservableRangeCollection<MonsterInfo> Monsters { get; set; } = new ObservableRangeCollection<MonsterInfo>();

        public void UpdateMonsters() => Monsters.ReplaceRange(from f in monsters where monsterSearch == null || monsterSearch == ""
            || f.Monster.Matches(monsterSearch, false) orderby f.Name select f);

    }
}
