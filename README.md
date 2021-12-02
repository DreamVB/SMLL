<p>Small Machine Little Language.</p>
<p>Small Machine Little Language is a small virtual machine that can run its own assembly language using special instructions that works like a real CPU</p>
<p>I made this project to get a better understanding of how computers and languages work, this project includes it own assembler that breaks down the source file into tokens and then assembles them into a machine-like code. The next part is the interpreter or VM that takes the generated code and executes and preforms action defined by the instructions.</p>
<p>This VM can allows you to preform mathematical operations decision-making using Jump instructions and control flow I included a small set of examples that shows the project in action.</p>
<p>Here is a list of the instructions and their purpose.</p>
<table>
<colgroup>
<col style="width: 14%" />
<col style="width: 12%" />
<col style="width: 73%" />
</colgroup>
<thead>
<tr class="header">
<th><strong>Opcode</strong></th>
<th><strong>Args</strong></th>
<th><strong>Description</strong></th>
</tr>
</thead>
<tbody>
<tr class="odd">
<td>PUSH</td>
<td>1</td>
<td>Push a value on the stack</td>
</tr>
<tr class="even">
<td>POP</td>
<td>0</td>
<td>Remove the stacks top item</td>
</tr>
<tr class="odd">
<td>ADD</td>
<td>0</td>
<td>Add the contents of the two stack items</td>
</tr>
<tr class="even">
<td>SUB</td>
<td>0</td>
<td>Subtract the top two items on the stack</td>
</tr>
<tr class="odd">
<td>MUL</td>
<td>0</td>
<td>Multiply the top two stack items</td>
</tr>
<tr class="even">
<td>DIV</td>
<td>0</td>
<td>Divide the top two stack items</td>
</tr>
<tr class="odd">
<td>AND</td>
<td>0</td>
<td>Bitwise and the two top stack items</td>
</tr>
<tr class="even">
<td>OR</td>
<td>0</td>
<td>Bitwise or the top two stack items</td>
</tr>
<tr class="odd">
<td>XOR</td>
<td>0</td>
<td>XOR the top two stack items</td>
</tr>
<tr class="even">
<td>MOD</td>
<td>0</td>
<td>Push the remainder of the top two stack items</td>
</tr>
<tr class="odd">
<td>NOT</td>
<td>0</td>
<td>Invert the stacks top item</td>
</tr>
<tr class="even">
<td>LT</td>
<td>0</td>
<td>Compare the two top stack items for less then</td>
</tr>
<tr class="odd">
<td>LTE</td>
<td>0</td>
<td>Compare the two top stack items for less than equal</td>
</tr>
<tr class="even">
<td>GT</td>
<td>0</td>
<td>Compare the two top stack items for greater than</td>
</tr>
<tr class="odd">
<td>GTE</td>
<td>0</td>
<td>Compare the two top stack items for greater than equal</td>
</tr>
<tr class="even">
<td>EQ</td>
<td>0</td>
<td>Compares of the top two stack items are equal</td>
</tr>
<tr class="odd">
<td>DUP</td>
<td>0</td>
<td>Duplicate the last top stack item</td>
</tr>
<tr class="even">
<td>INT</td>
<td>0</td>
<td>Converts the top stacks item to integer</td>
</tr>
<tr class="odd">
<td>STORE</td>
<td>1</td>
<td>Stores the top stacks item into the memory address</td>
</tr>
<tr class="even">
<td>LOAD</td>
<td>1</td>
<td>Loads contents of the memory address to the top of the stack</td>
</tr>
<tr class="odd">
<td>JMP</td>
<td>1</td>
<td>Jumps to a location in the code using a label name</td>
</tr>
<tr class="even">
<td>JIF</td>
<td>1</td>
<td>Jumps to a label location if the top stack value is equal to 1</td>
</tr>
<tr class="odd">
<td>JIFZ</td>
<td>1</td>
<td>Jumps to a label location if the top value of the stack is 0</td>
</tr>
<tr class="even">
<td>PRTD</td>
<td>0</td>
<td>Output the top of the stack to a double</td>
</tr>
<tr class="odd">
<td>PRTI</td>
<td>0</td>
<td>Output the top of the stack to an integer</td>
</tr>
<tr class="even">
<td>PRTC</td>
<td>0</td>
<td>Output the top of the stack to a char</td>
</tr>
<tr class="odd">
<td>NEG</td>
<td>0</td>
<td>Flip the top of the stack between positive and negative</td>
</tr>
<tr class="even">
<td>NOP</td>
<td>0</td>
<td>It does nothing</td>
</tr>
<tr class="odd">
<td>INC</td>
<td>0</td>
<td>Increment the top of the stack value by 1</td>
</tr>
<tr class="even">
<td>DEC</td>
<td>0</td>
<td>Decrement the top of the stack value by 1</td>
</tr>
<tr class="odd">
<td>INP</td>
<td>0</td>
<td>Allow the user to input from the keyboard</td>
</tr>
</tbody>
</table>

<P>Count down example</p>

```
; Count up to 20

PUSH 20
PUSH 0

STORE A
STORE B

:loop
 LOAD A
 LOAD B
 LT
 JIF Add
 JMP Finish

:Add
 LOAD A
 INC
 STORE A
 LOAD A
 PRTD
 PUSH ','
 PRTC
 JMP Loop

:Finish
halt
```
