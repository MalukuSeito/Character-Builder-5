using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class BuilderContext: OGLContext
    {
        public LinkedList<Player> UndoBuffer = new LinkedList<Player>();
        public LinkedList<Player> RedoBuffer = new LinkedList<Player>();
        public int UnsavedChanges = 0;
        public int MaxBuffer = 200;
        public PluginManager _plugins;
        public PluginManager Plugins { get { return _plugins; } set { _plugins = value; _plugins.Load(Player?.ExcludedSources); } }

        public Player _current;
        public Player Player
        {
            get { return _current; }
            set
            {
                _current = value;
                _current.Context = this;
                if (_current == null)
                {
                    Plugins?.Load(null);
                    if (ExcludedSources.Count > 0)
                    {
                        ExcludedSources.Clear();
                        SourcesChangedEvent?.Invoke(_current, null);
                    }
                }
                else
                {
                    Plugins?.Load(value.ActiveHouseRules);
                    if (!ExcludedSources.SetEquals(value.ExcludedSources))
                    {
                        ExcludedSources.Clear();
                        ExcludedSources.UnionWith(value.ExcludedSources);
                        SourcesChangedEvent?.Invoke(_current, null);
                    }
                }
            }
        }
        public string lastid = "";
        public void MakeHistory(string id = null)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                if (id == null) id = "";
                if (id == "" || id != lastid)
                {
                    byte[] port = Player.Portrait;
                    byte[] fac = Player.FactionImage;
                    Player.Portrait = null;
                    Player.FactionImage = null;
                    Player.Serializer.Serialize(mem, Player);
                    mem.Seek(0, SeekOrigin.Begin);
                    UndoBuffer.AddLast((Player)Player.Serializer.Deserialize(mem));
                    Player.Portrait = port;
                    Player.FactionImage = fac;
                    UndoBuffer.Last.Value.Portrait = port;
                    UndoBuffer.Last.Value.FactionImage = fac;
                    foreach (Possession pos in UndoBuffer.Last.Value.Possessions) if (pos.Description != null) pos.Description = pos.Description.Replace("\n", Environment.NewLine);
                    for (int i = 0; i < UndoBuffer.Last.Value.Journal.Count; i++) UndoBuffer.Last.Value.Journal[i] = UndoBuffer.Last.Value.Journal[i].Replace("\n", Environment.NewLine);
                    for (int i = 0; i < UndoBuffer.Last.Value.ComplexJournal.Count; i++) if (UndoBuffer.Last.Value.ComplexJournal[i].Text != null) UndoBuffer.Last.Value.ComplexJournal[i].Text = UndoBuffer.Last.Value.ComplexJournal[i].Text.Replace("\n", Environment.NewLine);
                    UndoBuffer.Last.Value.Allies = UndoBuffer.Last.Value.Allies.Replace("\n", Environment.NewLine);
                    UndoBuffer.Last.Value.Backstory = UndoBuffer.Last.Value.Backstory.Replace("\n", Environment.NewLine);
                    RedoBuffer.Clear();
                    HistoryButtonChange?.Invoke(Player, true, false);
                    if (UndoBuffer.Count > MaxBuffer) UndoBuffer.RemoveFirst();
                    UnsavedChanges++;
                }
                lastid = id;
                Player.ChoiceCounter.Clear();
                Player.ChoiceTotal.Clear();
            }
        }
        public event HistoryButtonChangeEvent HistoryButtonChange;
        public event EventHandler SourcesChangedEvent;
        public bool Undo()
        {
            if (UndoBuffer.Count > 0)
            {
                lastid = "";
                RedoBuffer.AddLast(Player);
                Player = UndoBuffer.Last.Value;
                UndoBuffer.RemoveLast();
                if (UnsavedChanges > 0) UnsavedChanges--;
                HistoryButtonChange?.Invoke(Player, UndoBuffer.Count > 0, true);
                return true;
            }
            return false;
        }
        public bool Redo()
        {
            if (RedoBuffer.Count > 0)
            {
                lastid = "";
                UndoBuffer.AddLast(Player);
                Player = RedoBuffer.Last.Value;
                RedoBuffer.RemoveLast();
                UnsavedChanges++;
                HistoryButtonChange?.Invoke(Player, true, RedoBuffer.Count > 0);
                return true;
            }
            return false;
        }
        public bool CanUndo()
        {
            return UndoBuffer.Count > 0;
        }
        public bool CanRedo()
        {
            return RedoBuffer.Count > 0;
        }

        public BuilderContext(Player p)
        {
            Player = p;
        }
        public BuilderContext()
        {
            Player = new Player();
        }
    }
}
