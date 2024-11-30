using System;
using System.Collections.Generic;

namespace ASTtransformation
{
    public class AST
    {
        public string Kind { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public List<AST> Children { get; }

        public AST(string kind, string value, string path, IEnumerable<AST> children)
        {
            Kind = kind;
            Value = value;
            Path = path;
            Children = new List<AST>(children);
        }

        public override string ToString()
        {
            return $"{Kind}({Value}, {Path})";
        }
    }

    
    public static class Semantics
    {
        // Check if the kind of the AST node is equal to the given kind
        public static bool Kind(AST ast, string kind)
        {
            return ast.Kind == kind;
        }

        // Check if the value of the AST node is equal to the given value
        public static bool Value(AST ast, string value)
        {
            return ast.Value == value;
        }

        // Match AST nodes based on a predicate
        public static List<AST> Match(AST ast, Func<AST, bool> predicate)
        {
            var matchedNodes = new List<AST>();
            Match(ast, predicate, matchedNodes);
            return matchedNodes;
        }

        private static void Match(AST ast, Func<AST, bool> predicate, List<AST> matchedNodes)
        {
            if (predicate(ast))
            {
                matchedNodes.Add(ast);
            }

            foreach (var child in ast.Children)
            {
                Match(child, predicate, matchedNodes);
            }
        }

        // AST TRANSFORMATION OPERATIONS
        // 1. Replace the value of the AST node with the given value
        public static void Replace(List<AST> astNodes, string newValue)
        {
            foreach (var ast in astNodes)
            {
                ast.Value = newValue;
            }
        }
        // 2. Remove the AST node from the tree, along with its children
        public static void Remove(AST root, List<AST> nodesToDelete)
        {
            foreach (var node in nodesToDelete)
            {
                RemoveNode(root, node);
            }
        }

        private static void RemoveNode(AST current, AST target)
        {
            if (current.Children.Contains(target))
            {
                // Remove the target node from its parent's children
                current.Children.Remove(target);
                return;
            }

            // Recursively search for the target node in the subtree
            foreach (var child in current.Children)
            {
                RemoveNode(child, target);
            }
        }

    }
}
