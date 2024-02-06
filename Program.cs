using TileEgnine;

Scene main = new Scene("main");
// player
var player = new Entity("Physical Object", '@');
player.AddComponent(new RigidBody(player));
main.AddEntity(player);

// floor
var wall = new Entity("Wall", 'X');
wall.Transform.Position = new Vector2(0, -10);
// This is why we need factories
// there's double initilization here, see?

// 1
var rb = new RigidBody(wall);
// 2
wall.AddComponent(rb);
rb.UseGravity = false;
main.AddEntity(wall);

main.Play();

while (Console.ReadKey().Key != ConsoleKey.E) ;

main.Stop();
