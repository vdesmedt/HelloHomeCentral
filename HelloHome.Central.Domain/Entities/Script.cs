using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public class Script
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Trigger Trigger { get; set; }
        public int TriggerId { get; set; }
        public IList<ScriptAction> Actions { get; set; }
        public Script OnFinnish { get; set; }
    }

    public class ScriptAction
    {
        public Script Script { get; set; }
        public int ScriptId { get; set; }
        public Action Action { get; set; }
        public int ActionId { get; set; }
        public int Sequence { get; set; }
    }
}