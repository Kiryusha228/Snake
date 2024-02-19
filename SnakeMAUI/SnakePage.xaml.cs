using System.Timers;
using SharpHook;
using SharpHook.Native;
using SnakeConsole;

namespace SnakeMAUI;

public partial class SnakePage : ContentPage
{

    Grid grid;
    Grid backgroundGrid;

    SwipeGestureRecognizer leftSwipeGesture;
    SwipeGestureRecognizer rightSwipeGesture;
    SwipeGestureRecognizer upSwipeGesture;
    SwipeGestureRecognizer downSwipeGesture;

    private Image[,] images;
    private Image[,] background;
    private Queue<Snake.SnakeDirection> directions;
    private Queue<Snake.SnakeDirection?> rotations;
    private Snake snake;
    private Field field;
    private System.Timers.Timer timer;
    private ImageSource grass;
    private ImageSource empty;
    private ImageSource head;
    private ImageSource body;
    private ImageSource bodyrot;
    private ImageSource apple;
    private ImageSource tale;
    private Snake.SnakeDirection? lastDirection;

    private int fieldSizeX;
    private int fieldSizeY;
    private int gridSize;

    public TaskPoolGlobalHook hook;
    public SnakePage(int fieldSize)
    {
        GC.Collect();
        InitializeComponent();

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            gridSize = 500;
        }
        else
        {
            if (DeviceDisplay.MainDisplayInfo.Width > DeviceDisplay.MainDisplayInfo.Height)
            {
                gridSize = (int)(DeviceDisplay.MainDisplayInfo.Height * 0.35);
            }
            else
            {
                gridSize = (int)(DeviceDisplay.MainDisplayInfo.Width * 0.35);
            }
        }

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            if (hook == null)
            {
				hook = new TaskPoolGlobalHook();
				hook.KeyPressed += Hook_KeyPressed;
				Run();
			}
        }
        
        Initialize(fieldSize);

    }

    private async void Run()
    {
		await hook.RunAsync();
    }


    private void Hook_KeyPressed(object sender, KeyboardHookEventArgs e)
    {
        if (directions.Count < 3)
        {

            switch (e.Data.KeyCode)
            {
                case KeyCode.VcNumPadLeft:
                    directions.Enqueue(Snake.SnakeDirection.Left);
                    break;
                case KeyCode.VcNumPadRight:
                    directions.Enqueue(Snake.SnakeDirection.Right);
                    break;
                case KeyCode.VcNumPadUp:
                    directions.Enqueue(Snake.SnakeDirection.Up);
                    break;
                case KeyCode.VcNumPadDown:
                    directions.Enqueue(Snake.SnakeDirection.Down);
                    break;
            }
        }
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (directions.Count != 0)
        {
            snake._direction = directions.Dequeue();
        }
        bool cont = snake.Move(field.SizeX, field.SizeY);
        if (cont)
        {
            
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    Draw(field.DrawField(snake));
                });
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Draw(field.DrawField(snake));
                });
            }
        }

        if (snake.Size == fieldSizeX * fieldSizeY)
        {
            Endgame(true);
        }
        if (!cont)
        {
            Endgame(false);
        }
    }

    private void Endgame(bool won)
    {
		timer.Stop();
		hook.Dispose();
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                if (won)
                {
                    await DisplayAlert("You WON!!!!!", $"Score: {snake.Size}", "Back");
                }
                else
                {
                    await DisplayAlert("You lost!", $"Score: {snake.Size}", "Back");
                }
                
                await Navigation.PopAsync();
            });
        }
        else
        {
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (won)
                {
                    await DisplayAlert("You WON!!!!!", $"Score: {snake.Size}", "Back");
                }
                else
                {
                    await DisplayAlert("You lost!", $"Score: {snake.Size}", "Back");
                }
                await Navigation.PopAsync();
            });
        }
		
	}

    private void Initialize(int fieldSize)
    {
        fieldSizeX = fieldSize;
        fieldSizeY = fieldSize;
        images = new Image[fieldSizeX, fieldSizeY];
        background = new Image[fieldSizeX, fieldSizeY];
        directions = new Queue<Snake.SnakeDirection>();
        rotations = new Queue<Snake.SnakeDirection?>();
        grass = ImageSource.FromFile("grass.bmp");
        empty = ImageSource.FromFile("empty.png");
        head = ImageSource.FromFile("head.png");
        body = ImageSource.FromFile("body.png");
        bodyrot = ImageSource.FromFile("bodyrot.png");
        tale = ImageSource.FromFile("tale.png");
        apple = ImageSource.FromFile("apple.png");
        snake = new Snake(fieldSizeX / 2, fieldSizeY / 2);
        field = new Field(fieldSizeX, fieldSizeY);
        timer = new System.Timers.Timer(300);
        timer.Elapsed += OnTimedEvent;
        lastDirection = Snake.SnakeDirection.Right;
        timer.Start();
        CreateGrids();

        for (int i = 0; i < fieldSize; i++)
        {
            grid.AddRowDefinition(new RowDefinition());
            grid.AddColumnDefinition(new ColumnDefinition());
            backgroundGrid.AddRowDefinition(new RowDefinition());
            backgroundGrid.AddColumnDefinition(new ColumnDefinition());
        }

        for (int i = 0; i < fieldSizeX; i++)
        {
            for (int j = 0; j < fieldSizeY; j++)
            {
                images[i, j] = new Image();
                images[i, j].SetValue(Grid.RowProperty, i);
                images[i, j].SetValue(Grid.ColumnProperty, j);
                grid.Children.Add(images[i, j]);
                images[i, j].Source = empty;
            }
        }

        for (int i = 0; i < fieldSizeX; i++)
        {
            for (int j = 0; j < fieldSizeY; j++)
            {
                background[i, j] = new Image();
                background[i, j].SetValue(Grid.RowProperty, i);
                background[i, j].SetValue(Grid.ColumnProperty, j);
                backgroundGrid.Children.Add(background[i, j]);
                background[i, j].Source = grass;
            }
        }
    }

    private void CreateGrids()
    {
        leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
        rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
        upSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up };
        downSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };

        leftSwipeGesture.Swiped += OnSwiped;
        rightSwipeGesture.Swiped += OnSwiped;
        upSwipeGesture.Swiped += OnSwiped;
        downSwipeGesture.Swiped += OnSwiped;

        grid = new Grid
        {
            WidthRequest = gridSize,
            HeightRequest = gridSize,
            Margin = -gridSize
        };

        
        grid.GestureRecognizers.Add(leftSwipeGesture);
        grid.GestureRecognizers.Add(rightSwipeGesture);
        grid.GestureRecognizers.Add(upSwipeGesture);
        grid.GestureRecognizers.Add(downSwipeGesture);

        hidden.GestureRecognizers.Add(leftSwipeGesture);
        hidden.GestureRecognizers.Add(rightSwipeGesture);
        hidden.GestureRecognizers.Add(upSwipeGesture);
        hidden.GestureRecognizers.Add(downSwipeGesture);

        backgroundGrid = new Grid
        {
            WidthRequest = gridSize,
            HeightRequest = gridSize
        };

        stackLayout.Add(backgroundGrid);
        stackLayout.Add(grid);
    }

    private void Draw(char[,] field)
    {
        for (int i = 0; i < fieldSizeX; i++)
        {
            for (int j = 0; j < fieldSizeY; j++)
            {
                if (snake._direction != lastDirection)
                {
                    images[snake._snake[1].x, snake._snake[1].y].Source = bodyrot;
                    switch (snake._direction)
                    {
                        case Snake.SnakeDirection.Left:
                            if (lastDirection == Snake.SnakeDirection.Up)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 90;
                            }
                            else if (lastDirection == Snake.SnakeDirection.Down)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 180;
                            }
                            break;
                        case Snake.SnakeDirection.Right:
                            if (lastDirection == Snake.SnakeDirection.Up)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 0;
                            }
                            else if (lastDirection == Snake.SnakeDirection.Down)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 270;
                            }
                            break;
                        case Snake.SnakeDirection.Up:
                            if (lastDirection == Snake.SnakeDirection.Left)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 270;
                            }
                            else if (lastDirection == Snake.SnakeDirection.Right)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 180;
                            }
                            break;
                        case Snake.SnakeDirection.Down:
                            if (lastDirection == Snake.SnakeDirection.Left)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 0;
                            }
                            else if (lastDirection == Snake.SnakeDirection.Right)
                            {
                                images[snake._snake[1].x, snake._snake[1].y].Rotation = 90;
                            }
                            break;
                    }
                }

                if (field[i, j] == '#')
                {
                    images[i, j].Source = head;
                    switch (snake._direction)
                    {
                        case Snake.SnakeDirection.Left:
                            if (images[i, j].Rotation != 270)
                            {
                                images[i, j].Rotation = 270;
                            }
                            break;
                        case Snake.SnakeDirection.Right:
                            if (images[i, j].Rotation != 90)
                            {
                                images[i, j].Rotation = 90;
                            }
                            break;
                        case Snake.SnakeDirection.Up:
                            if (images[i, j].Rotation != 0)
                            {
                                images[i, j].Rotation = 0;
                            }
                            break;
                        case Snake.SnakeDirection.Down:
                            if (images[i, j].Rotation != 180)
                            {
                                images[i, j].Rotation = 180;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (field[i, j] == '.' && images[i, j].Source != empty)
                {
                    images[i, j].Source = empty;
                }
                else if (field[i, j] == '*' && (i == snake._snake[snake.Size - 1].x && j == snake._snake[snake.Size - 1].y))
                {
                    if (images[i, j].Source == bodyrot)
                    {
                        switch (rotations.Dequeue())
                        {
                            case Snake.SnakeDirection.Left:
                                images[i, j].Rotation = 270;
                                break;
                            case Snake.SnakeDirection.Right:
                                images[i, j].Rotation = 90;
                                break;
                            case Snake.SnakeDirection.Up:
                                images[i, j].Rotation = 0;
                                break;
                            case Snake.SnakeDirection.Down:
                                images[i, j].Rotation = 180;
                                break;
                            default:
                                break;
                        }
                    }
                    images[i, j].Source = tale;
                }
                else if (field[i, j] == '*' && images[i, j].Source != body && images[i, j].Source != bodyrot && images[i, j].Source != tale)
                {
                    images[i, j].Source = body;
                }

                else if (field[i, j] == '$' && images[i, j].Source != apple)
                {
                    images[i, j].Source = apple;
                    images[i, j].Rotation = 0;
                }
            }
        }
        if (snake._direction != lastDirection)
        {
            rotations.Enqueue(snake._direction);
        }
        lastDirection = snake._direction;

    }

    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (directions.Count < 3)
            {
                switch (e.Direction)
                {
                    case SwipeDirection.Left:
                        directions.Enqueue(Snake.SnakeDirection.Left);
                        break;
                    case SwipeDirection.Right:
                        directions.Enqueue(Snake.SnakeDirection.Right);
                        break;
                    case SwipeDirection.Up:
                        directions.Enqueue(Snake.SnakeDirection.Up);
                        break;
                    case SwipeDirection.Down:
                        directions.Enqueue(Snake.SnakeDirection.Down);
                        break;
                }
            }
        });
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
        if (directions.Count < 3)
        {
            switch (((Button)sender).CommandParameter)
            {
                case "Left":
                    directions.Enqueue(Snake.SnakeDirection.Left);
                    break;
                case "Right":
                    directions.Enqueue(Snake.SnakeDirection.Right);
                    break;
                case "Up":
                    directions.Enqueue(Snake.SnakeDirection.Up);
                    break;
                case "Down":
                    directions.Enqueue(Snake.SnakeDirection.Down);
                    break;
            }
        }
    }
}