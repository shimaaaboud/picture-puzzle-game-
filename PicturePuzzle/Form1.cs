using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PicturePuzzle
{
    public partial class Form1 : Form
    {                                                                                                                                    
        public int[] GameSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        public Puzzlepicture[] pictures = new Puzzlepicture[9];
        public Form1()
        {
            InitializeComponent();
            
            for (int c1 = 0; c1 < 9; c1++)
            {
                pictures[c1]= new Puzzlepicture() ;
    
                pictures[c1].index = c1;
                pictures[c1].position = c1;
                pictures[c1].xindex =c1 % 3;
                pictures[c1].yindex =  c1 / 3;
                pictures[c1].Location = new Point(pictures[c1].xindex * 125, pictures[c1].yindex * 125);
                pictures[c1].Size = new Size(120, 120);
                pictures[c1].Image = Image.FromFile(Application.StartupPath + "\\picture\\" +( c1+1 )+ ".jpg");
                this.Controls.Add(pictures[c1]);
                pictures[c1].Click += new EventHandler(picture_clicked);
            }
            pictures[8].Visible = false;

            shuffle();

        }
        void picture_clicked(object sender, EventArgs e)
        {
            Puzzlepicture temp = sender as Puzzlepicture;
            int i = temp.index;
        

            if (pictures[8].xindex == pictures[i].xindex && Math.Pow((pictures[8].yindex - pictures[i].yindex),2) == 1)
            {
                swapPicture(pictures[8], pictures[i]);
                checkIfSolved();
                updateSequence();
            }
            else if (pictures[8].yindex == pictures[i].yindex && Math.Pow((pictures[8].xindex - pictures[i].xindex), 2) == 1)
            {

                swapPicture(pictures[8], pictures[i]);
                checkIfSolved();
                updateSequence();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            BFS FindSolution = new BFS(GameSequence);
            int[] temp = new int[9];
           //System.Threading.Thread.Sleep(1000);
            
            foreach (PuzzleNode tn in FindSolution.NodeStack )
            {
                temp = tn.Getsequence();
                System.Threading.Thread.Sleep(1000);

                for (int c1 = 0; c1 < 9; c1++)
                {
                    pictures[temp[c1]].position = c1;
                    pictures[temp[c1]].xindex = c1  % 3;
                    pictures[temp[c1]].yindex = c1 / 3;
                    pictures[temp[c1]].Location = new Point(pictures[temp[c1]].xindex * 125, pictures[temp[c1]].yindex * 125);
                
                }
                

                updateSequence();


            }
          

        }
        void swapPicture(Puzzlepicture p1, Puzzlepicture p2)
        {
                Puzzlepicture temp= new Puzzlepicture();
                temp.position = p1.position;
                temp.xindex = p1.xindex;
                temp.yindex = p1.yindex;
                temp.Location = p1.Location;

                p1.position = p2.position;
                p1.xindex = p2.xindex;
                p1.yindex =p2.yindex;
                p1.Location = p2.Location;

                p2.position = temp.position;
                p2.xindex = temp.xindex;
                p2.yindex = temp.yindex;
                p2.Location = temp.Location;
                

        }
        bool checkIfSolved()
        {
            bool temp=true;
            for (int c1 = 0; c1 < 9; c1++)
            {
                if (pictures[c1].position != c1)
                {
                    temp = false;
                    break;
                }

            }
            if (temp)
            {
                MessageBox.Show("solved by user");
            }
            return temp;
        }
        void shuffle()
        {
            Random r = new Random();
            for (int c1 = 0; c1 < 9; c1++)
            {
                int n1 = r.Next(0, 8);
                swapPicture(pictures[c1],pictures[n1]);
                
            }
            updateSequence();

        }
        void updateSequence()
        {


            for (int c1 = 0; c1 < 9; c1++)
            {
                int n1 = pictures[c1].position;
                GameSequence[n1] = c1;

            }

 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            shuffle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DFS FindSolution = new DFS(GameSequence);
            int[] temp = new int[9];

            foreach (PuzzleNode tn in FindSolution.NodeStack)
            {
                temp = tn.Getsequence();
                //System.Threading.Thread.Sleep(1000);

                for (int c1 = 0; c1 < 9; c1++)
                {
                    pictures[temp[c1]].position = c1;
                    pictures[temp[c1]].xindex = c1 % 3;
                    pictures[temp[c1]].yindex = c1 / 3;
                    pictures[temp[c1]].Location = new Point(pictures[temp[c1]].xindex * 125, pictures[temp[c1]].yindex * 125);

                }


                updateSequence();


            }
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
    public class Puzzlepicture : PictureBox
    {
        public int index;
        public int position;
        public int xindex;
        public int yindex;
    }
    public class PuzzleNode
    {
        string NodeHashe="";
        PuzzleNode ParentNode;
        int[] tilesequence=new int[9];
        public PuzzleNode (PuzzleNode N,int[] sequence)
        {
       
            ParentNode = N;
             //as arrays are passed by reference so changing the passed array will change this
            //internal array which thing we don't want to occure
            sequence.CopyTo(tilesequence,0);
            NodeHashe = GetNodeHash();
        }
        public int[] Getsequence()
        {
            int[] temp = new int[9];
            tilesequence.CopyTo(temp,0);
            return temp ;
        }
        public string GetNodeHash ()
        {
            string temp="";
            if (NodeHashe != "")
            { temp = NodeHashe; }
            else
            {
                for (int c1 = 0; c1 < 9; c1++)
                { temp += tilesequence[c1]; }
            }
            return temp;

        }
        public string GetNodeHash (PuzzleNode node)
        {
            string tempStr="";
            int[] tempArr=node.Getsequence();
                for (int c1 = 0; c1 < 9; c1++)
                { tempStr += tempArr [c1]; }
            
            return tempStr;

        }
        public bool EqualNodes(PuzzleNode n1, PuzzleNode n2)
        {

            bool equalSequence = false;
            if (n1 != null && n2 != null)
            {
                equalSequence = true;
                int[] temp1 = n1.Getsequence();
                int[] temp2 = n2.Getsequence();
                for (int c1 = 0; c1 < 9; c1++)
                {
                    if (temp1[c1] != temp2[c1])
                    {
                        equalSequence = false;
                        break;
                    }

                }
            }
            return equalSequence; 
            
        }
        public bool CheckIfSolved()
        {
            
            return EqualNodes (this,Constants.sortedNode ); 
            
        }
        public PuzzleNode getNodeParent()
        {
            return ParentNode;
        }
        public PuzzleNode left()
        {
            int[] tempSequence;
            tempSequence = this.Getsequence();
            PuzzleNode tempNode= null ;            
            for (int c1 = 0; c1 < 9; c1++)
            {
                if (tempSequence[c1] == 8)
                {
                    if (  c1 % 3 > 0 ) //if the nine (i.e empty tile) is not on the most left columen
                                              //c1+4(due to c1 +1(make index start from 1) +3 (make index more than to 3 to avoid 2%3 =0
                    {                         //c1 -1 to see if it the tile next to the most right one
                        //swap the position of empty tile and tile left to it
                        tempSequence[c1] = tempSequence[c1 - 1];
                        tempSequence[c1 - 1] = 8;
                        tempNode = new PuzzleNode(this, tempSequence);
                        
                        break;
                    }
                }
             }
            return tempNode;
        }


        public PuzzleNode right()
        {
            int[] tempSequence;
            tempSequence = this.Getsequence();
            PuzzleNode tempNode = null;
            for (int c1 = 0; c1 < 8; c1++)
            {
                if (tempSequence[c1] == 8)
                {
                    if (c1  % 3 < 2) //if the zero(i.e empty tile) is not on the most right columen
                    //c1+4(due to c1 +1(make index start from 1) +3 (make index more than to 3 to avoid 2%3 =0

                    {
                        //swap the position of empty tile and tile right to it
                        tempSequence[c1] = tempSequence[c1 + 1];
                        tempSequence[c1 + 1] = 8;
                        tempNode = new PuzzleNode(this, tempSequence);
                        
                        break;
                    }
                }
            }
            return tempNode;
        }

        public PuzzleNode up()
        {
            int[] tempSequence;
            tempSequence = this.Getsequence();
            PuzzleNode tempNode = null;
            for (int c1 = 0; c1 < 9; c1++)
            {
                if (tempSequence[c1] == 8)
                {
                    if (c1 / 3 > 0) //if the zero(i.e empty tile) is not on the most upper row
                    {
                        //swap the position of empty tile and tile upper to it
                        tempSequence[c1] = tempSequence[c1 - 3];
                        tempSequence[c1 - 3] = 8;
                        tempNode = new PuzzleNode(this, tempSequence);
                        break;
                    }
                }
            }
            return tempNode;
        }

        public PuzzleNode down()
        {
            int[] tempSequence;
            tempSequence = this.Getsequence();
            PuzzleNode tempNode = null;
            for (int c1 = 0; c1 < 9; c1++)
            {
                if (tempSequence[c1] == 8)
                {
                    if (c1 / 3 < 2) //if the zero(i.e empty tile) is not on the most lower row 
                    {
                        //swap the position of empty tile and tile lower to it
                        tempSequence[c1] = tempSequence[c1 + 3];
                        tempSequence[c1 + 3] = 8;
                        tempNode = new PuzzleNode(this, tempSequence);
                        break; 
                    }
                }
            }
            return tempNode;
        }
       
       
       
    }
    public class BFS 
    {
        int[] testsequence = new int[9];
        public PuzzleNode firstNode;
        PuzzleNode currentNode;
        PuzzleNode[] childNodes=new  PuzzleNode[4];
        Queue<PuzzleNode> BFSQueue = new Queue<PuzzleNode>();
        Hashtable Searched=new Hashtable ();
        bool solved = false;
        public Stack<PuzzleNode> NodeStack = new Stack<PuzzleNode>();

        public  BFS (int [] seq)
        {
            seq.CopyTo(testsequence,0);
            firstNode = new PuzzleNode(null, testsequence);
            BFSQueue.Enqueue(firstNode);

            while (BFSQueue.Count > 0 && !solved  )
            {
                currentNode  = BFSQueue.Dequeue();
                if (Searched.Contains(currentNode.GetNodeHash()))
                {
                    currentNode = null;
                    continue ;
                }
                else
                {
                    Searched.Add(currentNode.GetNodeHash(),currentNode );
                }

                childNodes[0] = currentNode.left();
                childNodes[1] = currentNode.right();
                childNodes[2] = currentNode.up();
                childNodes[3] = currentNode.down();
                for (int c1 = 0; c1 < 4; c1++)
                {
                    if (childNodes[c1] != null)
                    {
                        if (childNodes[c1].CheckIfSolved())
                        {
                            //call solution has been found
                            //Console.WriteLine("soultion found ,number of searched nodes{0}",Searched.Count);
                            
                            solved = true;
                            SolutionFound(childNodes[c1]);
                            break;
                        }
                        else
                        {
                                BFSQueue.Enqueue(childNodes[c1]);
                        }

                    }

                }
                

            }
            if (!solved)
            {
                MessageBox.Show("not solved in " + Searched.Count + " nodes");

            }

            
        }

        void SolutionFound(PuzzleNode lastNode)
        {

            while (lastNode.getNodeParent() != null)
            {
                NodeStack.Push(lastNode);
                lastNode = lastNode.getNodeParent();
            }
            MessageBox.Show("solved by BfS algorithm in " + Searched.Count + " nodes and number of steps is "+ NodeStack.Count );
           }
    }
    public class DFS
    {
        int[] testsequence = new int[9];
        public PuzzleNode firstNode;
        PuzzleNode currentNode;
        PuzzleNode[] childNodes = new PuzzleNode[4];
        Stack<PuzzleNode> DFSstack = new Stack<PuzzleNode>();
        Hashtable Searched = new Hashtable();
        bool solved = false;
        public Stack<PuzzleNode> NodeStack = new Stack<PuzzleNode>();

        public DFS(int[] seq)
        {
            seq.CopyTo(testsequence, 0);
            firstNode = new PuzzleNode(null, testsequence);
            DFSstack.Push(firstNode);

            while (DFSstack.Count > 0 && !solved)
            {
                currentNode = DFSstack.Pop();
                if (Searched.Contains(currentNode.GetNodeHash()))
                {
                    currentNode = null;
                    continue;
                }
                else
                {
                    Searched.Add(currentNode.GetNodeHash(), currentNode);
                }

                childNodes[0] = currentNode.left();
                childNodes[1] = currentNode.right();
                childNodes[2] = currentNode.up();
                childNodes[3] = currentNode.down();
                for (int c1 = 0; c1 < 4; c1++)
                {
                    if (childNodes[c1] != null)
                    {
                        if (childNodes[c1].CheckIfSolved())
                        {
                            //call solution has been found
                            //Console.WriteLine("soultion found ,number of searched nodes{0}",Searched.Count);

                            solved = true;
                            SolutionFound(childNodes[c1]);
                            break;
                        }
                        else
                        {
                            DFSstack.Push (childNodes[c1]);
                        }

                    }

                }


            }
            if (!solved)
            {
                MessageBox.Show("not solved in " + Searched.Count + " nodes");

            }


        }

        void SolutionFound(PuzzleNode lastNode)
        {

            while (lastNode.getNodeParent() != null)
            {
                NodeStack.Push(lastNode);
                lastNode = lastNode.getNodeParent();
            }
            MessageBox.Show("solved by BfS algorithm in " + Searched.Count + " nodes and number of steps is " + NodeStack.Count);
        }
    }
    public static class Constants
    {
        public static  PuzzleNode sortedNode ;
        static  Constants ()
        {
        sortedNode =new PuzzleNode(null,new int[]{0,1,2,3,4,5,6,7,8}) ;
        
        }
    }

}


