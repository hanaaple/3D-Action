using UnityEngine;

namespace BehaviourTreeGraph.Runtime
{
    // Inherit for Display Behaviour Tree Graph Editor
    public abstract class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTreeGraphAsset behaviourTree;

        protected void InitializeBtTree(BlackBoard blackboard)
        {
            behaviourTree = behaviourTree.Clone();
            // behaviourTree.InitializeNodeState();
            behaviourTree.Bind(blackboard);
        }
        
        protected virtual void Update()
        {
            behaviourTree.Update();
        }
    }
}