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
    - Module(body=[Assign(...)], )
    - Assign(out, Module/Assign)
    - Subscript(df, Module/Assign/Subscript)
    - Attribute(loc, Module/Assign/Subscript/Attribute)
    - Tuple((0, 'HP'), Module/Assign/Subscript/Tuple)
    - Constant(0, Module/Assign/Subscript/Tuple/Constant[0])
    - Constant(HP, Module/Assign/Subscript/Tuple/Constant[1])

## Test Case 2:
### Incorrect
![2_incorrect](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/2_incorrect.png)
    - Module(body=[Assign(...)], )
    - Assign(dfout, Module/Assign)
    - Call(df1, Module/Assign/Call)
    - Attribute(append, Module/Assign/Call/Attribute)
    - Name(df2, Module/Assign/Call/Name)
    - keyword(ignore_index, Module/Assign/Call/keyword)
    - Constant(True, Module/Assign/Call/keyword/Constant[0])
### Correct
![2_correct](https://github.com/bakhbyergyen7/jigsaw-reimplementation/blob/main/test_cases/2_correct.png)
    - Module(body=[Assign(...)], )
    - Assign(dfout, Module/Assign)
    - Call(df1, Module/Assign/Call)
    - Attribute(append, Module/Assign/Call/Attribute)
    - Name(df2, Module/Assign/Call/Name)
