/*
 
    SODV1202 - OOP/23JANMNTR2 - Sohaib Bajwa

    Uyara Montovaneli - 443149
    Hugo Camargo - 440258
    Fabio Weck - 441977

    Final project - Connect 4 Game 
    
 */

using System.Diagnostics.Metrics;

namespace Connect4___Final_Project
{
    public class Board //Draws the board and moves
    {

        private int[,] board;
        private static int ROWS = 6, COLUMNS = 7;
        public Board() //constructor
        {
            board = new int[ROWS, COLUMNS];
        }
        public int[,] GetBoard() //returns board to be used by game
        {
            return board;
        }

        public bool IsValidMove(int column) //returns if this move is valid  
        {
            return column >= 0 && column < 7 && board[0, column] == 0;
        }
        public void DrawMove(int column, int player) //draws the piece of each player in the board by checking if the bottom of the column contains 0 (available 'slot') and replaces it with player's number
        {
            for (int row = 5; row >= 0; row--)
            {
                if (board[row, column] == 0)
                {
                    board[row, column] = player;
                    break;
                }
            }
        }
        public bool IsFull() //checks if the board is completely populated with pieces
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void DrawBoard() //this method draws the board based on numbers
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("       Connect 4 Game      "); //header
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("  1   2   3   4   5   6   7"); //header
            Console.WriteLine("-----------------------------");
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Console.Write("| "); //draws column limits
                    if (board[row, col] == 0)
                    {
                        Console.Write("  "); //include spaces in columns
                    }
                    else if (board[row, col] == 1) //this 'else if' check the player's number 1 and replace with a red 'X'
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                        Console.ResetColor();
                    }
                    else if (board[row, col] == 2) //this 'else if' check the player's number 2 and replace with a blue 'O'
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("O ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("|");
                Console.WriteLine("-----------------------------");//bottom
            }
        }
    }
    public class CheckWin : Board //this derived class acts like a referee on the board, checking if any of the players won the game
    {

        int[,] boardArray;

        public CheckWin(Board currentBoard)  //constructor
        {
            boardArray = currentBoard.GetBoard();
        }

        public bool CheckWinOrNot() //main method to determine the winner
        {
            
            for (int row = 0; row < 6; row++) // check for horizontal win
            {
                for (int col = 0; col < 4; col++)
                {
                    if (boardArray[row, col] != 0 &&
                        boardArray[row, col] == boardArray[row, col + 1] &&
                        boardArray[row, col] == boardArray[row, col + 2] &&
                        boardArray[row, col] == boardArray[row, col + 3])
                    {
                        return true;
                    }
                }
            }

            
            for (int row = 0; row < 3; row++) // check for vertical win
            {
                for (int col = 0; col < 7; col++)
                {
                    if (boardArray[row, col] != 0 &&
                        boardArray[row, col] == boardArray[row + 1, col] &&
                        boardArray[row, col] == boardArray[row + 2, col] &&
                        boardArray[row, col] == boardArray[row + 3, col])
                    {
                        return true;
                    }
                }

            }

            
            for (int row = 0; row < 3; row++) // check for diagonal win (top-left to bottom-right)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (boardArray[row, col] != 0 &&
                        boardArray[row, col] == boardArray[row + 1, col + 1] &&
                        boardArray[row, col] == boardArray[row + 2, col + 2] &&
                        boardArray[row, col] == boardArray[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            
            for (int row = 3; row < 6; row++) // check for diagonal win (bottom-left to top-right)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (boardArray[row, col] != 0 &&
                        boardArray[row, col] == boardArray[row - 1, col + 1] &&
                        boardArray[row, col] == boardArray[row - 2, col + 2] &&
                        boardArray[row, col] == boardArray[row - 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
    }

    class Game //this class is the intermediary between players and the board
    {
        private Board board;
        private Player player1, player2;
        private CheckWin winner;

        public Game(Board board, Player player1, Player player2) //constructor
        {
            this.board = board;
            this.player1 = player1;
            this.player2 = player2;
            winner = new CheckWin(board);
        }

        internal void MakeMove(int column, int player) //checks the validity of the move and give the task of drawing the move to the board based on the player ID and column selected
        {
            if (board.IsValidMove(column))
            {
                board.DrawMove((column), player);
            }
            else
            {
                Console.WriteLine("Invalid move, try again..."); //advises the player if the move is invalid (like selecting wrong column or trying to put pieces in a full populated column)
            }
        }

        internal void Play() //main method to run the game - draws moves, check winners and full board
        {

            board.DrawBoard();
            
            while (true)
            {

                MakeMove(player1.Play(board), player1.GetPlayerID());

                board.DrawBoard();

                if (winner.CheckWinOrNot())
                {
                    player1.WriteWinnerMsg();

                    break; //break to exit the loop if player one wins
                }

                MakeMove(player2.Play(board), player2.GetPlayerID());
                board.DrawBoard();

                if (winner.CheckWinOrNot())
                {
                    player2.WriteWinnerMsg(); //break to exit the loop if player one wins
                    break;
                }

                if (board.IsFull()) //display a message to players if the board is full populated
                {
                    string tie = "\r\n  ________________   \r\n /_  __/  _/ ____/   \r\n  / /  / // __/      \r\n / / _/ // /____ _ _ \r\n/_/ /___/_____(_(_(_)\r\n                     \r\n";
                    
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Yellow;
                    Console.WriteLine(tie);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to return to main menu...");
                    Console.ReadKey(); //exits the loop after user input (ENTER)
                  
                    break;
                } 
            }
        }
    }

    public abstract class Player  //abstract class stores player's ID and provide methods to be overriden
    {
        protected int playerID;
        public Player(int playerID) //constructor
        {
            this.playerID = playerID;
        }

        public int GetPlayerID() //returns plyer ID
        {
            return this.playerID;
        }        
        
        public abstract int Play(Board board);

        public abstract void WriteWinnerMsg();
    }

    public class HumanPlayer : Player //derived class from Player class - human player
    {
        private int move;

        public HumanPlayer(int playerID) : base(playerID)
        {
            base.playerID = playerID;
        }

        private int Move(int move)  //returns move -1 to match column index (0 to 6)
        {
            this.move = move;
            return this.move - 1;
        }

        public override int Play(Board board) //collets move from player, checks validity and return move 
        {
            Console.WriteLine();
            Console.WriteLine("Player " + playerID + " turn");
            Console.WriteLine("Choose a number from 1 to 7");

            int move = Move(Convert.ToInt32(Console.ReadLine()));

            while (!board.IsValidMove(move))
            {
                Console.WriteLine("Invalid move. Try again!");                
                move = Move(Convert.ToInt32(Console.ReadLine()));
            }

            return move;
        }

        public override void WriteWinnerMsg() //method to display winner
        {

            string player1 = "\r\n    ____  __                                                      _            __\r\n   / __ \\/ ____ ___  _____  _____   ____  ____  ___     _      __(_____  _____/ /\r\n  / /_/ / / __ `/ / / / _ \\/ ___/  / __ \\/ __ \\/ _ \\   | | /| / / / __ \\/ ___/ / \r\n / ____/ / /_/ / /_/ /  __/ /     / /_/ / / / /  __/   | |/ |/ / / / / (__  /_/  \r\n/_/   /_/\\__,_/\\__, /\\___/_/      \\____/_/ /_/\\___/    |__/|__/_/_/ /_/____(_)   \r\n              /____/                                                             \r\n";
            string player2 = "\r\n    ____  __                         __                             _            __\r\n   / __ \\/ ____ ___  _____  _____   / /__      ______     _      __(_____  _____/ /\r\n  / /_/ / / __ `/ / / / _ \\/ ___/  / __| | /| / / __ \\   | | /| / / / __ \\/ ___/ / \r\n / ____/ / /_/ / /_/ /  __/ /     / /_ | |/ |/ / /_/ /   | |/ |/ / / / / (__  /_/  \r\n/_/   /_/\\__,_/\\__, /\\___/_/      \\__/ |__/|__/\\____/    |__/|__/_/_/ /_/____(_)   \r\n              /____/                                                               \r\n";
            string win;

            if(playerID == 1) //assigns message to each player
            {
                win = player1;
            }
            else 
            {
                win = player2;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(win);
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("Press ENTER to return to main menu...");
            Console.ReadKey();

        }
    }

    public class AiPlayer : Player //derived class from Player class - AiPlayer (computer)
    {
        public AiPlayer(int playerID) : base(playerID)
        {
            base.playerID = playerID;
        }

        private int Move(Board board) //this method checks move validity and provides a random number as A.I. move
        {
            var validMoves = new List<int>();

            for (int column = 0; column < 7; column++) 
            {
                if (board.IsValidMove(column))
                {
                    validMoves.Add(column);
                }
            }

            var random = new Random();

            string aiTurn = "The computer is planning a move..."; //displays a message when planning a move

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (char c in aiTurn) //loop to deliver character by character in a determined flow
            {
                Console.Write(c);
                Thread.Sleep(30);
            }

            Thread.Sleep(1500);
            Console.ResetColor();

            return validMoves[random.Next(validMoves.Count)];
        }

        public override int Play(Board board) //returns AI move
        {
            return Move(board);
        }

        public override void WriteWinnerMsg() //customized message displaying that the human player lost the game
        {
            string win = "\r\n__  __               __                   \r\n\\ \\/ ____  __  __   / ____  ________      \r\n \\  / __ \\/ / / /  / / __ \\/ ___/ _ \\     \r\n / / /_/ / /_/ /  / / /_/ (__  /  ___ _ _ \r\n/_/\\____/\\__,_/  /_/\\____/____/\\___(_(_(_)\r\n                                          \r\n";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(win);
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("Press ENTER to return to main menu...");
            Console.ReadKey();
        }
    }

    class Menu //class with all menu functionalities 
    {
        static int SKIPINTRO = 0; //static variable to avoid game returning to first page

        private static string instructions = @"
                OBJECTIVE:

                To be the first player to connect 4 of the same colored discs in a row (either vertically, horizontally, or diagonally)


                HOW TO PLAY:
                
                Players must alternate turns, and only one number must be choose in each turn. 
                On your turn, choose one number from 1 to 7. 
                The game ends when there is a 4-in-a-row or a stalemate.
                The starter of the previous game goes second on the next game.



        ";

        public Game MainMenu() {

            string connect4 = " \r\n $$$$$$\\                                                      $$\\           $$\\   $$\\ \r\n$$  __$$\\                                                     $$ |          $$ |  $$ |\r\n$$ /  \\__| $$$$$$\\  $$$$$$$\\  $$$$$$$\\   $$$$$$\\   $$$$$$$\\ $$$$$$\\         $$ |  $$ |\r\n$$ |      $$  __$$\\ $$  __$$\\ $$  __$$\\ $$  __$$\\ $$  _____|\\_$$  _|        $$$$$$$$ |\r\n$$ |      $$ /  $$ |$$ |  $$ |$$ |  $$ |$$$$$$$$ |$$ /        $$ |          \\_____$$ |\r\n$$ |  $$\\ $$ |  $$ |$$ |  $$ |$$ |  $$ |$$   ____|$$ |        $$ |$$\\             $$ |\r\n\\$$$$$$  |\\$$$$$$  |$$ |  $$ |$$ |  $$ |\\$$$$$$$\\ \\$$$$$$$\\   \\$$$$  |            $$ |\r\n \\______/  \\______/ \\__|  \\__|\\__|  \\__| \\_______| \\_______|   \\____/             \\__|\r\n                                                                                      \r\n                                                                                      \r\n                                                                                      \r\n";
            string presentation = "Bow Valley College - SODV1202\nConnect 4 project\n\nHugo Camargo, Uyara Montovaneli and Fabio Weck. 2023.\n";

            while (SKIPINTRO == 0) //this loop runs only once to display game presentation
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(connect4);
                Console.ResetColor();

                foreach(char c in presentation) //loop to display presentation in a determined flow
                {
                    Console.Write(c);
                    Thread.Sleep(30);
                }

                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Press ENTER to start...");
                Console.ReadLine();
                SKIPINTRO = 1; //changes the variable to avoid running presentation again
            }
            
            while (true)
            {

                int firstOption;
                
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(connect4);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Play");
                Console.WriteLine("2. Instructions");
                Console.WriteLine("3. Exit");
                Console.WriteLine();
                
                firstOption = Convert.ToInt32(Console.ReadLine());
                
                switch (firstOption) //this switch goes through menu options
                {
                    case 1: //play

                        Console.Clear();                   
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(connect4);
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("1. One player (with AI)");
                        Console.WriteLine("2. Two players");
                        Console.WriteLine();

                        int secondOption = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        Board board = new Board(); //when user selects play, new board and players are instantiated (2 human or 1 human + AI)
                        if (secondOption == 1)
                        {
                            return new Game(board, new HumanPlayer(1), new AiPlayer(2));
                        }
                        return new Game(board, new HumanPlayer(1), new HumanPlayer(2));


                    case 2: //instructions presentation
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(connect4);
                        Console.ResetColor();

                        foreach (char c in instructions)
                        {
                            Console.Write(c);
                            Thread.Sleep(1);
                        }

                        Console.WriteLine();
                        Console.WriteLine("Press ENTER to return to main menu...");
                        Console.ReadLine();
                        break;

                    case 3: //exits the game
                        System.Environment.Exit(0);
                        break;

                }
            }                            
        }
    }


    internal class Program //runs once with instantiated objects
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Menu menu = new Menu(); //instantiates a new menu
                Game game = menu.MainMenu();  //calls MainMenu method
                game.Play(); //calls Play method
            }            
        }
    }
}