namespace Environ
{
    using UnityEngine;
    using EnvironInfo;
    using EnvironEnum.GeneralEnum;

#if UNITY_EDITOR
    using UnityEditor;
    using EnvironGuiContent;
    using System.Linq;
#endif

    [CreateAssetMenu(fileName = "NewOutput.asset", menuName = "Environ/New Output", order = 1)]
    public class EnvironOutput : ScriptableObject
    {
        [Space(10)]
        [HideInInspector] public TransferCondition transferCondition = TransferCondition.ON_COLLISION_ENTER;
        [HideInInspector] public bool allowTransmission = false;
        [HideInInspector] public Similarity similarity = Similarity.STANDARD_A;

        [HideInInspector] public TerminalCondition endOnCondition;

        [HideInInspector] public bool refreshTimer = false;
        [HideInInspector] public float timeLimit;
        [HideInInspector] public float limitTimer;

        [HideInInspector] public DamageInfo damageI;
        [HideInInspector] public AppearanceInfo appearanceI;
        //public DestructionInfo destructionI;

        [HideInInspector] public EnvironObject source;
        [HideInInspector] public EnvironObject target;

        [HideInInspector]
        public string uniqueID = "0";



        public void SetSourceAndUID(EnvironObject eoSource)
        {
            source = eoSource;
            if (SimilarityCheck.IsStandard(similarity) || SimilarityCheck.IsUnique(similarity))
                uniqueID = GetInstanceID().ToString();

            if (damageI)
                damageI.uniqueID = GetInstanceID().ToString();
            if (appearanceI)
                appearanceI.uniqueID = GetInstanceID().ToString();
        }

        public static EnvironOutput SetSourceAndUID(EnvironOutput output, EnvironObject eoSource)
        {
            if (output == null)
                return null;

            output.source = eoSource;
            if (SimilarityCheck.IsStandard(output.similarity) || SimilarityCheck.IsUnique(output.similarity))
                output.uniqueID = output.GetInstanceID().ToString();

            if (output.damageI)
                output.damageI.uniqueID = output.GetInstanceID().ToString();
            if (output.appearanceI)
                output.appearanceI.uniqueID = output.GetInstanceID().ToString();

            return output;
        }

        public void Setup(Transform objTransform)
        {
            if (damageI != null)
            {
                damageI = Instantiate(damageI);
                damageI.Setup();
            }

            if (appearanceI != null)
            {
                appearanceI = Instantiate(appearanceI);
                appearanceI.Setup(objTransform);
            }

            //if (destructionI != null)
            //    destructionI = Instantiate(destructionI);

            limitTimer = timeLimit;

            if (SimilarityCheck.IsUnique(similarity))
                uniqueID = GetInstanceID().ToString();
        }

        public void Refresh()
        {
            if (refreshTimer)
                limitTimer = timeLimit;

            if (damageI != null)
                damageI.Refresh();
        }

        public void Update()
        {
            if (limitTimer > 0)
                limitTimer -= Time.deltaTime;
        }


#region Operator Overloads
        public static bool operator ==(EnvironOutput a, EnvironOutput b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;


            if (a.similarity == Similarity.UNIQUE_A || b.similarity == Similarity.UNIQUE_A || a.similarity == Similarity.STANDARD_A || b.similarity == Similarity.STANDARD_A)
                return a.uniqueID == b.uniqueID;

            if (a.similarity == Similarity.UNIQUE_B || b.similarity == Similarity.UNIQUE_B || a.similarity == Similarity.STANDARD_B || b.similarity == Similarity.STANDARD_B)
                if (a.uniqueID == b.uniqueID)
                    return true;

            if (a.similarity == Similarity.SELECTIVE_A || b.similarity == Similarity.SELECTIVE_A)
            {
               
            }

            if (a.similarity == Similarity.SELECTIVE_B || b.similarity == Similarity.SELECTIVE_B)
            {
                return (a.damageI == b.damageI) && (a.appearanceI == b.appearanceI);
            }

            return false;
        }

        public static bool operator !=(EnvironOutput a, EnvironOutput b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (EnvironOutput)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
#endregion
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(EnvironOutput))]
    public class EnvironOutputEditor : Editor
    {
        [HideInInspector]
        public bool debugMode;
        [HideInInspector]
        public GameObject sourceEO = null;
        [HideInInspector]
        public GameObject targetEO = null;
        [HideInInspector]
        public bool damageIFold = false;
        [HideInInspector]
        public bool appearanceIFold = false;


        public override void OnInspectorGUI()
        {
            EnvironOutput script = (EnvironOutput)target;
            EditorExtender.DrawCustomInspector(this);

            script.similarity = (Similarity)EditorGUILayout.EnumPopup(new GUIContent("Similarity Index", EGC.eUSSU), script.similarity);
            GUILayout.Space(20);

            script.transferCondition = (TransferCondition)EditorGUILayout.EnumPopup(EGC.eOut[0], script.transferCondition);
            script.allowTransmission = EditorGUILayout.Toggle(EGC.eOut[1], script.allowTransmission);

            if (script.transferCondition != TransferCondition.ON_COLLISION_STAY && script.transferCondition != TransferCondition.ON_TRIGGER_STAY)
            {
                script.endOnCondition = (TerminalCondition)EditorGUILayout.EnumPopup(EGC.eOut[2], script.endOnCondition);
                if (script.endOnCondition == TerminalCondition.ON_TIMER)
                {
                    script.timeLimit = EditorGUILayout.FloatField(EGC.eOut[3], script.timeLimit);
                    script.refreshTimer = EditorGUILayout.Toggle(EGC.eOut[4], script.refreshTimer);
                }

                GUILayout.Space(20);
            }


            script.damageI = (DamageInfo)EditorGUILayout.ObjectField("Damage Info", script.damageI, typeof(DamageInfo), false);
            if (script.damageI != null)
            {
                EditorGUI.indentLevel += 2;

                damageIFold = EditorGUILayout.Foldout(damageIFold, "Extended View");
                if (damageIFold)
                    CreateEditor(script.damageI).OnInspectorGUI();

                EditorGUI.indentLevel -= 2;
                GUILayout.Space(20);
            }

            script.appearanceI = (AppearanceInfo)EditorGUILayout.ObjectField("Appearance Info", script.appearanceI, typeof(AppearanceInfo), false);
            if (script.appearanceI != null)
            {
                EditorGUI.indentLevel += 2;

                appearanceIFold = EditorGUILayout.Foldout(appearanceIFold, "Extended View");
                if (appearanceIFold)
                    CreateEditor(script.appearanceI).OnInspectorGUI();

                EditorGUI.indentLevel -= 2;
                GUILayout.Space(20);
            }

            //script.destructionI = (destructionInfo)EditorGUILayout.ObjectField("Destruction Info", script.destructionI, typeof(destructionInfo), false);


            GUILayout.Space(20);
            EditorGUI.indentLevel += 1;
            debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), debugMode);
            if (debugMode)
            {
                EditorGUILayout.Space();
                if (script.source != null)
                    sourceEO = (GameObject)EditorGUILayout.ObjectField("Source", script.source.gameObject, typeof(GameObject), false);
                else
                    sourceEO = (GameObject)EditorGUILayout.ObjectField("Source", null, typeof(GameObject), false);

                if (script.target != null)
                    targetEO = (GameObject)EditorGUILayout.ObjectField("Target", script.target.gameObject, typeof(GameObject), false);
                else
                    targetEO = (GameObject)EditorGUILayout.ObjectField("Target", null, typeof(GameObject), false);
                EditorGUILayout.Space();

                if (script.endOnCondition == TerminalCondition.ON_TIMER)
                    script.limitTimer = EditorGUILayout.FloatField("Terminal Condition Timer", script.limitTimer);
                GUILayout.Space(20);

                EditorGUILayout.SelectableLabel("Unique ID: " + script.uniqueID);

                if (EditorApplication.isPlaying)
                    Repaint();
            }
            EditorGUI.indentLevel -= 1;
        }
    }
#endif
}