using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SMLL
{
    class CPU
    {
        //Variable frame class
        class Frame
        {
            private Hashtable variables = new Hashtable();

            public void Setvariable(int index, double value){
                if (!variables.ContainsKey(index))
                {
                    variables.Add(index, value);
                }
                else
                {
                    variables[index] = value;
                }
            }


            public double GetVariable(int index)
            {
                if (variables.ContainsKey(index))
                {
                    return Convert.ToDouble(variables[index]);
                }
                return 0;
            }
        }

        //CPU STACK
        private Stack<double> stack = new Stack<double>();
        private Frame CurrentFrame = new Frame();
        private double[] program = { 0 };
        private bool IsRunning { get; set; }
        private int it { get; set; }
        
        /// <summary>
        /// START OF COMPILER
        /// This takes a input does the following:
        /// 
        /// Strip any comments and blank lines
        /// Create a list of tokens
        /// Set up labels addresses
        /// Convert collected list of tokens into machine code for the Virtual Machine
        /// 
        /// </summary>
        private Hashtable Lables = new Hashtable();

        public int GetOpCodeFromToken(string cmd)
        {
            string Command = cmd.ToUpper();

            if (Command.Equals("NOP"))
            {
                return Instructions.NOP;
            }
            else if (Command.Equals("PUSH"))
            {
                return Instructions.PUSH;
            }
            else if (Command.Equals("POP"))
            {
                return Instructions.POP;
            }
            else if (Command.Equals("ADD"))
            {
                return Instructions.ADD;
            }
            else if (Command.Equals("SUB"))
            {
                return Instructions.SUB;
            }
            else if (Command.Equals("MUL"))
            {
                return Instructions.MUL;
            }
            else if (Command.Equals("DIV"))
            {
                return Instructions.DIV;
            }
            else if (Command.Equals("AND"))
            {
                return Instructions.ADD;
            }
            else if (Command.Equals("OR"))
            {
                return Instructions.OR;
            }
            else if (Command.Equals("XOR"))
            {
                return Instructions.XOR;
            }
            else if (Command.Equals("NOT"))
            {
                return Instructions.NOT;
            }
            else if (Command.Equals("INT"))
            {
                return Instructions.INT;
            }
            else if (Command.Equals("INC"))
            {
                return Instructions.INC;
            }
            else if (Command.Equals("DEC"))
            {
                return Instructions.DEC;
            }
            else if (Command.Equals("DUP"))
            {
                return Instructions.DUP;
            }
            else if (Command.Equals("INP"))
            {
                return Instructions.INP;
            }
            else if (Command.Equals("PRTD"))
            {
                return Instructions.PRTD;
            }
            else if (Command.Equals("PRTC"))
            {
                return Instructions.PRTC;
            }
            else if (Command.Equals("PRTI"))
            {
                return Instructions.PRTI;
            }
            else if (Command.Equals("JMP"))
            {
                return Instructions.JMP;
            }
            else if (Command.Equals("JIFZ"))
            {
                return Instructions.JIFZ;
            }
            else if (Command.Equals("GT"))
            {
                return Instructions.GT;
            }
            else if (Command.Equals("GTE"))
            {
                return Instructions.GTE;
            }
            else if (Command.Equals("LT"))
            {
                return Instructions.LT;
            }
            else if (Command.Equals("EQ"))
            {
                return Instructions.EQ;
            }
            else if (Command.Equals("LTE"))
            {
                return Instructions.LTE;
            }
            else if (Command.Equals("JIF"))
            {
                return Instructions.JIF;
            }
            else if (Command.Equals("HALT"))
            {
                return Instructions.HALT;
            }
            else if (Command.Equals("STORE"))
            {
                return Instructions.STORE;
            }
            else if (Command.Equals("LOAD"))
            {
                return Instructions.LOAD;
            }
            else
            {
                return Instructions.INVAILD;
            }
        }

        public bool IsNumber(string S)
        {
            double x;
            return Double.TryParse(S, out x);
        }

        public bool Compile(string Filename)
        {
            List<string> Tokens = new List<string>();
            List<double> ByteCode = new List<double>();
            string[] words = { };
            string sLine = string.Empty;

            //This compiles the file into bytecode
            try
            {
                //Token builder
                if (!File.Exists(Filename))
                {
                    //File not found
                    Console.WriteLine("Source File Not Found:\n" + Filename);
                    return false;
                }
                else
                {
                    //Read all the lines from the file.
                    string[] lines = File.ReadAllLines(Filename);

                    foreach (string s in lines)
                    {
                        //Trim the input line
                        sLine = s.Trim();
                        //Check sLine length and check if line is a comment
                        if (sLine.Length > 0 && sLine[0] != ';')
                        {
                            //Split the line into the words list
                            words = sLine.Split(' ','\t');
                            //
                            foreach (string word in words)
                            {
                                //Trim any spaces
                                string sWord = word.Trim();
                                //Check word length
                                if (sWord.Length > 0)
                                {
                                    //Check for label
                                    if (sWord.StartsWith(":"))
                                    {
                                        //Trim : from start of string
                                        sWord = sWord.Remove(0, 1).ToUpper();

                                        if (!Lables.ContainsKey(sWord))
                                        {
                                            //Add label name and address to labels hash table
                                            Lables.Add(sWord, Tokens.Count() - 1);
                                        }
                                    }
                                    else
                                    {
                                        //Add word to main token list
                                        Tokens.Add(sWord);
                                    }
                                }
                            }
                        }
                    }
                    //CPU code builder after we done the first phases above
                    if (Tokens.Count().Equals(0))
                    {
                        Console.WriteLine("Compile Error: Missing Tokens.");
                        return false;
                    }
                    else
                    {
                        //Begin code generator
                        for (int x = 0; x < Tokens.Count;x++ )
                        {
                            //Get token
                            string sTok = Tokens[x];

                            //Get instruction id
                            int check_op = GetOpCodeFromToken(sTok);

                            switch (check_op)
                            {
                                case Instructions.PUSH:
                                    //Get next token
                                    ByteCode.Add(check_op);
                                    x++;
                                    sTok = Tokens[x];
                                    if (IsNumber(sTok))
                                    {
                                        ByteCode.Add(double.Parse(sTok));
                                    }
                                    else
                                    {
                                        //Char
                                        sTok = sTok.Trim('\'');
                                        ByteCode.Add((int)sTok[0]);
                                    }
                                    break;
                                case Instructions.NOP:
                                case Instructions.ADD:
                                case Instructions.SUB:
                                case Instructions.MUL:
                                case Instructions.DIV:
                                case Instructions.AND:
                                case Instructions.OR:
                                case Instructions.XOR:
                                case Instructions.NOT:
                                case Instructions.GT:
                                case Instructions.GTE:
                                case Instructions.LTE:
                                case Instructions.LT:
                                case Instructions.EQ:
                                case Instructions.POP:
                                case Instructions.DUP:
                                case Instructions.INT:
                                case Instructions.INP:
                                case Instructions.INC:
                                case Instructions.DEC:
                                case Instructions.HALT:
                                    ByteCode.Add((int)check_op);
                                    break;
                                case Instructions.PRTD:
                                case Instructions.PRTC:
                                case Instructions.PRTI:
                                    ByteCode.Add(check_op);
                                    break;
                                case Instructions.JMP:
                                case Instructions.JIF:
                                case Instructions.JIFZ:
                                    //Get next token
                                    ByteCode.Add(check_op);
                                    x++;
                                    sTok = Tokens[x];

                                    try
                                    {
                                        ByteCode.Add((int)Lables[sTok.ToUpper()]);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Invalid Jump Address " + sTok);
                                        return false;
                                    }
                                    break;
                                case Instructions.STORE:
                                case Instructions.LOAD:
                                    //
                                    //Get next token
                                    ByteCode.Add(check_op);
                                    x++;
                                    sTok = Tokens[x];
                                    if (IsNumber(sTok))
                                    {
                                        ByteCode.Add(int.Parse(sTok));
                                    }
                                    else
                                    {
                                        //Char
                                        ByteCode.Add((int)sTok[0]);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Invalid Token " + sTok);
                                    return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //Check length of byte code
            if (ByteCode.Count.Equals(0))
            {
                Console.WriteLine("Compile Error");
                return false;
            }

            int i = 0;
            //Resize program data to size of bytecode
            program = new double[ByteCode.Count];

            //Copy the data over to program array
            foreach (double DByte in ByteCode)
            {
                program[i] = DByte;
                i++;    
            }

            //Clear up
            Tokens.Clear();
            ByteCode.Clear();
            Array.Clear(words, 0, words.Length);

            return true;
        }

        /// <summary>
        /// END OF COMPILER
        /// </summary>

        public void ClearUp()
        {
            Array.Clear(this.program, 0, this.program.Length);
            Lables.Clear();
            this.it = 0;
        }

        public bool DoBinBitCmp(int Instruction, double v1, double v2)
        {
            bool v = false;
            int a = Convert.ToInt32(v1);
            int b = Convert.ToInt32(v2);

            switch (Instruction)
            {
                case Instructions.LT:
                    v = (a < b);
                    break;
                case Instructions.GT:
                    v = (a > b);
                    break;
                case Instructions.LTE:
                    v = (a <= b);
                    break;
                case Instructions.GTE:
                    v = (a >= b);
                    break;
                case Instructions.EQ:
                    v = (a == b);
                    break;
            }
            return v;
        }

        public int DoBinBitWise(int Instruction, double v1, double v2){
            int v = 0;

            switch (Instruction)
            {
                case Instructions.AND:
                    v = ((int)v1 & (int)v2);
                    break;
                case Instructions.OR:
                    v = ((int)v1 | (int)v2);
                    break;
                case Instructions.XOR:
                    v = ((int)v1 ^ (int)v2);
                    break;
                case Instructions.MOD:
                    v = ((int)v1 % (int)v2);
                    break;
            }
            return v;
        }

        public double DoBinOp(int Instruction, double v1, double v2)
        {
            double RetVal = 0.0;
            switch (Instruction)
            {
                case Instructions.ADD:
                    RetVal = v1 + v2;
                    break;
                case Instructions.SUB:
                    RetVal = v2 - v1;
                    break;
                case Instructions.MUL:
                    RetVal = v1 * v2;
                    break;
                case Instructions.DIV:
                    RetVal = v2 / v1;
                    break;
                    
            }
            return RetVal;
        }

        public void Execute()
        {
            this.IsRunning = true;
            int vAddr = 0;
            int jAddr = 0;
            int pc = 0;

            double first = 0.0;
            double second = 0.0;

            while (this.IsRunning)
            {

                it = (int)this.program[pc];

                switch (it)
                {
                    case Instructions.HALT:
                        this.IsRunning = false;
                        break;
                    case Instructions.PUSH:
                        pc++;
                        stack.Push(program[pc]);
                        break;
                    case Instructions.ADD:
                    case Instructions.SUB:
                    case Instructions.MUL:
                    case Instructions.DIV:
                        if (stack.Count != 2)
                        {
                            this.IsRunning = false;
                            Console.Write("Runtime Error: Stack Underflow.");
                        }
                        else
                        {
                            //Fetch first and second values of the stack.
                            first = stack.Pop();
                            second = stack.Pop();
                            //Push result onto the stack
                            stack.Push(DoBinOp(it, first, second));
                        }
                        break;
                    case Instructions.AND:
                    case Instructions.OR:
                    case Instructions.XOR:
                    case Instructions.MOD:
                        first = Convert.ToInt32(stack.Pop());
                        second = Convert.ToInt32(stack.Pop());
                        stack.Push(Convert.ToInt32(DoBinBitWise(it,first,second)));
                        break;
                    case Instructions.LT:
                    case Instructions.GT:
                    case Instructions.LTE:
                    case Instructions.GTE:
                    case Instructions.EQ:
                        first = Convert.ToInt32(stack.Pop());
                        second = Convert.ToInt32(stack.Pop());
                        stack.Push(Convert.ToInt32(DoBinBitCmp(it, second, first)));
                        break;
                    case Instructions.NOT:
                        stack.Push(Convert.ToInt32(!Convert.ToBoolean(stack.Pop())));
                        break;
                    case Instructions.NEG:
                        stack.Push(Convert.ToInt32(-stack.Pop()));
                        break;
                    case Instructions.DUP:
                        stack.Push(stack.Peek());
                        break;
                    case Instructions.INT:
                        //Convert what is on the stack to a integer type
                        stack.Push((int)stack.Pop());
                        break;
                    case Instructions.INC:
                        stack.Push(stack.Pop() + 1);
                        break;
                    case Instructions.DEC:
                        stack.Push(stack.Pop() - 1);
                        break;
                    case Instructions.STORE:
                        pc++;
                        vAddr = (int)this.program[pc];
                        CurrentFrame.Setvariable(vAddr, stack.Pop());
                        break;
                    case Instructions.LOAD:
                        pc++;
                        vAddr = (int)this.program[pc];
                        stack.Push(CurrentFrame.GetVariable(vAddr));
                        break;
                    case Instructions.JMP:
                        pc++;
                        pc = (int)this.program[pc];
                        break;
                    case Instructions.JIF:
                        pc++;
                        jAddr = (int)this.program[pc];

                        if (Convert.ToBoolean(stack.Pop()))
                        {
                            pc = jAddr;
                        }

                        break;
                    case Instructions.JIFZ:
                        pc++;
                        jAddr = (int)this.program[pc];

                        if (!Convert.ToBoolean(stack.Pop()))
                        {
                            pc = jAddr;
                        }
                        break;
                    case Instructions.INP:
                        string sValue = Console.ReadLine().Trim();
                        //Input from console and store in stack
                        try
                        {
                            if (IsNumber(sValue))
                            {
                                stack.Push((double.Parse(sValue)));
                            }
                            else
                            {
                                stack.Push((int)sValue[0]);
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine("Runtime Error:\n"  + err.Message);
                            IsRunning = false;
                        }

                        break;
                    case Instructions.PRTD:
                        Console.Write((double)stack.Pop());
                        break;
                    case Instructions.PRTI:
                        Console.Write((int)stack.Pop());
                        break;
                    case Instructions.PRTC:
                        Console.Write((char)stack.Pop());
                        break;
                    case Instructions.POP:
                        if (stack.Count().Equals(0))
                        {
                            this.IsRunning = false;
                            Console.WriteLine("Stack Is Empty.");
                        }
                        else
                        {
                            stack.Pop();
                        }
                        break;
                    case Instructions.NOP:
                        break;
                }
                //Used for getting next instruction
                pc++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CPU vm = new CPU();
            bool Done = false;

            if (args.Length < 1)
            {
                Console.WriteLine("Small Machine Little Language.");
                Console.WriteLine("\tUsage: SMLL.exe Filename.vm");
                return;
            }

            Done = vm.Compile(args[0]);

            if (Done)
            {
                vm.Execute();
            }
            else
            {
                Console.WriteLine("Unexpected Compile Error");
            }

            vm.ClearUp();
        }
    }
}
