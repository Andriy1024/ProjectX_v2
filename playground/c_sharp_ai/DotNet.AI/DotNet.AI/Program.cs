using TorchSharp;


/*
Action - is any choice an AI can make.
*/

// 0 - wall
// 1 - floor
// 2 - goal
int[,] maze1 = {
    //0   1   2   3   4   5   6   7   8   9   10  11
    { 0 , 0 , 0 , 0 , 0 , 2 , 0 , 0 , 0 , 0 , 0 , 0 }, //row 0
    { 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 }, //row 1
    { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 0 }, //row 2
    { 0 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 0 }, //row 3
    { 0 , 0 , 0 , 0 , 1 , 1 , 0 , 1 , 0 , 1 , 1 , 0 }, //row 4
    { 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 }, //row 5
    { 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 }, //row 6
    { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 0 }, //row 7
    { 0 , 1 , 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 0 }, //row 8
    { 0 , 1 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 1 , 1 , 0 }, //row 9
    { 0 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 0 , 1 , 1 , 0 }, //row 10
    { 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 }  //row 11 (start position is (11, 5))
};

const string UP = "up";
const string DOWN = "down";
const string LEFT = "left";
const string RIGHT = "right";

string[] actions = [UP, DOWN, LEFT, RIGHT]; 

const int WALL_REWARD_VALUE = -500;
const int FLOOR_REWARD_VALUE = -10;
const int GOAL_REWARD_VALUE = 500;

var rewards = SetupRewards(maze1);

var qValues = SetupQValues(maze1);

static int[,] SetupRewards(int[,] maze)
{
    int mazeRows = maze.GetLength(0);
    int mazeColumns = maze.GetLength(1);

    var rewards = new int[mazeRows, mazeColumns];

    for (int i = 0; i < mazeRows; i++)
    {
        for (int j = 0; j < mazeColumns; j++)
        {
            switch (maze[i, j])
            {
                case 0:
                    rewards[i, j] = WALL_REWARD_VALUE;
                    break;
                case 1:
                    rewards[i, j] = FLOOR_REWARD_VALUE;
                    break;
                case 2:
                    rewards[i, j] = GOAL_REWARD_VALUE;
                    break;
            }
        }
    }

    return rewards;
}

static torch.Tensor SetupQValues(int[,] maze)
{
    int mazeRows = maze.GetLength(0);
    int mazeColumns = maze.GetLength(1);
    
    // Tensor is a multi-dimensional matrix containing elements of a single data type
    // The 4 here is because there are four possible actions up, down, left, and right.
    torch.Tensor qValues = torch.zeros(mazeRows, mazeColumns, 4);

    return qValues;
}

/*
 Function that will determine if the model has reached a finished state.
 We define a finished state as one of two scenarios.
 The model has hit a wall, in which case it cannot continue to go onto that space,
 or the model has reached the goal and successfully completed navigating the maze.
 */
static bool HasHitWallOrEndOfMaze(int[,] rewards, 
    int currentRow, int currentColumn, 
    int floorValue)
{
    return rewards[currentRow, currentColumn] != floorValue;
}

/*
 The next function is exclusive so it can return a number up to, but not including the number passed in.
 So we pass in four so that the possible values are zero, one, two, and three.
 */
static long DetermineNextAction(
    torch.Tensor qValues,
    int currentRow,
    int currentColumn,
    float epsilon)
{
    var random = new Random();
    
    double randomBetween0And1 = random.NextDouble();
    
    long nextAction = randomBetween0And1 < epsilon
        ? torch.argmax(qValues[currentRow, currentColumn]).item<long>()
        : random.Next(4);
    
    return nextAction;
}

/*
 Once the AI has selected an action, the next step is to actually move it to that space in the environment.
 */
(int, int) MoveOneSpace(
    int[,] maze,
    int currentRow,
    int currentColumn,
    long nextAction)
{
    int mazeRows = maze.GetLength(0);
    int mazeColumns = maze.GetLength(1);
    
    int newRow = currentRow;
    int newColumn = currentColumn;

    if (actions[nextAction] == UP && currentRow > 0) {
        newRow--;
    }
    else if (actions[nextAction] == DOWN && currentRow < mazeRows - 1) {
        newRow++;
    }
    else if (actions[nextAction] == LEFT && currentColumn > 0) {
        newColumn--;
    }
    else if (actions[nextAction] == RIGHT && currentColumn < mazeColumns - 1) {
        newColumn++;
    }
    
    return (newRow, newColumn);
}

void TrainTheModel(int[,] maze, 
    int floorValue, 
    float epsilon, 
    float discountFactor, 
    float learningRate,
    float episodes)
{
    for (int episode = 0; episode < episodes; episode++)
    {
        Console.WriteLine($"--- Starting episode {episode} ---");

        int currentRow = 11;
        int currentColumn = 5;

        while (!HasHitWallOrEndOfMaze(rewards, currentRow, currentColumn, floorValue))
        {
            long currentAction = DetermineNextAction(qValues, currentRow, currentColumn, epsilon);
            
            int previousRow = currentRow;
            int previousColumn = currentColumn;
            
            (int, int) nextMove = MoveOneSpace(maze, currentRow, currentColumn, currentAction);
            currentRow = nextMove.Item1;
            currentColumn = nextMove.Item2;
            
            float reward = rewards[currentRow, currentColumn];
            float previousQValue = qValues[previousRow, previousColumn, currentAction].item<float>();
            float temporalDifference = reward + (discountFactor * torch.max(qValues[currentRow, currentColumn])).item<float>() - previousQValue;
            float nextQValue = previousQValue + (learningRate * temporalDifference);
            
            qValues[previousRow, previousColumn, currentAction] = nextQValue;
        }
        
        Console.WriteLine($"--- Finished episode {episode} ---");
    }

    Console.WriteLine("Completed training!");
}

List<int[]> NavigateMaze(int[,] maze, int startRow, int startColumn, int floorValue, int wallValue)
{
    List<int[]> path = new List<int[]>();
    
    if (HasHitWallOrEndOfMaze(rewards: rewards, startRow, startColumn, floorValue))
    {
        return [];
    }
    
    int currentRow = startRow;
    int currentColumn = startColumn;
    path = [[currentRow, currentColumn]];

    while (!HasHitWallOrEndOfMaze(rewards, currentRow, currentColumn, floorValue))
    {
        int nextAction = (int)DetermineNextAction(qValues, currentRow, currentColumn, 1.0f);
        
        (int, int) nextMove = MoveOneSpace(maze, currentRow, currentColumn, nextAction);
        
        currentRow = nextMove.Item1;
        currentColumn = nextMove.Item2;

        if (rewards[currentRow, currentColumn] != wallValue)
        {
            path.Add([currentRow, currentColumn]);
        }
        else
        {
            continue;
        }
    }

    int moveCount = 1;
    for (int i = 0; i < path.Count; i++)
    {
        Console.WriteLine($"Move {moveCount}: (");
        foreach (var element in path[i])
        {
            Console.Write(" " + element);
        }
        
        Console.Write(" )");
        Console.WriteLine();
        
        moveCount++;
    }

    return path;
}

const float EPSILON = 0.95f;
const float DISCOUNT_FACTOR = 0.8f;
const float LEARNING_RATE = 0.9f;
const int EPISODES = 1500;
const int START_ROW = 11;
const int START_COLUMN = 5;

TrainTheModel(maze1, FLOOR_REWARD_VALUE, EPSILON, DISCOUNT_FACTOR, LEARNING_RATE, EPISODES);
NavigateMaze(maze1, START_ROW, START_COLUMN, FLOOR_REWARD_VALUE, WALL_REWARD_VALUE);