using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool isCut = false;
        public Button LastClickFigure;
        public Button PrevClick;
        public int turn = 1;
        Image whiteFigure;
        Image blackFigure;
        public const int CellSize = 100;
        public const int MapSize = 8;
        public int[,] map = new int[MapSize, MapSize];
        public int[,] CutMap = new int[MapSize, MapSize];
        int IndexCut = 0;
        bool isFind = false;
        bool Cut = false;
        int IndexNextPlayer = 0;
        public Form1()
        {
            InitializeComponent();
            whiteFigure = new Bitmap(@"c:\Users\user\Desktop\white.webp");
            blackFigure = new Bitmap((@"C:\Users\User\Desktop\black.webp"));
            Init();
        }
        public void Init()
        {
            map = new int[MapSize, MapSize]
            {
                { 0 , 1 , 0 , 1 , 0 , 1 , 0 , 1 },
                { 1 , 0 , 1 , 0 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 0 , 1 , 0 , 1 },
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
                { 2 , 0 , 2 , 0 , 2 , 0 , 2 , 0 },
                { 0 , 2 , 0 , 2 , 0 , 2 , 0 , 2 },
                { 2 , 0 , 2 , 0 , 2 , 0 , 2 , 0 },
            };
            CutMap = new int[MapSize, MapSize]
          {
                { 0 , 1 , 0 , 1 , 0 , 1 , 0 , 1 },
                { 1 , 0 , 1 , 0 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 0 , 1 , 0 , 1 },
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
                { 2 , 0 , 2 , 0 , 2 , 0 , 2 , 0 },
                { 0 , 2 , 0 , 2 , 0 , 2 , 0 , 2 },
                { 2 , 0 , 2 , 0 , 2 , 0 , 2 , 0 },
          };
            CreateMap();
        }
        public void CreateMap()
        {
            this.Width = MapSize * CellSize;
            this.Height = MapSize * CellSize;
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * CellSize, i * CellSize);
                    button.Height = CellSize;
                    button.Width = CellSize;
                    this.Controls.Add(button);
                    if (map[i, j] == 1)
                    {
                        button.Image = blackFigure;
                    }
                    else if (map[i, j] == 2)
                    {
                        button.Image = whiteFigure;
                    }
                    button.BackColor = (i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0) ? Color.Gray : Color.WhiteSmoke;
                    button.Click += ClickEvent;
                    button.Click += CutEvent;
                    button.Click += CutEvent;
                }
            }
        }
        public void ClickAndPaint(Button _ClickBtn, int NextPositionX, int NextPositionY, int _currentPlayer, int FigurePosition, Color color)
        {
            if (turn == _currentPlayer)
            {
                if (map[_ClickBtn.Location.Y / CellSize, _ClickBtn.Location.X / CellSize] == FigurePosition)
                {
                    _ClickBtn.BackColor = color;
                    if (NextPositionX >= 0 && NextPositionX <= 7)
                    {
                        if (map[NextPositionY, NextPositionX] == 0) {
                            GetButtonAt(NextPositionX * CellSize, NextPositionY * CellSize).BackColor = color;
                            LastClickFigure = _ClickBtn;
                        }
                    }
                }
            }
        }
        public void MoveFigure(Button _ClickBtn, Color color, int CurrentPl, Image figure, int FigureNumber)
        {
            if (_ClickBtn.BackColor == color && _ClickBtn.Image == null && turn == CurrentPl && LastClickFigure != null)
            {
                LastClickFigure.Image = null;
                _ClickBtn.Image = figure;
                map[_ClickBtn.Location.Y / CellSize, _ClickBtn.Location.X / CellSize] = FigureNumber; map[LastClickFigure.Location.Y / CellSize, LastClickFigure.Location.X / CellSize] = 0;
                CutMap[_ClickBtn.Location.Y / CellSize, _ClickBtn.Location.X / CellSize] = FigureNumber;CutMap[LastClickFigure.Location.Y / CellSize, LastClickFigure.Location.X / CellSize] = 0;
                ResetMapColor(); CurrentP();
            }
        }
        public void ClickEvent(object sender, EventArgs e)
        {
            Button ClickBtn = sender as Button;
            if ((ClickBtn.Image != null || ClickBtn.BackColor == Color.WhiteSmoke || ClickBtn.BackColor == Color.Gray) && !isCut) 
                ResetMapColor();
            if (!isCut)
            {
                ClickAndPaint(ClickBtn, (ClickBtn.Location.X - 100) / CellSize, (ClickBtn.Location.Y - 100) / CellSize, 2, 2, Color.Yellow); ClickAndPaint(ClickBtn, (ClickBtn.Location.X + 100) / CellSize, (ClickBtn.Location.Y - 100) / CellSize, 2, 2, Color.Yellow);
                ClickAndPaint(ClickBtn, (ClickBtn.Location.X - 100) / CellSize, (ClickBtn.Location.Y + 100) / CellSize, 1, 1, Color.YellowGreen); ClickAndPaint(ClickBtn, (ClickBtn.Location.X + 100) / CellSize, (ClickBtn.Location.Y + 100) / CellSize, 1, 1, Color.YellowGreen);
                MoveFigure(ClickBtn, Color.YellowGreen, 1, blackFigure, 1);MoveFigure(ClickBtn, Color.Yellow, 2, whiteFigure, 2);
                LastClickFigure = ClickBtn;
            } 
        }
         public void CutEvent(object sender, EventArgs e)
        {
           
            Button ClickBtn = sender as Button;
            foreach (Control control in Controls)
            {
                if (control is Button button)
                { 
                    int MyPosX = button.Location.X / CellSize;
                    int MyPosY = button.Location.Y / CellSize;
                    FindCut(button, ClickBtn, 2, 1, MyPosX, MyPosY, MyPosX + 1, MyPosY + 1, MyPosX + 2, MyPosY + 2, Color.Beige, Color.Coral, GetFigure());FindCut(button, ClickBtn, 2, 1, MyPosX, MyPosY, MyPosX - 1, MyPosY - 1, MyPosX - 2, MyPosY - 2, Color.Beige, Color.Coral, GetFigure());
                    FindCut(button, ClickBtn, 2, 1, MyPosX, MyPosY, MyPosX - 1, MyPosY + 1, MyPosX - 2, MyPosY + 2, Color.Beige, Color.Coral, GetFigure());FindCut(button, ClickBtn, 2, 1, MyPosX, MyPosY, MyPosX + 1, MyPosY - 1, MyPosX + 2, MyPosY - 2, Color.Beige, Color.Coral, GetFigure());

                    FindCut(button, ClickBtn, 1, 2, MyPosX, MyPosY, MyPosX + 1, MyPosY + 1, MyPosX + 2, MyPosY + 2, Color.Beige, Color.Coral, GetFigure());FindCut(button, ClickBtn, 1, 2, MyPosX, MyPosY, MyPosX - 1, MyPosY - 1, MyPosX - 2, MyPosY - 2, Color.Beige, Color.Coral, GetFigure());
                    FindCut(button, ClickBtn, 1, 2, MyPosX, MyPosY, MyPosX - 1, MyPosY + 1, MyPosX - 2, MyPosY + 2, Color.Beige, Color.Coral, GetFigure());FindCut(button, ClickBtn, 1, 2, MyPosX, MyPosY, MyPosX + 1, MyPosY - 1, MyPosX + 2, MyPosY - 2, Color.Beige, Color.Coral, GetFigure());
                }
            }
            isFind = false;
            Cut = false;
            PrevClick = ClickBtn;
        }
        public void FindCut(Button button, Button ClickBtn, int MyTurn, int OpTurn, int MyPosX, int MyPosY, int OpPosX, int OpPosY, int EmpPosX, int EmpPosY, Color EmptyColor, Color MyColor, Image Figure)
        {
            if (MyTurn == turn && !Cut)
            {
                if (OpPosY > 0 && OpPosX > 0 && OpPosX < 7 && OpPosY < 7 && EmpPosX >= 0 && EmpPosX <= 7 && EmpPosY >= 0 && EmpPosY <= 7) 
                {
                    if (map[MyPosY, MyPosX] == MyTurn && map[OpPosY, OpPosX] == OpTurn && map[EmpPosY,EmpPosX] == 0 && (IndexCut == 0 || CutMap[MyPosY,MyPosX] == 'C'))
                    {
                        isCut = true;
                        GetCutPaint(ClickBtn, EmpPosX, EmpPosY, MyPosX, MyPosY, MyColor, EmptyColor);
                        CutFigure(ClickBtn, MyTurn, MyPosX, MyPosY, OpPosX, OpPosY, EmpPosX, EmpPosY);
                       if (CutMap[EmpPosY, EmpPosX] == 'C')
                        GetNextCurrentPlayer(MyTurn, OpTurn);
                    }
                }
            }
        }
        public void CutFigure(Button ClickBtn, int MyTurn, int MyPosX, int MyPosY, int OpPosX, int OpPosY, int EmpPosX, int EmpPosY)
        {
            if (PrevClick == GetButtonAt(MyPosX * CellSize, MyPosY * CellSize) && ClickBtn == GetButtonAt(EmpPosX * CellSize, EmpPosY * CellSize))
            {
                Cut = true;
                GetButtonAt(MyPosX * CellSize, MyPosY * CellSize).Image = null;GetButtonAt(EmpPosX * CellSize, EmpPosY * CellSize).Image = GetFigure();GetButtonAt(OpPosX * CellSize, OpPosY * CellSize).Image = null;
                ResetMapColor();
                CutMap[EmpPosY, EmpPosX] = 'C';
                map[EmpPosY, EmpPosX] = MyTurn;map[OpPosY, OpPosX] = 0;map[MyPosY, MyPosX] = 0;
                IndexCut++;
            }
        }
        public void GetNextCurrentPlayer(int MyTurn, int OpTurn)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    int MyPosY = button.Location.Y / CellSize; int MyPosX = button.Location.X / CellSize;
                    Player(MyPosX, MyPosY, MyPosX - 1, MyPosY - 1, MyPosX - 2, MyPosY - 2, MyTurn, OpTurn);Player(MyPosX, MyPosY, MyPosX - 1, MyPosY + 1, MyPosX - 2, MyPosY + 2, MyTurn, OpTurn);
                    Player(MyPosX, MyPosY, MyPosX + 1, MyPosY - 1, MyPosX + 2, MyPosY - 2, MyTurn, OpTurn);Player(MyPosX, MyPosY, MyPosX + 1, MyPosY + 1, MyPosX + 2, MyPosY + 2, MyTurn, OpTurn);
                }
            }
            if (IndexNextPlayer  ==  0)
            {
                NextAfterCut();FindC(MyTurn);
            }
            else
            {
                IndexNextPlayer = 0;
            }
        }
        public void FindC(int Myturn)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    if (CutMap[button.Location.Y / CellSize, button.Location.X / CellSize] == 'C')
                        CutMap[button.Location.Y / CellSize, button.Location.X / CellSize] = Myturn;
                }
            }
        }
        public void NextAfterCut()
        {
            IndexCut = 0;Cut = false;CurrentP();ResetMapColor();isFind = false;isCut = false;
        }
        public void Player(int MyPosX, int MyPosY,int OpPosX, int OpPosY, int EmpPosX, int EmpPosY, int MyTurn, int OpTurn)
        {
            if (CutMap[MyPosY, MyPosX] == 'C' && MyTurn == turn && Cut)
            {
                if (OpPosY > 0 && OpPosX > 0 && OpPosX < 7 && OpPosY < 7 && EmpPosX >= 0 && EmpPosX <= 7 && EmpPosY >= 0 && EmpPosY <= 7)
                {
                    if (map[MyPosY, MyPosX] == MyTurn && map[OpPosY, OpPosX] == OpTurn && map[EmpPosY, EmpPosX] == 0)
                    {
                        IndexNextPlayer++;
                    }
                }
            }
        }
        public void GetCutPaint(Button ClickBtn,int EmpPosX,int EmpPosY,int MyPosX,int MyPosY,Color MyColor,Color EmptyColor)
        {
            if (!isFind)
            {
                GetButtonAt(EmpPosX * CellSize, EmpPosY * CellSize).BackColor = EmptyColor;
            }
            if (ClickBtn == GetButtonAt(MyPosX * CellSize, MyPosY * CellSize))
            {
                isFind = true;
                ResetMapColor();
                GetButtonAt(EmpPosX * CellSize, EmpPosY * CellSize).BackColor = EmptyColor;
                ClickBtn.BackColor = MyColor;
            }
            else
            {
                GetButtonAt(MyPosX * CellSize, MyPosY * CellSize).BackColor = Color.Gray;
            }
        }

public Image GetFigure()
        {
            if(turn == 1) {
                return blackFigure;
            }
                return whiteFigure;
        }
        public void CurrentP()
        {
            turn = (turn == 1) ? 2 : 1;
        }
        public void ResetMapColor()
        {
            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    if (button.BackColor != Color.WhiteSmoke)
                        button.BackColor = Color.Gray;
                }
            }
        }
        public Button GetButtonAt(int x, int y)
        {
            string buttonName = "button" + x + "_" + y;
            foreach (Control control in Controls)
            {
                if (control is Button button && control.Location.X == x && control.Location.Y == y)
                    return button;
            }
            return null;
        }
    }
}

