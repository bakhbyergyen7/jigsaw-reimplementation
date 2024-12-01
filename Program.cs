using ASTtransformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            References = CompilerReference.FromAssemblyFiles(typeof(Program).GetTypeInfo().Assembly)
        }).Value;

        private static SynthesisEngine _prose;

        private static readonly Dictionary<State, object> Examples = new Dictionary<State, object>();
        private static ProgramNode _topProgram;

        static void Main(string[] args)
        {
            _prose = ConfigureSynthesis();
            // Test the RemoveKind operation
            var child1 = new MyAST("Kind1", "Value1", "Path1", new List<MyAST>());
            var root1 = new MyAST("Root", "Value", "Path", new List<MyAST>{child1});
            var root2 = new MyAST("Root", "Value", "Path", new List<MyAST>());

            // do synthesis
            SynthesizeProgram(root1, root2);

            // Console.WriteLine("\nThis is the input AST:");
            // PrintAST(root1);

            // // This is the AST transformation required to fix the input AST.
            // // I want to use PROSE to learn this transformation.
            // var result = Semantics.RemoveKind(root1, "Kind1");

            // Console.WriteLine("\nThis is the AST of the input AST after applying the RemoveKind transformation:");
            // PrintAST(result);

            // // This is the correct output AST
            // Console.WriteLine("\nThis is the correct output AST:");
            // PrintAST(root2);

            // Example #2: Test the UpdateKind operation
            var root3 = new MyAST("root", "Value_root3", "Path1", new List<MyAST>());
            var root4 = new MyAST("root", "Value_root4", "Path1", new List<MyAST>());
            // Console.WriteLine("\nThis is the input AST:");
            // PrintAST(root3);

            // do synthesis
            SynthesizeProgram(root3, root4);

            // // This is the AST transformation required to fix the input AST.
            // // I want to use PROSE to learn this transformation.
            // var result2 = Semantics.UpdateKind(root3, "root", "Value_root4");

            // Console.WriteLine("\nThis is the AST of the input AST after applying the UpdateKind transformation:");
            // PrintAST(result2);

            // // This is the correct output AST
            // Console.WriteLine("\nThis is the correct output AST:");
            // PrintAST(root4);

            // Example #3: Hard example
            var constantTrue = new MyAST("Constant", "True", "Module/Assign/Call/keyword/Constant[0]", new List<MyAST>());
            var keyword = new MyAST("keyword", "ignore_index", "Module/Assign/Call/keyword", new List<MyAST> { constantTrue });
            var name = new MyAST("Name", "df2", "Module/Assign/Call/Name", new List<MyAST>());
            var attribute = new MyAST("Attribute", "append", "Module/Assign/Call/Attribute", new List<MyAST>());
            var call = new MyAST("Call", "df1", "Module/Assign/Call", new List<MyAST> { name, attribute, keyword });
            var assign = new MyAST("Assign", "dfout", "Module/Assign", new List<MyAST> { call });
            var in_module = new MyAST("Module", "body=[Assign(...)]", "", new List<MyAST> { assign });


            // This is the correct output AST
            var name2 = new MyAST("Name", "df2", "Module/Assign/Call/Name", new List<MyAST>());
            var attribute2 = new MyAST("Attribute", "append", "Module/Assign/Call/Attribute", new List<MyAST>());
            var call2 = new MyAST("Call", "df1", "Module/Assign/Call", new List<MyAST> { name2, attribute2 });
            var assign2 = new MyAST("Assign", "dfout", "Module/Assign", new List<MyAST> { call2 });
            var out_module = new MyAST("Module", "body=[Assign(...)]", "", new List<MyAST> { assign2 });

            // do synthesis
            SynthesizeProgram(in_module, out_module);

            // Console.WriteLine("\nThis is the input AST:");
            // PrintAST(in_module);

            // // This is the AST transformation required to fix the input AST.
            // // I want to use PROSE to learn this transformation.
            // var result3 = Semantics.RemoveKind(in_module, "keyword");

            // Console.WriteLine("\nThis is the AST of the input AST after applying the RemoveKind transformation:");
            // PrintAST(result3);

            // // This is the correct output AST
            // Console.WriteLine("\nThis is the correct output AST:");
            // PrintAST(out_module);


        }
        // Function to print AST structure recursively
        static void PrintAST(MyAST ast)
        {
            Console.WriteLine(ast);
        }
        public static SynthesisEngine ConfigureSynthesis()
        {
            var witnessFunctions = new WitnessFunctions(Grammar);
            var deductiveSynthesis = new DeductiveSynthesis(witnessFunctions);
            var synthesisExtrategies = new ISynthesisStrategy[] {deductiveSynthesis};
            var synthesisConfig = new SynthesisEngine.Config {Strategies = synthesisExtrategies};
            var prose = new SynthesisEngine(Grammar, synthesisConfig);
            return prose;
        }
        // synthesis wrapper given the input AST and the correct output AST
        public static ProgramNode SynthesizeProgram(MyAST input, MyAST output)
        {
            Examples.Clear();
            State inputState = State.CreateForExecution(Grammar.InputSymbol, input);
            Examples.Add(inputState, output);

            var spec = new ExampleSpec(Examples);
            Console.Out.WriteLine("Learning a program for examples:");
            foreach (KeyValuePair<State, object> example in Examples)
                Console.WriteLine("\"{0}\" -> \"{1}\"", example.Key.Bindings.First().Value, example.Value);

            var scoreFeature = new RankingScore(Grammar);
            ProgramSet topPrograms = _prose.LearnGrammarTopK(spec, scoreFeature, 4, null);
            if (topPrograms.IsEmpty) throw new Exception("No program was found for this specification.");

            _topProgram = topPrograms.RealizedPrograms.First();
            Console.Out.WriteLine("Top 4 learned programs:");
            var counter = 1;
            foreach (ProgramNode program in topPrograms.RealizedPrograms)
            {
                if (counter > 4) break;
                Console.Out.WriteLine("==========================");
                Console.Out.WriteLine("Program {0}: ", counter);
                Console.Out.WriteLine(program.PrintAST(ASTSerializationFormat.HumanReadable));
                counter++;
            }
            return _topProgram;
        }

    }
}