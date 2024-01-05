namespace Demo
{
    public abstract class Ability
    {
        public bool CanUse => _countDown <= 0;
        public int Cooldown { get; protected init; }

        protected float _countDown;

        public Ability(int cooldown)
        {
            Cooldown = cooldown;
            _countDown = 0;
        }

        public virtual bool TryUse()
        {
            if (CanUse)
            {
                Use();
                _countDown = Cooldown;
                return true;
            }

            return false;
        }

        protected abstract void Use();
    }
}