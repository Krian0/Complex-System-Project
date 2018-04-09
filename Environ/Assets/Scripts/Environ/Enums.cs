
namespace EnvironEnum
{
    namespace DamageEnum
    {
        public enum DamageCondition
        {
            ON_CONTACT = 0,
            ON_TRIGGER_ENTER,
            ON_TRIGGER_EXIT,
            ON_TRIGGER_STAY
        }

        public enum DamageRegularity
        {
            UNTIL_DESTROYED = 0,
            ONCE,
            TIME_LIMIT,
            ATTACK_LIMIT,
            DAMAGE_LIMIT
        }

        //Do not change, move or remove NULL
        public enum DamageType
        {
            NULL = 0,
            FIRE,
            WATER,
            NATURE,
            FROST,
            ELECTRICITY,
            PHYSICAL
        }
    }

    namespace ResistanceEnum
    {
        public enum ResistanceType
        {
            NULLIFY_DAMAGE = 0,
            REDUCE_DAMAGE,
            MULTIPLY_DAMAGE,
            HEAL
        }
    }

    namespace DestructAndSpawnEnum
    {
        public enum DestructOrSpawnCondition
        {
            ON_CONTACT = 0,
            ON_TRIGGER_ENTER,
            ON_TRIGGER_EXIT,
            TIMER_ZERO,
            HITPOINTS_ZERO,
            DAMAGE_TYPE
        }
    }

    namespace TagEnum
    {
        public enum ObjectTag
        {
            PLAYER = 0,
            KILL_ZONE
        }
    }
}
