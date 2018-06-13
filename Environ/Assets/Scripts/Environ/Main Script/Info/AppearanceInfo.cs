namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Environ.Support.Enum.Damage;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Linq;
#endif

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New AppearanceInfo", order = 1)]
    public class AppearanceInfo : EnvironInfoBase
    {
        [Space(10)]
        public ParticleSystem objectParticle;
        public Material objectMaterial; //implement this

        public bool particlesOn;
        public bool materialOn;         //and this

        public bool hideOnResistance;
        public List<DType> hideIDList;


        public void Setup(Transform objTransform)
        {
            if (objectParticle != null)
            {
                objectParticle = Instantiate(objectParticle);
                objectParticle.transform.position = objTransform.position;
                objectParticle.transform.SetParent(objTransform);

                particlesOn = true;
            }

            if (objectMaterial != null)
            {
                MeshRenderer mRenderer = objTransform.GetComponent<MeshRenderer>();
                if (mRenderer != null)
                {
                    List<Material> materials = new List<Material>(mRenderer.materials);
                    materials.Add(objectMaterial);
                    mRenderer.materials = materials.ToArray();
                }
            }
        }

        public void UpdateAppearance()
        {
            if (particlesOn && !objectParticle.isPlaying)
                objectParticle.Play();

            else if (!particlesOn)
                objectParticle.Stop();
        }

        private void AddMaterial(MeshRenderer mRenderer)
        {

        }

        public List<DType> GetDistinctHideIDList()
        {
            return hideIDList.Distinct().ToList();
        }


        #region Operator Overrides
        public static bool operator ==(AppearanceInfo a, AppearanceInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.uniqueID == b.uniqueID;
        }

        public static bool operator !=(AppearanceInfo a, AppearanceInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (AppearanceInfo)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AppearanceInfo))]
    public class AppearanceInfoEditor : Editor
    {

        SerializedProperty objectParticle;
        SerializedProperty objectMaterial;

        SerializedProperty particlesOn;
        SerializedProperty materialOn;

        SerializedProperty hideOnResistance;
        SerializedProperty hideIDList;

        SerializedProperty debugMode;

        private void OnEnable()
        {
            objectParticle = serializedObject.FindProperty("objectParticle"); ;
            objectMaterial = serializedObject.FindProperty("objectMaterial"); ;

            particlesOn = serializedObject.FindProperty("particlesOn"); ;
            materialOn = serializedObject.FindProperty("materialOn"); ;

            hideOnResistance = serializedObject.FindProperty("hideOnResistance"); ;
            hideIDList = serializedObject.FindProperty("hideIDList");

            debugMode = serializedObject.FindProperty("debugMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(objectParticle);
            EditorGUILayout.PropertyField(objectMaterial);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(hideOnResistance);

            if (hideOnResistance.boolValue)
                EditorGUILayout.PropertyField(hideIDList, true);

            GUILayout.Space(20);


            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(debugMode, new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"));
            if (debugMode.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(particlesOn);
                EditorGUILayout.PropertyField(materialOn);
            }
            EditorGUI.indentLevel -= 1;

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}