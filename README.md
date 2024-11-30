# jigsaw-reimplementation

## Test Case 1:
### Incorrect
![1_incorrect](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/1_incorrect.png)
<ul>
    <li>Module(body=[Assign(...)], )</li>
    <li>Assign(out, Module/Assign)</li>
    <li>Subscript(df, Module/Assign/Subscript)</li>
    <li>Attribute(iloc, Module/Assign/Subscript/Attribute)</li>
    <li>Tuple((0, 'HP'), Module/Assign/Subscript/Tuple)</li>
    <li>Constant(0, Module/Assign/Subscript/Tuple/Constant[0])</li>
    <li>Constant(HP, Module/Assign/Subscript/Tuple/Constant[1])</li>
</ul>

### Correct
![1_correct](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/1_correct.png)
<ul>
    <li>Module(body=[Assign(...)], )</li>
    <li>Assign(out, Module/Assign)</li>
    <li>Subscript(df, Module/Assign/Subscript)</li>
    <li>Attribute(loc, Module/Assign/Subscript/Attribute)</li>
    <li>Tuple((0, 'HP'), Module/Assign/Subscript/Tuple)</li>
    <li>Constant(0, Module/Assign/Subscript/Tuple/Constant[0])</li>
    <li>Constant(HP, Module/Assign/Subscript/Tuple/Constant[1])</li>
</ul>

## Test Case 2:
### Incorrect
![2_incorrect](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/2_incorrect.png)
<ul>
    <li>Module(body=[Assign(...)], )</li>
    <li>Assign(dfout, Module/Assign)</li>
    <li>Call(df1, Module/Assign/Call)</li>
    <li>Attribute(append, Module/Assign/Call/Attribute)</li>
    <li>Name(df2, Module/Assign/Call/Name)</li>
    <li>keyword(ignore_index, Module/Assign/Call/keyword)</li>
    <li>Constant(True, Module/Assign/Call/keyword/Constant[0])</li>
</ul>
### Correct
![2_correct](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/2_correct.png)
<ul>
    <li>Module(body=[Assign(...)], )</li>
    <li>Assign(dfout, Module/Assign)</li>
    <li>Call(df1, Module/Assign/Call)</li>
    <li>Attribute(append, Module/Assign/Call/Attribute)</li>
    <li>Name(df2, Module/Assign/Call/Name)</li>
</ul>
