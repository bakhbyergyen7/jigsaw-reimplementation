import ast
from graphviz import Digraph
import libcst as cst
import os
os.environ["PATH"] += os.pathsep + 'C:/Program Files/Graphviz/bin'


# Create a Graphviz Digraph object
dot = Digraph()

# Define a function to recursively add nodes to the Digraph
def add_node(node, parent=None):
    node_name = str(node.__class__.__name__)
    dot.node(str(id(node)), node_name)
    if parent:
        dot.edge(str(id(parent)), str(id(node)))
    for child in ast.iter_child_nodes(node):
        add_node(child, node)

incorrect_code = "dfout = df1.append(df2, ignore_index=True)"
correct_code =  "dfout = df1.append(df2)"

tree = ast.parse(incorrect_code)

# Add nodes to the Digraph
add_node(tree)

# Render the Digraph as a PNG file
dot.format = 'png'
dot.render('2_incorrect', view=True)

print(ast.dump(tree))
