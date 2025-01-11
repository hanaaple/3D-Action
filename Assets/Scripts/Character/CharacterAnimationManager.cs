using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Animator를 관리해주는 클래스
    /// </summary>
    public class CharacterAnimationManager : MonoBehaviour
    {
        public bool isBusy;
        
        public Action OnAnimationEnd;

        private Animator _animator;
        
        private static readonly Dictionary<string, int> AnimationIds = new();

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetBool(string key, bool value)
        {
            var id = TryGetAnimationHash(key);
            
            _animator.SetBool(id, value);
        }
        
        public void SetTrigger(string key)
        {
            var id = TryGetAnimationHash(key);
            
            _animator.SetTrigger(id);
        }

        public void SetInteger(string key, int value)
        {
            var id = TryGetAnimationHash(key);
            
            _animator.SetInteger(id, value);
        }

        public void SetFloat(string key, float value)
        {
            var id = TryGetAnimationHash(key);
            
            _animator.SetFloat(id, value);
        }
        
        private int TryGetAnimationHash(string key)
        {
            if (!AnimationIds.TryGetValue(key, out var id))
            {
                id = Animator.StringToHash(key);
                AnimationIds.Add(key, id);
            }

            return id;
        }

    }
}