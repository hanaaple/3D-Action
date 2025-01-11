using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BehaviourTreeGraph.Runtime
{
    public class BehaviourTreeGraphAsset : ScriptableObject
    {
        public BehaviourTreeGraphNode.NodeState treeState = BehaviourTreeGraphNode.NodeState.Running;
        public BehaviourTreeGraphNode rootNode;
        
        // have to check why undo.record(node) did not work
        public List<BehaviourTreeGraphNode> nodes = new();

        
        // leaf node (action node)에서 
        
        public BehaviourTreeGraphNode.NodeState Update()
        {
            // if (rootNode.state is BehaviourTreeGraphNode.NodeState.Running or BehaviourTreeGraphNode.NodeState.Waiting)
            // {
            treeState = rootNode.Update();
            // }

            return treeState;
        }

        public BehaviourTreeGraphNode CreateNode(Type type, string guid = "")
        {
            var node = ScriptableObject.CreateInstance(type) as BehaviourTreeGraphNode;
            node.Initialize(type.Name, guid);
            nodes.Add(node);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }

            AssetDatabase.SaveAssets();
#endif

            return node;
        }

        public void DeleteNode(BehaviourTreeGraphNode node)
        {
            nodes.Remove(node);
            
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
#endif
        }

        public BehaviourTreeGraphAsset Clone()
        {
            // Debug.Log("Clone");
            BehaviourTreeGraphAsset tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<BehaviourTreeGraphNode>();
            tree.rootNode.Traverse(node => { tree.nodes.Add(node); });
            return tree;
        }

        public virtual void Bind(BlackBoard blackBoard)
        {
            rootNode.Traverse(node =>
            {
                node.blackboard = blackBoard;
            });
        }
    }
}