using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace MonoGameProject1.Tests;

public class LeakTests
{
    private class DummyScene : Scene
    {
        public DummyScene(int objectCount)
        {
            var list = new List<GameObject>();
            for (int i = 0; i < objectCount; i++)
            {
                // Create bare game objects with no behaviors to avoid MonoGame dependencies
                list.Add(new GameObject($"GO_{i}"));
            }
            AddGameObjects(list);
        }

        public override void Initialize()
        {
            // No-op
        }
    }

    private static void ForceFullGC()
    {
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    }

    [Fact]
    public void OldScene_Is_GarbageCollected_After_ChangeScene()
    {
        WeakReference sceneRef;
        List<WeakReference> weakGOs;

        void CreateAndSwap()
        {
            var s = new DummyScene(objectCount: 25);
            SceneManager.ChangeScene(s);

            sceneRef = new WeakReference(s);
            var objectRefs = s
                .GetType()
                .GetField("_gameObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(s) as List<GameObject> ?? new List<GameObject>();
            weakGOs = objectRefs.Select(o => new WeakReference(o)).ToList();
            objectRefs = null; // drop strong list reference ASAP

            // Swap to new scene; s goes out of scope after this method returns
            SceneManager.ChangeScene(new DummyScene(objectCount: 10));
        }

        CreateAndSwap();

        // Give GC a few attempts
        for (int i = 0; i < 20; i++)
        {
            ForceFullGC();
            if (!sceneRef.IsAlive && weakGOs.All(w => !w.IsAlive))
                break;
            Thread.Sleep(50);
        }

        // Assert: previous scene and its objects are collectible
        Assert.False(sceneRef.IsAlive);
        Assert.All(weakGOs, wr => Assert.False(wr.IsAlive));
    }

    [Fact]
    public void Repeated_Scene_Changes_Do_Not_Grow_Memory_Significantly()
    {
        // Warmup
        SceneManager.ChangeScene(new DummyScene(5));
        ForceFullGC();
        long baseline = GC.GetTotalMemory(forceFullCollection: true);

        // Perform multiple scene swaps
        for (int i = 0; i < 10; i++)
        {
            SceneManager.ChangeScene(new DummyScene(50));
            SceneManager.ChangeScene(new DummyScene(5));
        }

        ForceFullGC();
        long after = GC.GetTotalMemory(forceFullCollection: true);

        // Allow a generous threshold for allocator noise
        long threshold = 50_000; // 50 KB
        Assert.True(after - baseline < threshold,
            $"Memory grew by {after - baseline} bytes which exceeds threshold {threshold}.");
    }
}
