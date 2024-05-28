using System;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    [CustomEditor(typeof(EnemySetup))]
    public class EnemyEditor : Editor
    {
        private SerializedProperty _followDistance;
        private SerializedProperty _animator;
        private SerializedProperty _thinkDelay;
        private SerializedProperty _idleOffset;
        private SerializedProperty _detector;
        private SerializedProperty _foodPart;
        
        [Space, Header("Enemy Type")] 
        private SerializedProperty _type;
        private SerializedProperty _agent;
        private SerializedProperty _sound;
        private SerializedProperty _particle;
        private SerializedProperty _swarm;
        
        private void OnEnable()
        {
            _followDistance = serializedObject.FindProperty(nameof(_followDistance));
            _animator = serializedObject.FindProperty(nameof(_animator));
            _thinkDelay = serializedObject.FindProperty(nameof(_thinkDelay));
            _idleOffset = serializedObject.FindProperty(nameof(_idleOffset));
            _detector = serializedObject.FindProperty(nameof(_detector));
            _foodPart = serializedObject.FindProperty(nameof(_foodPart));
            
            _type = serializedObject.FindProperty(nameof(_type));
            _agent = serializedObject.FindProperty(nameof(_agent));
            _sound = serializedObject.FindProperty(nameof(_sound));
            _particle = serializedObject.FindProperty(nameof(_particle));
            _swarm = serializedObject.FindProperty(nameof(_swarm));
        }

        public override void OnInspectorGUI()
        {
            DrawMain();
            DrawAdditions();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMain()
        {
            EditorGUILayout.PropertyField(_followDistance);
            EditorGUILayout.PropertyField(_animator);
            EditorGUILayout.PropertyField(_thinkDelay);
            EditorGUILayout.PropertyField(_idleOffset);
            EditorGUILayout.PropertyField(_detector);
            EditorGUILayout.PropertyField(_foodPart);
            EditorGUILayout.PropertyField(_type);
        }

        private void DrawAdditions()
        {
            switch ((EnemyTypes)_type.enumValueIndex)
            {
                case EnemyTypes.Mover:
                    ShowMoverOptions();
                    break;
                
                case EnemyTypes.Toxin:
                    ShowToxinOptions();
                    break;
                
                case EnemyTypes.Swarm:
                    ShowSwarmOptions();
                    break;

                case EnemyTypes.Hider:
                    ShowHiderOptions();
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ShowMoverOptions()
        {
            _sound.objectReferenceValue = null;
            _particle.objectReferenceValue = null;
            _swarm.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_agent);
        }
        
        private void ShowToxinOptions()
        {
            _agent.objectReferenceValue = null;
            _swarm.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_sound);
            EditorGUILayout.PropertyField(_particle);
        }
        
        private void ShowSwarmOptions()
        {
            _particle.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_agent);
            EditorGUILayout.PropertyField(_sound);
            EditorGUILayout.PropertyField(_swarm);
        }

        private void ShowHiderOptions()
        {
            _sound.objectReferenceValue = null;
            _swarm.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_agent);
            EditorGUILayout.PropertyField(_particle);
        }
    }
}