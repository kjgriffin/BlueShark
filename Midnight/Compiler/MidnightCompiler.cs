using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Midnight.Compiler.AST;
using Midnight.Compiling.AST;
using Midnight.LanguageDefinitions;
using Midnight.Lexing;
using Midnight.PreProcessor;

namespace Midnight.Compiling
{
    class MidnightCompiler
    {

        Lexer lexer = new Lexer();

        string rawsource = "";

        string postprocsource = "";



        public ASTProgram Compile(string input, bool verboseDebug = false)
        {
            rawsource = input;
            postprocsource = input.StripSingleLineComments("//").StripMultiLineComments("/*", "*/");

            lexer.Tokenize(postprocsource, LanguageDefinition.Seperators);

            ASTProgram program = new ASTProgram();
            try
            {
                ((IASTElement)program).Parse(lexer, null);
            }
            catch (MidnightCompilerParseError ex)
            {
                Debug.WriteLine("Project Failed to Compile!");
                Debug.WriteLine(ex, ex.EMessage);
                return program;
            }

            if (verboseDebug)
            {
                StringBuilder sb = new StringBuilder();
                ((IASTElement)program).GenerateDebugXMLTree(null, sb);
                System.Diagnostics.Debug.Write(sb.ToString());
            }

            return program;
        }



    }
}
