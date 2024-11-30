using System;
using System.Collections.Generic;
using ASTtransformation;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.VersionSpace;

namespace TestSemantics
{
    class Program
    {
        private static readonly Grammar Grammar = DSLCompiler.Compile(new CompilerOptions
        {
            InputGrammarText = File.ReadAllText("synthesis/grammar/ASTtransform.grammar"),
            References = CompilerReference.FromAssemblyFiles(typeof(Program).Assembly)
        }).Value;

        private static SynthesisEngine _prose;
        private static readonly Dictionary<State, object> Examples = new Dictionary<State, object>();
        private static ProgramNode _topProgram;

        static void Main(string[] args)
        {
            // Test fixing
            // I want this to be my input AST
            var constantTrue = new AST("Constant", "True", "Module/Assign/Call/keyword/Constant[0]", new List<AST>());
            var keyword = new AST("keyword", "ignore_index", "Module/Assign/Call/keyword", new List<AST> { constantTrue });
            var name = new AST("Name", "df2", "Module/Assign/Call/Name", new List<AST>());
            var attribute = new AST("Attribute", "append", "Module/Assign/Call/Attribute", new List<AST>());
            var call = new AST("Call", "df1", "Module/Assign/Call", new List<AST> { name, attribute, keyword });
            var assign = new AST("Assign", "dfout", "Module/Assign", new List<AST> { call });
            var module = new AST("Module", "body=[Assign(...)]", "", new List<AST> { assign });

            // // This is the AST transformation required to fix the input AST. 
            // // I want to use PROSE to learn this transformation.
            // var matchedNodes = Semantics.Match(module, ast => Semantics.Kind(ast, "keyword"));
            // Semantics.Remove(module, matchedNodes);

            // Console.WriteLine("\nThis is the AST of the=third code snippet after applying the Remove transformation:");
            // PrintAST(module);

            // This is the correct output AST
            var name2 = new AST("Name", "df2", "Module/Assign/Call/Name", new List<AST>());
            var attribute2 = new AST("Attribute", "append", "Module/Assign/Call/Attribute", new List<AST>());
            var call2 = new AST("Call", "df1", "Module/Assign/Call", new List<AST> { name2, attribute2 });
            var assign2 = new AST("Assign", "dfout", "Module/Assign", new List<AST> { call2 });
            var module2 = new AST("Module", "body=[Assign(...)]", "", new List<AST> { assign2 });

            // Console.WriteLine("\nThis is the correct output AST:");
            // PrintAST(module2);

            // Define my input-output examples
            var input = module;
            var output = module2;
            Console.WriteLine("\nThis is the input AST:");  
            PrintAST(input);
            Console.WriteLine("\nThis is the output AST:");
            PrintAST(output);
            
            // Create a state with the input AST
            State inputState = State.CreateForExecution(Grammar.InputSymbol, input);
            // Add the input-output examples to the dictionary
            Examples.Add(inputState, output);



        }
        // Function to print AST structure recursively
        static void PrintAST(AST ast)
        {
            Console.WriteLine(ast.ToString());
            foreach (var child in ast.Children)
            {
                PrintAST(child);
            }
        }
    }
}
