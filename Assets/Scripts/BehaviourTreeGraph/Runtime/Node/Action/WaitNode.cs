using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviourTreeGraph.Runtime.Node.Action
{
    public class WaitNode : ActionNode
    {
        [FormerlySerializedAs("tick")] public float waitSec;

        private float _startTime;

        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - _startTime > waitSec)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}