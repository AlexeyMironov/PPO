unit Clock;

interface

uses
  System.SysUtils, System.Classes, Vcl.Controls, Vcl.ExtCtrls, Graphics;

type
  TCLock = class(TImage)
  private
    { Private declarations }
    timer: TTimer;
    angle: real;
    sx1, sy1, mx1, my1, hx1, hy1: integer;
    hour, min, sec, oth: word;
    procedure TimerHandler(Sender: TObject);
    procedure DrawBase();
  protected
    { Protected declarations }
  public
    constructor Create(AOwner: TComponent); override;
  published
    { Published declarations }
  end;

procedure Register;

implementation

const
  BackColor = $00F5D3C0;
  ForeColor = clSkyBlue;
  ident = 20;
  cHeight = 250;
  cWidth = 250;

procedure Register;
begin
  RegisterComponents('PPO', [TCLock]);
end;

constructor TCLock.Create(AOwner: TComponent);
begin
  inherited Create(AOwner);

  AutoSize := false;
  Height := cHeight;
  Width := cWidth;
  Font.Color := clBlue;
  Font.Style := [fsBold];
  Font.Height := 16;
  Font.Size := 12;

  timer := TTimer.Create(self);
  timer.Interval := 1000;
  timer.Enabled := true;
  timer.OnTimer := TimerHandler;

  angle := 0;

  DrawBase();
  TimerHandler(nil);
end;

procedure TCLock.DrawBase();
var
  i, x, y, rad: integer;
  a: real;
begin

  Canvas.Font.Name := 'Arial';
  Canvas.Brush.Color := BackColor;
  Canvas.FillRect(Rect(0, 0, cWidth, cHeight));

  hx1 := cWidth div 2;
  hy1 := hx1;
  mx1 := hy1;
  my1 := mx1;
  sx1 := my1;
  sy1 := sx1;
  // Разметка циферблата
  for i := 1 to 12 do
  begin
    rad := cWidth div 2;
    a := pi / 180 - (((pi / 180) * 30) * i);
    x := Round((rad - ident) * sin(a)) + rad;
    y := Round((rad - ident) * cos(a)) + rad;
    x := (rad * 2) - x;
    y := (rad * 2) - y;
    case i of
      12:
        begin
          y := y - 15
        end;
      2:
        begin
          x := x + 10
        end;
      1:
        begin
          x := x + 15;
          y := y - 5
        end;
      3:
        begin
          x := x + 3
        end;
      6:
        begin
          y := y + 3;
          x := x - 3;
        end;
      7:
        begin
          y := y + 5
        end;
      8:
        begin
          x := x - 7
        end;
      9:
        begin
          x := x - 10;
          y := y - 10
        end;
      10:
        begin
          y := y - 10;
          x := x - 15
        end;
      11:
        begin
          y := y - 10;
          x := x - 15
        end;
    end;
    Canvas.TextOut(x, y, IntToStr(i));
  end;
  Canvas.Brush.Color := ForeColor;
  Canvas.Ellipse(ident, ident, cWidth - ident, cHeight - ident);
end;

procedure TCLock.TimerHandler(Sender: TObject);
var
  rad, x1, y1: integer;
  ah, am, asec: real;
begin
  rad := (cWidth div 2);
  // Смотрим дату и время
  DecodeTime(Now, hour, min, sec, oth);
  // Определяем углы
  ah := pi / 180 - (((pi / 180) * 30) * hour);
  am := pi / 180 - (((pi / 180) * 6) * min);
  asec := pi / 180 - (((pi / 180) * 6) * sec);
  // Часы
  Canvas.MoveTo(rad, rad);
  Canvas.Pen.Color := ForeColor;
  Canvas.Pen.Width := 5;
  Canvas.LineTo(hx1, hy1);
  Canvas.Pen.Color := clBlack;
  Canvas.MoveTo(rad, rad);
  // Радиус умножить на sin(x), округлить и прибавить к координате
  x1 := Round((rad - 40 - ident) * sin(ah)) + rad;
  y1 := Round((rad - 40 - ident) * cos(ah)) + rad;
  y1 := (rad * 2) - y1;
  Canvas.LineTo(x1, y1);
  hx1 := x1;
  hy1 := y1;

  // Минуты
  Canvas.MoveTo(rad, rad);
  Canvas.Pen.Color := ForeColor;
  Canvas.Pen.Width := 2;
  Canvas.LineTo(mx1, my1);
  Canvas.Pen.Color := clBlack;
  Canvas.MoveTo(rad, rad);
  // Радиус умножить на sin(x), округлить и прибавить к координате
  x1 := Round((rad - 10 - ident) * sin(am)) + rad;
  y1 := Round((rad - 10 - ident) * cos(am)) + rad;

  x1 := (rad * 2) - x1;
  y1 := (rad * 2) - y1;

  mx1 := x1;
  my1 := y1;
  Canvas.LineTo(x1, y1);
  // Секунды
  Canvas.MoveTo(rad, rad);
  Canvas.Pen.Color := ForeColor;
  Canvas.Pen.Width := 1;
  Canvas.LineTo(sx1, sy1);
  Canvas.Pen.Color := clRed;
  Canvas.MoveTo(rad, rad);
  // Радиус умножить на sin(x), округлить и прибавить к координате
  x1 := Round((rad - 2 - ident) * sin(asec)) + rad;
  y1 := Round((rad - 2 - ident) * cos(asec)) + rad;

  x1 := (rad * 2) - x1;
  y1 := (rad * 2) - y1;
  Canvas.LineTo(x1, y1);
  sx1 := x1;
  sy1 := y1;
end;

end.
