#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class GridView : ViewBase
    {
        float cellSize = 1;

        float halfCellSize = 0.5f;

        float distance = 1;

        GridViewDirection direction = GridViewDirection.Forward;

        Vector3 forward = Vector3.Forward;

        Vector3 up = Vector3.Up;

        Vector3 right = Vector3.Right;

        Position targetCellPosition;

        public float CellSize
        {
            get { return cellSize; }
            set
            {
                if (cellSize == value) return;

                cellSize = value;
                halfCellSize = cellSize * 0.5f;

                MatrixDirty = true;
            }
        }

        public float Distance
        {
            get { return distance; }
            set
            {
                if (distance == value) return;

                distance = value;
                MatrixDirty = true;
            }
        }

        public GridViewDirection Direction
        {
            get { return direction; }
            set
            {
                if (direction == value) return;

                direction = value;

                Vector3 forward;
                Vector3 up;

                if (direction == GridViewDirection.Down)
                {
                    forward = Vector3.Down;
                    up = Vector3.Backward;
                    right = Vector3.Left;
                }
                else if (direction == GridViewDirection.Up)
                {
                    forward = Vector3.Up;
                    up = Vector3.Forward;
                    right = Vector3.Left;
                }
                else
                {
                    switch (direction)
                    {
                        case GridViewDirection.Forward:
                            forward = Vector3.Forward;
                            right = Vector3.Right;
                            break;
                        case GridViewDirection.Backward:
                            forward = Vector3.Backward;
                            right = Vector3.Left;
                            break;
                        case GridViewDirection.Left:
                            forward = Vector3.Left;
                            right = Vector3.Forward;
                            break;
                        case GridViewDirection.Right:
                            forward = Vector3.Right;
                            right = Vector3.Backward;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    up = Vector3.Up;
                }


                MatrixDirty = true;
            }
        }

        public Position TargetCellPosition
        {
            get { return targetCellPosition; }
            set
            {
                if (targetCellPosition == value) return;

                targetCellPosition = value;
                MatrixDirty = true;
            }
        }

        public void MoveLeft()
        {
            targetCellPosition.X--;

            MatrixDirty = true;
        }

        public void MoveRight()
        {
            targetCellPosition.X++;

            MatrixDirty = true;
        }

        public void MoveUp()
        {
            targetCellPosition.Y++;

            MatrixDirty = true;
        }

        public void MoveDown()
        {
            targetCellPosition.Y--;

            MatrixDirty = true;
        }

        public void MoveForward()
        {
            targetCellPosition.Z--;

            MatrixDirty = true;
        }

        public void MoveBackward()
        {
            targetCellPosition.Z++;

            MatrixDirty = true;
        }

        protected override void UpdateOverride()
        {
            var targetElementCenter = new Vector3
            {
                X = targetCellPosition.X * cellSize + halfCellSize,
                Y = targetCellPosition.Y * cellSize + halfCellSize,
                Z = targetCellPosition.Z * cellSize + halfCellSize
            };
            var backward = -forward;
            var position = targetElementCenter + distance * backward;

            Vector3 target;
            Vector3.Add(ref position, ref forward, out target);

            Matrix.CreateLookAt(ref position, ref target, ref up, out Matrix);

            MatrixDirty = false;
        }
    }
}
