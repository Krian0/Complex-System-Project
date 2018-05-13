namespace EnvironInfo
{
    using UnityEngine;
    using EnvironEnum.DamageEnum;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "NewDamageInfo.asset", menuName = "Environ/Info/New DamageInfo", order = 1)]
    public class DamageInfo : ScriptableObject
    {
        [Space(10)]
        public DamageType ID;
        public float damage;
        [HideInInspector] public float attackGap;
        [HideInInspector] public float gapTimer;
        [HideInInspector] public float delay;
        [HideInInspector] public float delayTimer;
        [HideInInspector] public bool refreshDelay = false;

        [Space(40)]
        [HideInInspector] public DamageLimit limitType;

        [HideInInspector] public float limit = 0;
        [HideInInspector] public float limitTracker;
        [HideInInspector] public bool removeOnLimitReached;

        [HideInInspector] public bool refreshLimit = false;

        [HideInInspector] public bool canAttackFlag;
        [HideInInspector] public bool removeOutputFlag = false;

        [HideInInspector] public bool debugMode;


        public void Setup()
        {
            delayTimer = delay;
            gapTimer = 0;
            limitTracker = limit;
        }

        public bool CanAttack()
        {
            canAttackFlag = false;

            if (delayTimer > 0)
                delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                if (gapTimer > 0)
                    gapTimer -= Time.deltaTime;

                if (gapTimer <= 0)
                {
                    gapTimer = attackGap;

                    switch (limitType)
                    {
                        case DamageLimit.NO_LIMIT:
                            canAttackFlag = true;
                            break;

                        case DamageLimit.TIME_LIMIT:
                            limitTracker -= Time.deltaTime;
                            if (limitTracker > 0)
                                canAttackFlag = true;
                            break;

                        case DamageLimit.ATTACK_LIMIT:
                            if (limitTracker > 0)
                            {
                                canAttackFlag = true;
                                limitTracker -= 1;
                            }
                            break;

                        case DamageLimit.DAMAGE_LIMIT:
                            if (limitTracker > 0)
                                canAttackFlag = true;
                            break;

                        default:
                            break;
                    }
                }
            }

            if (limitTracker <= 0)
                removeOutputFlag = true;

            return canAttackFlag;
        }

        public float UpdateLimit(float damageValue)
        {
            if (limitType == DamageLimit.DAMAGE_LIMIT && damageValue < 0)
                limitTracker -= damageValue;

            return damageValue;
        }

        public void ResetLimit()
        {
            limitTracker = limit;
        }

        public void ResetDelay()
        {
            delayTimer = delay;
        }


        public static bool operator ==(DamageInfo a, DamageInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            //Neither ID or limitType will compare. It worked, then it suddenly didn't. I have no idea what broke it.

            return ((int)a.ID == (int)b.ID) && (a.damage == b.damage) && (a.attackGap == b.attackGap) && (a.delay == b.delay) && ((int)a.limitType == (int)b.limitType);
        }

        public static bool operator !=(DamageInfo a, DamageInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            DamageInfo di = (DamageInfo)o;

            return this == di;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(DamageInfo))]
    public class DamageInfoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DamageInfo script = (DamageInfo)target;

            DrawDefaultInspector();
            script.attackGap = EditorGUILayout.FloatField(new GUIContent("Attack Gap", "The time in seconds that should pass between each attack"), script.attackGap);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            script.delay = EditorGUILayout.FloatField(new GUIContent("Initial Attack Delay", "The time in seconds to delay the initial attack"), script.delay);
            script.refreshDelay = EditorGUILayout.Toggle(new GUIContent("Refresh Delay on Existing Transfer?", "Controls whether the attack delay should reset if the effect already exists on an object"), script.refreshDelay);
            EditorGUILayout.Space();

            script.limitType = (DamageLimit)EditorGUILayout.EnumPopup(new GUIContent("Limit Type", "Controls when the damage should stop"), script.limitType);

            if (script.limitType != DamageLimit.NO_LIMIT)
            {
                string limitName = script.limitType.ToString();
                limitName = limitName[0] + limitName.Substring(1, limitName.IndexOf("_") - 1).ToLower() + " Limit";
                script.limit = EditorGUILayout.FloatField(new GUIContent(limitName, "Attacks can only be made when the " + limitName + " is above 0"), script.limit);
                script.refreshLimit = EditorGUILayout.Toggle(new GUIContent("Refresh Limit?", "Controls whether the " + limitName + " should reset if the effect already exists on an object"), script.refreshLimit);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            script.debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Shows hidden variables in inspector for debugging purposes"), script.debugMode);

            if (script.debugMode)
            {
                EditorGUILayout.Space();
                script.delayTimer = EditorGUILayout.FloatField("Delay Timer", script.delayTimer);
                script.gapTimer = EditorGUILayout.FloatField("Gap Timer", script.gapTimer);

                if (script.limitType != DamageLimit.NO_LIMIT)
                    script.limitTracker = EditorGUILayout.FloatField("Limit Tracker", script.limitTracker);

                script.canAttackFlag = EditorGUILayout.Toggle("Can Attack Flag", script.canAttackFlag);
                script.removeOutputFlag = EditorGUILayout.Toggle("Remove OutputFlag", script.removeOutputFlag);
            }
        }
    }
#endif
}