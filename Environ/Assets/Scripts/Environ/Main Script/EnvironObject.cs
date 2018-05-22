namespace Environ
{
    using UnityEngine;
    using EnvironEnum.GeneralEnum;
    using EnvironEnum.DamageEnum;
    using EnvironInfo;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class EnvironObject : MonoBehaviour
    {
        public float hitPointLimit;
        public float hitPoints;

        public ResistanceInfo resistances;
        public AppearanceInfo appearance;
        //public List<DestructionInfo> destroyConditions;

        public List<EnvironOutput> output;
        public EnvironEffectList effects;


        void Start()
        {
            for (int i = output.Count - 1; i >= 0; i--)
            {
                if (output[i] == null)
                    output.RemoveAt(i);
                else
                {
                    if (SimilarityCheck.IsUnique(output[i].similarity))
                        output[i] = Instantiate(output[i]);
                    output[i].SetSourceAndUID(this);
                }
            }

            effects = new EnvironEffectList();

            if (hitPoints == 0)
                hitPoints = hitPointLimit;
        }

        private void Update()
        {
            if (effects == null) return;

            foreach (EnvironOutput e in effects.inputList)
            {
                if (e.damageI != null)
                    if (e.damageI.CanAttack())                       //Passes adjusted damage through UpdateLimit
                        hitPoints -= e.damageI.UpdateLimit(GetAdjustedDamage(e.damageI.damage, e.damageI.ID));

                if (e.appearanceI != null)
                    e.appearanceI.UpdateAppearance(); 
            }

            if (hitPoints < 0)
                hitPoints = 0;
            if (hitPoints > hitPointLimit)
                hitPoints = hitPointLimit;
        }

        private float GetAdjustedDamage(float damage, DType damageID)
        {
            if (resistances == null)
                return damage;

            return resistances.GetAdjustedDamage(damage, damageID);
        }


        #region OnTrigger & OnCollision Functions
        private void OnTriggerEnter(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_ENTER, other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            OnEnterOrStay(TransferCondition.ON_TRIGGER_STAY, other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(TransferCondition.ON_TRIGGER_EXIT, TransferCondition.ON_TRIGGER_STAY, TerminalCondition.ON_TRIGGER_EXIT, other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_ENTER, collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnEnterOrStay(TransferCondition.ON_COLLISION_STAY, collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(TransferCondition.ON_COLLISION_EXIT, TransferCondition.ON_COLLISION_STAY, TerminalCondition.ON_COLLISION_EXIT, collision.gameObject);
        }
        #endregion

        private void OnEnterOrStay(TransferCondition enter, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
                if (eo.transferCondition == enter)
                    otherEO.effects.Add(eo, obj.transform);

            //foreach (EnvironOutput eo in output)
            //    if (eo.transferOnCondition == enter || eo.transferOnCondition == stay)
            //        otherEO.effects.Add(eo, obj.transform);
        }


        private void OnExit(TransferCondition exit, TransferCondition stay, TerminalCondition endCondition, GameObject obj)
        {
            EnvironObject otherEO = obj.GetComponent<EnvironObject>();
            if (otherEO == null || output.Count == 0 || otherEO.effects == null)
                return;

            foreach (EnvironOutput eo in output)
            {
                if (eo.transferCondition == exit)
                    otherEO.effects.Add(eo, obj.transform);

                if (eo.transferCondition == stay || eo.endOnCondition == endCondition)
                    otherEO.effects.Remove(eo);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(EnvironObject))]
    public class EnvironObjectEditor : Editor
    {
        [HideInInspector] public bool debugMode;

        public override void OnInspectorGUI()
        {
            EnvironObject script = (EnvironObject)target;
            EditorExtender.DrawCustomInspector(this);

            GUILayout.Space(20);
            EditorGUI.indentLevel += 1;
            debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), debugMode);
            GUILayout.Space(20);
            if (debugMode)
            {
                int i = 0;
                foreach (EnvironOutput eo in script.output)
                {
                    i++;
                    EditorGUILayout.LabelField("Output " + i + " Unique ID: " + eo.uniqueID);
                    eo.uniqueID = EditorGUILayout.TextField(eo.uniqueID);
                }

                if (EditorApplication.isPlaying)
                    Repaint();
            }
            EditorGUI.indentLevel -= 1;
        }
    }
#endif
}