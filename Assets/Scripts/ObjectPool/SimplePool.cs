///
/// Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///   UPDATES:
/// 	2015-04-16: Changed Pool to use a Stack generic.
/// 
/// Usage:
/// 
///   There's no need to do any special setup of any kind.
/// 
///   Instead of call Instantiate(), use this:
///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
/// 
///   Instead of destroying an object, use this:
///       SimplePool.Despawn(myGameObject);
/// 
///   If desired, you can preload the pool with a number of instances:
///       SimplePool.Preload(somePrefab, 20);
/// 
/// Remember that Awake and Start will only ever be called on the first instantiation
/// and that member variables won't be reset automatically.  You should reset your
/// object yourself after calling Spawn().  (i.e. You'll have to do things like set
/// the object's HPs to max, reset animation states, etc...)
/// 
/// 
/// 


using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{

    // You can avoid resizing of the Stack's internal array by
    // setting this to a number equal to or greater to what you
    // expect most of your pool sizes to be.
    // Note, you can also use Preload() to set the initial size
    // of a pool -- this can be handy if only some of your pools
    // are going to be exceptionally large (for example, your bullets.)
    const int DEFAULT_POOL_SIZE = 3;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    internal class Pool
    {
        // We append an id to the name of anything we instantiate.
        // This is purely cosmetic.
        int nextId = 1;

        // The structure containing our inactive objects.
        // Why a Stack and not a List? Because we'll never need to
        // pluck an object from the start or middle of the array.
        // We'll always just grab the last one, which eliminates
        // any need to shuffle the objects around in memory.
        Stack<Reusable> inactive;

        // The prefab that we are pooling
        Reusable prefab;
        // Cached activeSelf
        bool activeSelf;

        // Constructor
        public Pool(Reusable prefab, int initialQty)
        {
            this.prefab = prefab;

            // If Stack uses a linked list internally, then this
            // whole initialQty thing is a placebo that we could
            // strip out for more minimal code.
            inactive = new Stack<Reusable>(initialQty);
            activeSelf = prefab.gameObject.activeSelf;
        }

        public void Preload(int quantity)
        {
            // HACK : 프리펩 자체를 비활성화하고 인스턴스화한 후 복구
            // 이렇게하면 비활성화된 상태로 생성되어 OnEnable/OnDisable이 발생하지 않음
            prefab.gameObject.SetActive(false);
            {
                for (int i = inactive.Count; i < quantity; i++)
                {
                    // We don't have an object in our pool, so we
                    // instantiate a whole new object.
                    Reusable reusable = UnityEngine.Object.Instantiate(prefab);
                    reusable.gameObject.name = prefab.name + " (" + (nextId++) + ")";

                    // Add a PoolMember component so we know what pool
                    // we belong to.
                    reusable.pool = this;

                    inactive.Push(reusable);
                }
            }
            prefab.gameObject.SetActive(activeSelf);
        }

        // Spawn an object from our pool
        public Reusable Spawn()
        {
            Reusable reusable;

            if (inactive.Count == 0)
            {
                // We don't have an object in our pool, so we
                // instantiate a whole new object.
                reusable = Object.Instantiate(prefab);
                reusable.name = prefab.name + " (" + (nextId++) + ")";
                reusable.pool = this;
            }
            else
            {
                // Grab the last object in the inactive array
                reusable = inactive.Pop();

                if (reusable == null)
                {
                    // The inactive object we expected to find no longer exists.
                    // The most likely causes are:
                    //   - Someone calling Destroy() on our object
                    //   - A scene change (which will destroy all our objects).
                    //     NOTE: This could be prevented with a DontDestroyOnLoad
                    //	   if you really don't want this.
                    // No worries -- we'll just try the next one in our sequence.

                    return Spawn();
                }
            }

            return reusable;
        }

        // Spawn an object from our pool
        public Reusable Spawn(Vector3 pos)
        {
            Reusable reusable = Spawn();
            GameObject obj = reusable.gameObject;

            obj.transform.position = pos;
            obj.SetActive(activeSelf);
            return reusable;
        }

        // Spawn an object from our pool
        public Reusable Spawn(Vector3 pos, Quaternion rot)
        {
            Reusable reusable = Spawn();
            GameObject obj = reusable.gameObject;

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(activeSelf);
            return reusable;
        }

        // Return an object to the inactive pool.
        public void Despawn(Reusable reusable)
        {
            // Since Stack doesn't have a Capacity member, we can't control
            // the growth factor if it does have to expand an internal array.
            // On the other hand, it might simply be using a linked list 
            // internally.  But then, why does it allow us to specificy a size
            // in the constructor? Stack is weird.
            inactive.Push(reusable);
        }

        public void Clear()
        {
            while (inactive.Count > 0)
                UnityEngine.Object.Destroy(inactive.Pop().gameObject);
        }
    }

    // All of our pools
    static readonly Dictionary<Reusable, Pool> pools = new Dictionary<Reusable, Pool>();

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    static void Init(Reusable prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 10+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    /// </summary>
    static public void Preload(Reusable prefab, int qty = 1)
    {
        Init(prefab, qty);

        pools[prefab].Preload(qty);
    }

    static public void Clear(Reusable prefab)
    {
        pools[prefab].Clear();
    }
    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    static public Reusable Spawn(Reusable prefab)
    {
        Init(prefab);

        var reusable = pools[prefab].Spawn();
        reusable.Initailize();

        return reusable;
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    static public Reusable Spawn(Reusable prefab, Vector3 pos)
    {
        Init(prefab);
        var reusable = pools[prefab].Spawn(pos);
        reusable.Initailize();

        return reusable;
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    static public Reusable Spawn(Reusable prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);

        var reusable = pools[prefab].Spawn(pos, rot);
        reusable.Initailize();

        return reusable;
    }

    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// </summary>
    static public void Despawn(Reusable obj)
    {
        //if (pm == null)
        //{
        //    Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
        //    GameObject.Destroy(obj);
        //}
        //else
        //{
        obj.Hibernate();
        obj.gameObject.SetActive(false);
        obj.pool.Despawn(obj);
        //}
    }
}