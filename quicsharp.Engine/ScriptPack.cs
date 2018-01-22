using ScriptCs.Contracts;

namespace quicsharp.Engine
{
	internal class ScriptPack : IScriptPack
	{
		private ScriptPackContext _context;

		public ScriptPack()
		{
			_context = new ScriptPackContext();
		}

		public IScriptPackContext GetContext() => _context;

		public void Initialize(IScriptPackSession session)
		{
		}

		public void Terminate()
		{
		}
	}
}