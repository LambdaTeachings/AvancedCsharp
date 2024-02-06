//

namespace TileEgnine
{
    // internal means we don't want client code to reach this class
    internal class Engine
    {
        #region Singleton

        private static Engine _instance;
        public static Engine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Engine();

                return _instance;
            }
        }

        private Engine() { }

        #endregion

        Scene _context = null;

        // runs the scene
        public void Run(Scene scene)
        {
            _context = scene;
            Physics.Tick = 0.016f;
            Task.Run(RunContext);
        }

        public void Stop()
        {
            _context = null;
        }

        private void RunContext()
        {
            while (_context != null)
            {
                // 60 fps
                Thread.Sleep((int)(Physics.Tick * 1000));
                Console.Clear();

                foreach (var entity in _context.Root)
                {
                    TickEntity(entity);
                }

                Physics.ResolveCollisions();
            }
        }

        private void TickEntity(Entity entity)
        {
            foreach (var component in entity.Components)
            {
                component.Update();
                if (component is RigidBody rb)
                    Physics.CacheRigidbody(rb);

                foreach (var child in entity.Children)
                {
                    TickEntity(child);
                }
            }
        }
    }

    public class Scene
    {
        public string Name { get; set; }
        // should use readonly lists but it doesn't matter
        // since this is a demo
        public List<Entity> Root = new List<Entity>(16);

        public Scene(string name)
        {
            Name = name;
        }

        public void AddEntity(Entity entity) => Root.Add(entity);

        public void Play() => Engine.Instance.Run(this);
        public void Stop() => Engine.Instance.Stop();
    }

    public class Entity
    {
        // this should be lazy
        // client code shouldn't really access components list directly
        public List<Entity> Children = new List<Entity>(0);
        public List<Component> Components = new List<Component>(2);

        public string Name { get; set; }
        public char Renderer { get; set; }

        public Transform Transform => Components[0] as Transform;

        public Entity()
        {
            Components.Add(new Transform(this));
        }

        public Entity(string name) : this()
        {
            Name = name;
        }

        public Entity(string name, char renderer) : this(name)
        {
            Name = name;
            Renderer = renderer;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }
    }

    public abstract class Component
    {
        public Entity Entity { get; }
        public Transform Transform => Entity.Transform;
        public Component(Entity entity)
        {
            Entity = entity;
        }

        public virtual void Update()
        {
            Console.SetCursorPosition((int)Transform.Position.X, -(int)Transform.Position.Y);
            Console.Write(Entity.Renderer);
        }
    }

    public class Transform : Component
    {
        internal Transform(Entity entity) : base(entity) { }

        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public Vector2 Size { get; set; }

        public override string ToString()
        {
            return Position.ToString();
        }
    }

    public class RigidBody : Component
    {
        public RigidBody(Entity entity) : base(entity) { }

        public bool UseGravity { get; set; } = true;

        public override void Update()
        {
            base.Update();

            // obviously this is not how physics work
            // obviously I don't give a fuck
            if (UseGravity)
            {
                Transform.Position =
                    new Vector2(Transform.Position.X,
                    Transform.Position.Y - Physics.Gravity * Physics.Tick);
            }
        }
    }

    public static class Physics
    {
        public static float Gravity { get; set; } = 9.8f;
        public static float Tick { get; internal set; }

        static List<RigidBody> _rbs = new List<RigidBody>(16);

        internal static void CacheRigidbody(RigidBody rb)
        {
            _rbs.Add(rb);
        }

        // extremely brute force (n^2)
        // but that's not the point
        public static void ResolveCollisions()
        {
            for (int i = 0; i < _rbs.Count-1; i++)
            {
                for (int j = i + 1; j < _rbs.Count; j++)
                {
                    if (_rbs[i].Transform.Position.DistanceFrom(_rbs[j].Transform.Position) < 1)
                    {
                        // not how collision works but...
                        // proves the point
                        _rbs[i].UseGravity = false;
                        _rbs[j].UseGravity = false;
                    }
                }
            }

            _rbs.Clear();
        }
    }

    public struct Vector2
    {
        public readonly float X;
        public readonly float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float DistanceFrom(Vector2 other)
        {
            var x = MathF.Pow(X - other.X, 2);
            var y = MathF.Pow(Y - other.Y, 2);

            return MathF.Pow(x + y, 0.5f);
        }

        public override string ToString()
        {
            return $"({X:n},{Y:n})";
        }
    }
}
