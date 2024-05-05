game_loop();


char check_winner(char[,] position)
{
    // For rows
    for (int r = 0; r < position.GetLength(0); r++) {
        if (position[r, 0] != '_' && position[r, 0] == position[r, 1] && position[r, 1] == position[r, 2])
            return position[r, 0];
    }
    // For cols
    for (int c = 0; c < position.GetLength(1); c++) {
        if (position[0, c] != '_' && position[0, c] == position[1, c] && position[1, c] == position[2, c])
            return position[0, c];
    }
    // For diagonals
    if (position[0, 0] != '_' && position[0, 0] == position[1, 1] && position[1, 1] == position[2, 2])
        return position[0, 0];

    if (position[0, 2] != '_' && position[0, 2] == position[1, 1] && position[1, 1] == position[2, 0])
        return position[0, 2];

    return '_';
}

bool moves_remaining(char[,] position) 
{
    for (int r = 0; r < position.GetLength(0); r++) {
        for (int c = 0; c < position.GetLength(1); c++) {
            if (position[r, c] == '_') return true;
        }
    }

    return false;
} 

int nums = 0;

int minimax(char[,] position, bool maximizing, int alpha, int beta, int depth)
{
    nums++;
    if (check_winner(position) != '_' || moves_remaining(position) == false) {
        if (check_winner(position) == 'X') 
            return 10;
        else if (check_winner(position) == 'O')
            return -10;
        else 
            return 0;
    }

    if (maximizing) {
        int max_eval = int.MinValue;

        for (int r = 0; r < position.GetLength(0); r++) {
            for (int c = 0; c < position.GetLength(1); c++) {
                if (position[r, c] == '_') {
                    position[r, c] = 'X';
                    int eval = minimax(position, false, alpha, beta, depth + 1);
                    max_eval = Math.Max(max_eval, eval);
                    position[r, c] = '_';
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha) 
                        break;
                }
            }
        }
        
        return max_eval;
    }
    else {
        int min_eval = int.MaxValue;

        for (int r = 0; r < position.GetLength(0); r++) {
            for (int c = 0; c < position.GetLength(1); c++) {
                if (position[r, c] == '_') {
                    position[r, c] = 'O';
                    int eval = minimax(position, true, alpha, beta, depth + 1);
                    min_eval = Math.Min(min_eval, eval);
                    position[r, c] = '_';
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)  
                        break;
                }
            }
        }

        return min_eval;
    }
}

int[] pick_best_move(char[,] position)
{
    int[] best_move = {-1, -1};
    int min_eval = int.MaxValue;
    nums = 0;
    for (int r = 0; r < position.GetLength(0); r++) {
        for (int c = 0; c < position.GetLength(1); c++) {
            if (position[r, c] == '_') {
                position[r, c] = 'O';
                int eval = minimax(position, true, int.MinValue, int.MaxValue, 0);
                position[r, c] = '_';
                if (eval < min_eval) {
                    min_eval = eval;
                    best_move[0] = r;
                    best_move[1] = c;
                }
            }
        }
    }

    Console.WriteLine("Number of moves checked: " + nums);

    return best_move;
}

void game_loop()
{
    char[,] board = {
        {'_', '_', '_'},
        {'_', '_', '_'},
        {'_', '_', '_'},
    };
    char player = 'X';
    char computer = 'O';
    char side = player;

    while (true) {
        Console.Write(print_board(board));
        int[] move = new int[2];

        if (side == player) {
            Console.WriteLine("Choose square from 1 to 9:");

            int choice = Convert.ToInt32(Console.ReadLine());

            while (choice < 1 || choice > 9) {
                Console.WriteLine("I said from 1 to 9 you bigot");
                choice = Convert.ToInt32(Console.ReadLine());
            }

            choice--;

            int x = choice % 3;
            int y = (int)Math.Floor(choice / 3f);

            while (board[y, x] != '_') {
                Console.WriteLine("Choose an empty square you fucking idiot.");
                choice = Convert.ToInt32(Console.ReadLine());
                choice--;
                x = choice % 3;
                y = (int)Math.Floor(choice / 3f);
            }

            move[0] = y;
            move[1] = x;
        }
        else {
            move = pick_best_move(board);
        }

        board[move[0], move[1]] = side;

        if (check_winner(board) != '_' || moves_remaining(board) == false) {
            Console.Write(print_board(board));
            if (check_winner(board) == player) {
                Console.WriteLine("You win.");
                break;
            }
            else if (check_winner(board) == computer) {
                Console.WriteLine("You lose.");
                break;
            }
            else {
                Console.WriteLine("It's a draw.");
                break;
            }
        }

        side = player == side ? computer : player;
    }
}

string print_board(char[,] position)
{
    string s = "";

    s += $"{position[0, 0]} | {position[0, 1]} | {position[0, 2]}\n";
    s += "---------\n";
    s += $"{position[1, 0]} | {position[1, 1]} | {position[1, 2]}\n";
    s += "---------\n";
    s += $"{position[2, 0]} | {position[2, 1]} | {position[2, 2]}\n";

    return s;
}