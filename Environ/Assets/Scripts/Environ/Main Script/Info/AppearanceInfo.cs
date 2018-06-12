namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New AppearanceInfo", order = 1)]
    public class AppearanceInfo : ScriptableObject
    {
        [Space(10)]
        public ParticleSystem objectParticle;
        public Material objectMaterial;

        [HideInInspector] public bool particlesOn;
        [HideInInspector] public bool materialOn;

        [HideInInspector]
        public string uniqueID = "0";

#if UNITY_EDITOR
        [HideInInspector] public bool debugMode;
#endif


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
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AppearanceInfo))]
    public class AppearanceInfoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AppearanceInfo script = (AppearanceInfo)target;

            EditorExtender.DrawCustomInspector(this);
            GUILayout.Space(20);

            EditorGUI.indentLevel += 1;
            script.debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), script.debugMode);
            if (script.debugMode)
            {
                EditorGUILayout.Space();
                script.particlesOn = EditorGUILayout.Toggle("Particles On", script.particlesOn);
                script.materialOn = EditorGUILayout.Toggle("Material On", script.materialOn);
            }
            EditorGUI.indentLevel -= 1;
        }
    }
#endif
}