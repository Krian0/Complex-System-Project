namespace Environ.Support.Enum
{
    namespace General
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
            UNIQUE = 0,
            STANDARD,
            SELECTIVE
        }
    }

    namespace Damage
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

    namespace Resistance
    {
        public enum RType
        {
            NULLIFY_DAMAGE = 0,
            REDUCE_DAMAGE,
            MULTIPLY_DAMAGE,
            HEAL
        }
    }

    namespace Appearance
    {

    }

    namespace DestructAndSpawn
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

    namespace Tag
    {
        public enum ObjectTag
        {
            PLAYER = 0,
            GRUNT,
            BOSS,
            ENVIRONMENT
        }
    }
}
