namespace EnvironInfo
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New AppearanceInfo", order = 1)]
    public class AppearanceInfo : ScriptableObject
    {
        public ParticleSystem objectParticle;
        public Transform anchorPoint;

        [Space(20)] public Texture objectTexture;
        [Space(20)] public Material objectMaterial;

        //Ask how to go about using an effect that is supposed to work ON_ENTER, but stops, for example, 10 seconds after ON_EXIT. Fire is odd without being able to do that.
        //Thinking of adding a "Terminal Condition" counterpart to the Transfer Condition, with an optional time delay. Not entirely sure how to make that work though.
        //Maybe Destroy(object, timeToWait)?

        //public float killAfter;
        //[HideInInspector] public float killAfterTimer;

        [HideInInspector] public bool particlesOn;
        [HideInInspector] public bool textureOn;
        [HideInInspector] public bool materialOn;

        [HideInInspector] public bool debugMode;


        public void Setup(Transform objTransform)
        {
            objectParticle = Instantiate(objectParticle);
            objectParticle.transform.position = objTransform.position;

            objectParticle.transform.SetParent(objTransform);

            Vector3 a = objectParticle.transform.lossyScale;
            Vector3 b = objTransform.lossyScale;

            objectParticle.transform.localScale = new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

            particlesOn = true;
        }

        public void UpdateAppearance()
        {


            if (particlesOn && !objectParticle.isPlaying)
                objectParticle.Play();

            else if (!particlesOn)
                objectParticle.Stop();
        }

        public static bool operator ==(AppearanceInfo a, AppearanceInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return (a.objectTexture == b.objectTexture) && (a.objectMaterial == b.objectMaterial) && (a.objectParticle == b.objectParticle);
        }

        public static bool operator !=(AppearanceInfo a, AppearanceInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            AppearanceInfo ai = (AppearanceInfo)o;

            return this == ai;
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

            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            script.debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), script.debugMode);

            if (script.debugMode)
            {
                EditorGUILayout.Space();
                script.particlesOn = EditorGUILayout.Toggle("Particles On", script.particlesOn);
                script.textureOn = EditorGUILayout.Toggle("Texture On", script.textureOn);
                script.materialOn = EditorGUILayout.Toggle("Material On", script.materialOn);
            }
        }
    }
#endif
}