namespace Demo
{
    public class RealtimeAbility : Ability
    {
        public float TimeLeftTillUse => _countDown;
        DateTime _lastUse;
        public RealtimeAbility(int cooldown) : base(cooldown)
        {
            _lastUse = DateTime.Now;
        }

        public override bool TryUse()
        {
            _countDown = (float)Cooldown - (float)(DateTime.Now - _lastUse).TotalSeconds;
            var ret = base.TryUse();
            if (ret)
            {
                _lastUse = DateTime.Now;
            }

            return ret;
        }

        protected override void Use()
        {
            Console.WriteLine("A Fireball was shot!");
        }
    }
}