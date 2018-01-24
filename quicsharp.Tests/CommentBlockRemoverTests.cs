using FluentAssertions;
using NUnit.Framework;
using quicsharp.Engine.LinePreprocessors;
using System;

namespace quicsharp.Tests
{
	public class CommentBlockRemoverTests
	{
		public class ProcessMethod
		{
			private CommentRemover _remover = new CommentRemover();

			[Test]
			public void Does_Not_Remove_Empty_Lines()
			{
				var lines = @"

/* COMMENT */

".ToLines();

				_remover.Process(ref lines);

				lines.Length.Should().Be(5);
			}

			[Test]
			public void Does_Not_Remove_CommentOnly_Lines()
			{
				var lines = @"
/**/
".ToLines();

				_remover.Process(ref lines);

				lines.Length.Should().Be(3);
			}

			[Test]
			public void Removes_Empty_Comments()
			{
				var lines = @"
/**/
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("");
			}

			[Test]
			public void Removes_Comments()
			{
				var lines = @"
/* COMMENT */
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("");
			}

			[Test]
			public void Does_Not_Remove_Code_Before_And_After_Comments()
			{
				var lines = @"
CO/* COMMENT */DE
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE");
			}

			[Test]
			public void Does_Not_Remove_Code_Lines_Before_And_After_Comments()
			{
				var lines = @"
CODE1
CO/* COMMENT */DE2
CODE3
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CODE2");
				lines[3].Should().Be("CODE3");
			}

			[Test]
			public void Does_Not_Remove_Code_Lines_Before_And_After_Comments_But_Respects_Whitespace()
			{
				var lines = @"
CODE1
CO  /*COMMENT*/  DE2
CODE3
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CO    DE2");
				lines[3].Should().Be("CODE3");
			}

			[Test]
			public void Removes_Multiline_Comments()
			{
				var lines = @"
CODE1
/*
COMMENT
*/
CODE2
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be(""); // was /*
				lines[3].Should().Be(""); // was COMMENT
				lines[4].Should().Be(""); // was */
				lines[5].Should().Be("CODE2");
			}

			[Test]
			public void Removes_Multiline_Comments_Within_Code_Lines()
			{
				var lines = @"
CODE1
CODE2 /*
COMMENT
*/ CODE3
CODE4
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CODE2 ");
				lines[3].Should().Be(""); // was COMMENT
				lines[4].Should().Be(" CODE3");
				lines[5].Should().Be("CODE4");
			}

			[Test]
			public void Removes_Multiline_Comments_Within_Code_Lines_With_Inline_Comments()
			{
				var lines = @"
CODE1
CO/* COMMENT */DE2 /*
COMMENT
*/ CODE3
CODE4
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CODE2 "); // had inline comment
				lines[3].Should().Be(""); // was COMMENT
				lines[4].Should().Be(" CODE3");
				lines[5].Should().Be("CODE4");
			}

			[Test]
			public void Ingnores_Nested_Comments()
			{
				var lines = @"
CODE1
CODE2 /* COMMENT1
COMMENT2
/* COMMENT3 */
COMMENT4
COMMENT5 */ CODE3
CODE4
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CODE2 "); // had following COMMENT1
				lines[3].Should().Be(""); // was COMMENT2
				lines[4].Should().Be(""); // was inline comment COMMENT3
				lines[5].Should().Be(""); // was COMMENT4
				lines[6].Should().Be(" CODE3"); // had preceding COMMENT5
				lines[7].Should().Be("CODE4");
			}

			[Test]
			public void Removes_Doubleslash_Comments()
			{
				var lines = @"
CODE1
//COMMENT
CODE3
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("");
				lines[3].Should().Be("CODE3");
			}

			[Test]
			public void Does_Not_Remove_Code_Before_Doubleslash_Comments()
			{
				var lines = @"
CODE1
CODE2 //COMMENT
CODE3
".ToLines();

				_remover.Process(ref lines);

				lines[1].Should().Be("CODE1");
				lines[2].Should().Be("CODE2 ");
				lines[3].Should().Be("CODE3");
			}
		}
	}
}
