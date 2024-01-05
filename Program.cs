using Demo;


// ---- Ability Demo

var fireball = new RealtimeAbility(3);

while (true)
{
    Console.ReadLine();
    if (fireball.TryUse())
    {
        Console.WriteLine("You used a fireball!");
    }
    else
    {
        Console.WriteLine($"Fireball still can't be used, still has {fireball.TimeLeftTillUse:0.0} seconds to go.");
    }
}

// -----