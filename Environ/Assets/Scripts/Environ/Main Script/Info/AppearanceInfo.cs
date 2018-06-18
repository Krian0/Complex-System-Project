namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Environ.Support.Enum.Damage;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Linq;
    using Support.Containers;
#endif

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New AppearanceInfo", order = 1)]
    public class AppearanceInfo : EnvironInfoBase
    {
        [Space(10)]
        public ParticleSystem particle;
        public Material material;
        public uint priority;
        public MeshRenderer mRenderer;

        public bool particlesOn;
        public bool materialOn;

        public bool hideOnResistance;
        public List<DType> hideIDList;


        public void Setup(GameObject targetObj)
        {
            if (particle)
            {
                particle = Instantiate(particle);
                particle.transform.position = targetObj.transform.position;
                particle.transform.SetParent(targetObj.transform);

                particlesOn = true;
            }

            mRenderer = targetObj.GetComponent<MeshRenderer>();
            materialOn = true;
        }

        public void SetupRenderer()
        {
            if (!mRenderer)
                return;

            if (!material)
                material = mRenderer.material;
            else
                mRenderer.material = material;
        }

        public void UpdateAppearance()
        {
            if (particle)
            {
                if (particlesOn && !particle.isPlaying)
                    particle.Play();

                else if (!particlesOn && particle.isPlaying)
                    particle.Stop();
            }
        }


        public void SetRendererMaterial()
        {
            if (mRenderer)
                mRenderer.material = material;
        }

        public void SetRendererMaterial(Material mat)
        {
            if (mRenderer)
                mRenderer.material = mat;
        }

        public void StopAndDestroy()
        {
            if (particle)
            {
                particle.Stop();
                Destroy(particle.gameObject, 4);
            }
        }

        public void TurnOn()
        {
            particlesOn = true;
            materialOn = true;
        }

        public void TurnOff()
        {
            particlesOn = false;
            materialOn = false;
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

        SerializedProperty particle;
        SerializedProperty material;
        SerializedProperty priority;

        SerializedProperty particlesOn;
        SerializedProperty materialOn;

        SerializedProperty hideOnResistance;
        SerializedProperty hideIDList;

        SerializedProperty debugMode;

        private void OnEnable()
        {
            particle = serializedObject.FindProperty("particle");
            material = serializedObject.FindProperty("material");
            priority = serializedObject.FindProperty("priority");

            particlesOn = serializedObject.FindProperty("particlesOn");
            materialOn = serializedObject.FindProperty("materialOn");

            hideOnResistance = serializedObject.FindProperty("hideOnResistance");
            hideIDList = serializedObject.FindProperty("hideIDList");

            debugMode = serializedObject.FindProperty("debugMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(particle);
            EditorGUILayout.PropertyField(material);
            EditorGUILayout.PropertyField(priority);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(hideOnResistance);
            if (hideOnResistance.boolValue)
                EditorGUILayout.PropertyField(hideIDList, true);
            GUILayout.Space(20);

            ShowDebug();

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowDebug()
        {
            EditorGUI.indentLevel += 1;

            EditorGUILayout.PropertyField(debugMode, new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"));
            if (debugMode.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(particlesOn);
                EditorGUILayout.PropertyField(materialOn);
            }

            EditorGUI.indentLevel -= 1;
        }
    }
#endif
}