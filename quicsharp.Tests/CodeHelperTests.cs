using FluentAssertions;
using NUnit.Framework;
using quicsharp.Engine;
using quicsharp.Engine.LinePreprocessors;
using System;

namespace quicsharp.Tests
{
	public class CodeHelperTests
	{
		public class EnsureSemicolonMethod
		{
			[Test]
			public void Adds_A_Semicolon_If_Required()
			{
				"Code".EnsureSemicolon().Should().Be("Code;");
			}

			[Test]
			public void Does_Not_Add_A_Semicolon_If_One_Is_Present()
			{
				"Code;".EnsureSemicolon().Should().Be("Code;");
			}

			[Test]
			public void Does_Not_Remove_Whitespace_If_Semicolon_Is_Present()
			{
				"Code; ".EnsureSemicolon().Should().Be("Code; ");
			}

			[Test]
			public void Does_Not_Remove_Multiple_Semicolons()
			{
				"Code;;".EnsureSemicolon().Should().Be("Code;;");
			}
		}
	}
}
