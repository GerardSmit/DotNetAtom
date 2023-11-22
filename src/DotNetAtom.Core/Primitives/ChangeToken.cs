using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;

namespace DotNetAtom.Primitives;

public class SimpleChangeToken : IChangeToken
{
	private readonly List<ChangeTokenRegistration> _callbacks = [];

	public bool HasChanged { get; private set; }

	public bool ActiveChangeCallbacks => true;

	public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
	{
		lock (_callbacks)
		{
			var registration = new ChangeTokenRegistration(callback, state);
			_callbacks.Add(registration);
			return new RemoveCallback(_callbacks, registration);
		}
	}

	public void OnChange()
	{
		if (HasChanged)
		{
			throw new InvalidOperationException("Token has already changed.");
		}

		HasChanged = true;

		ChangeTokenRegistration[] callbacks;

		lock (_callbacks)
		{
			callbacks = _callbacks.ToArray();
		}

		foreach (var callback in callbacks)
		{
			callback.Action(callback.State);
		}
	}

	private record struct ChangeTokenRegistration(Action<object?> Action, object? State);

	private class RemoveCallback(List<ChangeTokenRegistration> callbacks, ChangeTokenRegistration registration)
		: IDisposable
	{
		public void Dispose()
		{
			lock (callbacks)
			{
				callbacks.Remove(registration);
			}
		}
	}
}
