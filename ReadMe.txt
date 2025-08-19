I checked this implementation of ChatApplication agains ChatGPT for fun. According to ChatGPT this implementation 
  of Cancellation is the best. It can be confusing to use both a bool variable for cancellation like 
  ProgramIsRunning and CancellationToken and it is unnecessary, so stick with one approach and if you allready are
  using a Task using CTS.Cancel() is the cleanest approach anyway, so just use that. And it's easy to use.  

  ChatGPT: 

		  ✅ Does the while (!cTS.IsCancellationRequested) pattern work?
		Yes. This is the standard pattern in .NET for long-running loops that should exit on cancellation.

		✅ Can other parts of the app (like tasks) observe the same token?
		Yes — just pass cTS.Token to all other tasks or methods that accept a CancellationToken, like you're doing already in ListenAsync(ct).

		✅ Is cTS.Dispose() in the right place?
		Yes, you’re disposing the CancellationTokenSource after the loop ends, which is good. Just make sure that:

		All other async tasks using the token have completed before disposing.

		If you started background tasks (like Task.Run(...)), you may want to await them before disposing.

		✅ Your approach promotes clean separation of concerns:
		ConsoleInputProcessor handles input, not sending.

		ChatPeer handles network logic, not input.

		The UI, network, and control flow are decoupled.

		Well done.