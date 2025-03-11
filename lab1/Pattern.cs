using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lab1
{
    // Общий интерфейс для всех элементов, которые можно рисовать
    public interface IDrawable
    {
        void Draw(Graphics g, Point location);

    }

    // Интерфейс для фигур
    public interface IShape : IDrawable
    {
        void Draw(Graphics g, Point location);
    }

    // Интерфейс для стрелок
    public interface IArrow : IDrawable
    {
        void Draw(Graphics g, Point start, Point end);
    }

    // Конкретные фигуры
    public class Rectangle : IShape
    {
        private Pen _pen;

        public Rectangle(Pen pen)
        {
            _pen = pen;
        }

        public void Draw(Graphics g, Point location)
        {
            g.DrawRectangle(_pen, location.X, location.Y, 100, 60); 
        }
    }

    public class Oval : IShape
    {
        private Pen _pen;

        public Oval(Pen pen)
        {
            _pen = pen;
        }

        public void Draw(Graphics g, Point location)
        {
            g.DrawEllipse(_pen, location.X, location.Y, 100, 60); 
        }
    }

    // Конкретные стрелки
    public class SolidArrow : IArrow
    {
        private Pen _pen;

        public SolidArrow(Pen pen)
        {
            _pen = pen;
        }
        public void Draw(Graphics g, Point location)
        {
            // Фиксируем конечную точку стрелки (например, на 100 пикселей правее)
            Point end = new Point(location.X + 100, location.Y);
            Draw(g, location, end);
        }


        public void Draw(Graphics g, Point start, Point end)
        {
            // Укорачиваем линию стрелки, чтобы она не заходила в наконечник
            float arrowSize = 10; // Размер наконечника
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            Point shortenedEnd = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle)),
                (int)(end.Y - arrowSize * Math.Sin(angle))
            );

            // Рисуем линию стрелки
            g.DrawLine(_pen, start, shortenedEnd);

            // Рисуем наконечник стрелки (только контур)
            DrawArrowhead(g, shortenedEnd, end);
        }

        private void DrawArrowhead(Graphics g, Point start, Point end)
        {
            float arrowSize = 10;
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            PointF[] arrowPoints = new PointF[3];
            arrowPoints[0] = end;
            arrowPoints[1] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6)
            );
            arrowPoints[2] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6)
            );

            g.DrawPolygon(_pen, arrowPoints); // Заливаем наконечник
        }
    }

    public class DashedArrow : IArrow
    {
        private Pen _pen;

        public DashedArrow(Pen pen)
        {
            _pen = pen;
            _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
   
        }
        public void Draw(Graphics g, Point location)
        {
            // Фиксируем конечную точку стрелки (например, на 100 пикселей правее)
            Point end = new Point(location.X + 100, location.Y);
            Draw(g, location, end);
        }
        // Реализация метода из IArrow
        public void Draw(Graphics g, Point start, Point end)
        {  // Укорачиваем линию стрелки, чтобы она не заходила в наконечник
            float arrowSize = 10; // Размер наконечника
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            Point shortenedEnd = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle)),
                (int)(end.Y - arrowSize * Math.Sin(angle))
            );

            // Рисуем линию стрелки
            g.DrawLine(_pen, start, shortenedEnd);

            // Рисуем наконечник стрелки (только контур)
              DrawArrowhead(g, shortenedEnd, end);
        }

        private void DrawArrowhead(Graphics g, Point start, Point end)
        {
            float arrowSize = 10; 
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            PointF[] arrowPoints = new PointF[3];
            arrowPoints[0] = end;
            arrowPoints[1] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6)
            );
            arrowPoints[2] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6)
            );

            g.DrawPolygon(_pen, arrowPoints);
        }

    }

    // Интерфейс для фабрик
    public interface IShapeFactory
    {
        IShape CreateRectangle();
        IShape CreateOval();
        IArrow CreateSolidArrow();
        IArrow CreateDashedArrow();
    }

    // Фабрика для красных элементов
    public class RedFactory : IShapeFactory
    {
        public IShape CreateRectangle()
        {
            return new Rectangle(new Pen(Color.Red, 3));
        }

        public IShape CreateOval()
        {
            return new Oval(new Pen(Color.Red, 3));
        }

        public IArrow CreateSolidArrow()
        {
            return new SolidArrow(new Pen(Color.Red, 3));
        }

        public IArrow CreateDashedArrow()
        {
            return new DashedArrow(new Pen(Color.Red, 3));
        }
    }

    // Фабрика для черных элементов
    public class BlackFactory : IShapeFactory
    {
        public IShape CreateRectangle()
        {
            return new Rectangle(new Pen(Color.Black, 3));
        }

        public IShape CreateOval()
        {
            return new Oval(new Pen(Color.Black, 3));
        }

        public IArrow CreateSolidArrow()
        {
            return new SolidArrow(new Pen(Color.Black, 3));
        }

        public IArrow CreateDashedArrow()
        {
            return new DashedArrow(new Pen(Color.Black, 3));
        }
    }
    public class DrawableWrapper
    {
        public IDrawable Element { get; set; }
        public Point Location { get; set; }
        public bool IsSelected { get; set; }
        public float RotationAngle { get; set; }
        public string Text { get; set; }

        public DrawableWrapper(IDrawable element, Point location)
        {
            Element = element;
            Location = location;
            IsSelected = false;
            RotationAngle = 0;
            Text = "";
        }

        public void Draw(Graphics g)
        {
            // Сохраняем текущее состояние Graphics
            var state = g.Save();

            // Применяем поворот
            g.TranslateTransform(Location.X + 50, Location.Y + 30); // Центр фигуры
            g.RotateTransform(RotationAngle);
            g.TranslateTransform(-(Location.X + 50), -(Location.Y + 30));

            // Рисуем элемент
            Element.Draw(g, Location);

            // Рисуем текст внутри фигуры
            if (!string.IsNullOrEmpty(Text))
            {
                var font = new Font("Arial", 12);
                var textSize = g.MeasureString(Text, font);
                var textLocation = new PointF(
                    Location.X + (100 - textSize.Width) / 2,
                    Location.Y + (60 - textSize.Height) / 2
                );
                g.DrawString(Text, font, Brushes.Black, textLocation);
            }

            // Восстанавливаем состояние Graphics
            g.Restore(state);

            if (IsSelected)
            {
                g.DrawRectangle(Pens.Black, Location.X - 5, Location.Y - 5, 110, 70); // Выделение элемента
            }
        }
    }
}
