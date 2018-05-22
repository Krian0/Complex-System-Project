namespace EnvironEnum
{
    namespace GeneralEnum
    {
        public enum TransferCondition
        {
            ON_COLLISION_ENTER = 0,
            ON_COLLISION_EXIT,
            ON_COLLISION_STAY,
            ON_TRIGGER_ENTER,
            ON_TRIGGER_EXIT,
            ON_TRIGGER_STAY
        }

        public enum TerminalCondition
        {
            ON_COLLISION_EXIT = 0,
            ON_TRIGGER_EXIT,
            ON_TIMER,
            NONE
        }

        public enum Similarity
        {
            UNIQUE_A = 0,
            UNIQUE_B,
            STANDARD_A,
            STANDARD_B,
            SELECTIVE_A,
            SELECTIVE_B
        }

        struct SimilarityCheck
        {
            public static bool IsUnique(Similarity sIndex)
            {
                return (sIndex == Similarity.UNIQUE_A || sIndex == Similarity.UNIQUE_B);
            }

            public static bool IsStandard(Similarity sIndex)
            {
                return (sIndex == Similarity.STANDARD_A || sIndex == Similarity.STANDARD_B);
            }

            public static bool IsSelective(Similarity sIndex)
            {
                return (sIndex == Similarity.SELECTIVE_A || sIndex == Similarity.SELECTIVE_B);
            }
        }
    }

    namespace DamageEnum
    {
        public enum DType
        {
            NULL = 0,
            FIRE,
            WATER,
            NATURE,
            FROST,
            ELECTRICITY,
            PHYSICAL
        }

        public enum DLimit
        {
            NO_LIMIT = 0,
            TIME_LIMIT,
            ATTACK_LIMIT,
            DAMAGE_LIMIT
        }
    }

    namespace ResistanceEnum
    {
        public enum RType
        {
            NULLIFY_DAMAGE = 0,
            REDUCE_DAMAGE,
            MULTIPLY_DAMAGE,
            HEAL
        }
    }

    namespace AppearanceEnum
    {

    }

    namespace DestructAndSpawnEnum
    {
        public enum DestructOrSpawnCondition
        {
            ON_COLLISION_ENTER = 0,
            ON_COLLISION_EXIT,
            ON_TRIGGER_ENTER,
            ON_TRIGGER_EXIT,
            TIMER_ZERO,
            HITPOINTS_ZERO,
            DAMAGE_TYPE
        }
    }

    //namespace TagEnum
    //{
    //    public enum ObjectTag
    //    {
    //        PLAYER = 0,
    //        KILL_ZONE
    //    }
    //}
}
