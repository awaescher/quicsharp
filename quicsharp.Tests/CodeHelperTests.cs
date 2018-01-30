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

			public class RemoveSemicolonMethod
			{
				[Test]
				public void Does_Not_Change_Code_Without_Semicolon()
				{
					"Code".RemoveSemicolon().Should().Be("Code");
				}

				[Test]
				public void Removes_Semicolon_At_The_End_Of_The_Line()
				{
					"Code;".RemoveSemicolon().Should().Be("Code");
				}

				[Test]
				public void Removes_Whitespace_At_The_End_Of_The_Line()
				{
					"Code ".RemoveSemicolon().Should().Be("Code");
				}

				[Test]
				public void Removes_Semicolon_And_Whitespace_At_The_End_Of_The_Line()
				{
					"Code; ".RemoveSemicolon().Should().Be("Code");
				}

				[Test]
				public void Does_Not_Remove_Whitespace_At_The_Beginning_Of_The_Line()
				{
					" Co de".RemoveSemicolon().Should().Be(" Co de");
				}

				[Test]
				public void Removes_Multiple_Semicolons()
				{
					"Code;;".RemoveSemicolon().Should().Be("Code");
				}

				[Test]
				public void Does_Only_Remove_Semicolons_At_The_End_Of_The_Line()
				{
					";Co;de;".RemoveSemicolon().Should().Be(";Co;de");
				}
			}
		}
	}
}
