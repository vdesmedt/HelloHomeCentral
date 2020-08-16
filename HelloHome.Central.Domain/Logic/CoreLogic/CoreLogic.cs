using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = System.Action;

namespace HelloHome.Central.Domain.Logic.CoreLogic
{
    public interface ICoreLogic
    {
        IList<Entities.Action> GetActionsFor(Trigger trigger);
    }

    public class CoreLogic : ICoreLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private IList<Script> _scripts;

        public CoreLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IList<Entities.Action> GetActionsFor(Trigger trigger)
        {
            List<Entities.Action> actions = new List<Entities.Action>();
            var triggeredScripts = _unitOfWork.Scripts
                .Include("Conditions.Condition.Port")
                .Where(s => s.TriggerId == trigger.Id)
                .ToList();
            var scriptToRun  = triggeredScripts.Where(s => s.Conditions.All(Met)).ToList();
            foreach (var script in scriptToRun)
            {
                var scriptWithAction = _unitOfWork.Scripts
                    .Include("Actions.Action.Actuator.Node")
                    .Single(s => s.Id == script.Id);
                foreach(var a in scriptWithAction.Actions)
                    actions.Add(a.Action);
            }

            return actions;

            bool Met(ScriptCondition scriptConditions)
            {
                return scriptConditions.Condition.Check();
            }
        }
    }
}