internal class Game
{
    private readonly IReadWrite _io;
    private readonly Board _board;

    public Game(IReadWrite io)
    {
        _io = io;
        _board = new Board();
    }

    public void Play()
    {
        _io.Write(Streams.Title);

        for (var player = 1; player <= 2; player++)
        {
            _io.WriteLine(Formats.InitialPieces, player);
            for (var i = 1; i <= 3; i++)
            {
                _board[_io.ReadCoordinates(_board)] = player == 1 ? Piece.NewPlayer1() : Piece.NewPlayer2();
            }
        }

        _board.CountNeighbours();
        _board.Display(_io);

        while(true)
        {
            _board.CalculateNextGeneration();
            _io.WriteLine();
            _board.Display(_io);

            if (_board.Result is not null) { break; }

            var player1Coordinate = _io.ReadCoordinates(1, _board);
            var player2Coordinate = _io.ReadCoordinates(2, _board);

            if (player1Coordinate == player2Coordinate)
            {
                _io.Write(Streams.SameCoords);
                // This is a bug existing in the original code. The line should be _board[_coordinates[_player]] = 0;
                _board.ClearCell(player1Coordinate + 1);
            }
            else
            {
                _board.AddPlayer1Piece(player1Coordinate);
                _board.AddPlayer2Piece(player2Coordinate);
            }
        }

        _io.WriteLine(_board.Result);
    }
}
