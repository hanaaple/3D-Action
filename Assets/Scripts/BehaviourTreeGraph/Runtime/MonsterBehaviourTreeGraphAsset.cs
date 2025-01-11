using UnityEngine;

namespace BehaviourTreeGraph.Runtime
{
    [CreateAssetMenu(menuName = "BehaviourTree/New Monster Graph")]
    public class MonsterBehaviourTreeGraphAsset : BehaviourTreeGraphAsset
    {
        // For Debug
        public MonsterBlackBoard blackboard;

        public override void Bind(BlackBoard blackBoard)
        {
            blackboard = blackBoard as MonsterBlackBoard;
            base.Bind(blackBoard);

            rootNode.Traverse(node => { node.InitializeBlackBoard(); });
        }
    }
}