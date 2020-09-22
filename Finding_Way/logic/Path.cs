using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finding_Way
{
    class Path
    {
        public int agentCoordinateX = 0;
        public int agentCoordinateY = 0;
        public List<Sides> steps = new List<Sides>();

        public Path(int x, int y, Sides side)
        {
            agentCoordinateX = x;
            agentCoordinateY = y;

            AddingStep(side);
        }

        // конструктор добавляющий в новый Path список шагов (сторон) из прошлого Path (который передается в конструктор)
        public Path(Path path)
        {
            this.agentCoordinateX = path.agentCoordinateX;
            this.agentCoordinateY = path.agentCoordinateY;

            for (int i = 0; i < path.steps.Count; i++)
            {
                this.steps.Add(path.steps[i]);
            }
        }

        public Path(int x, int y)
        {
            agentCoordinateX = x;
            agentCoordinateY = y;
        }

        // метод дабавляющий в список сторон еще одну сторону
        public void AddingStep(Sides side)
        {
            steps.Add(side);

            switch (side)
            {
                case Sides.up:
                    agentCoordinateY--;
                    break;
                case Sides.left:
                    agentCoordinateX--;
                    break;
                case Sides.down:
                    agentCoordinateY++;
                    break;
                case Sides.right:
                    agentCoordinateX++;
                    break;
            }
        }
    }
}
