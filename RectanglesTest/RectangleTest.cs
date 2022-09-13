using NUnit.Framework;
using WindowsFormsApp1;
using System.Drawing;
using System;

namespace RectanglesTest
{
    public class RectangleTest
    {
        RectangleManager recManager;
        Graphics gp;
        Pen pnLine = new Pen(Brushes.Black, (float)0.5);

        [SetUp]
        public void Setup()
        {
            var lines = 5;
            if (lines >= 5 && lines <= 25)
            {
                Font fnt = new Font("Arial", 10);
                float x = 0f;
                float y = 0f;
                float panelWidth = 1244;
                float panelHeight = 484;
                float xspace = panelWidth / lines;
                float yspace = panelHeight / lines;
                recManager = new RectangleManager(gp, lines, panelWidth, panelHeight);

                for (int i = 0; i < (lines + 1); i++)
                {
                    if (gp != null)//for testing purposes only
                    {
                        gp.DrawLine(pnLine, x, 0, x, panelHeight);
                        x += xspace;
                    }
                }
                x = 0f;
                for (int i = 0; i < (lines + 1); i++)
                {
                    if (gp != null)//for testing purposes only
                    {
                        gp.DrawLine(pnLine, 0, y, panelWidth, y);
                        y += yspace;
                    }
                }
                x = 0f;
                y = 0f;
                int counter = 1;
                int multiplier = 0;
                int maxWidth = lines;
                for (int i = 0; i < lines; i++)
                {
                    for (int j = 0; j < lines; j++)
                    {
                        if (gp != null)//for testing purposes only
                        {
                            gp.DrawString(counter.ToString(), new Font("Arial", 12), Brushes.Black, x, y);
                        }
                        recManager.AddCoordinate(new Coordinate() { ID = (counter - 1), X = x, Y = (multiplier * yspace), MaxWidth = maxWidth });
                        x += xspace;
                        counter++;
                    }
                    y += yspace;
                    x = 0f;
                    multiplier++;
                    maxWidth = counter + lines;
                }
            }
        }

        [Test]
        public void RectangleManager_CreateRectangle_GivenValidParameter_ShouldCreateRectangle()
        {
            int location = 2;
            int widthCellSize = 2;
            int heightCellSize = 3;

            var recResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);

            Assert.IsTrue(recResponse.IsSuccessful);
        }

        [Test]
        [TestCase(2, 2, 3)]
        [TestCase(7, 2, 3)]
        public void RectangleManager_CreateRectangle_GivenOverlapParameter_ShouldThrowOverlapError(int location, int widthCellSize, int heightCellSize)
        {
            var firstRecResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);
            var secondRecResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);

            Assert.IsTrue(firstRecResponse.IsSuccessful);
            Assert.IsFalse(secondRecResponse.IsSuccessful);
            Assert.AreEqual("Rectangle will overlap", secondRecResponse.Message);
        }

        [Test]
        [TestCase(15, 2, 3)]
        [TestCase(22, 2, 3)]
        public void RectangleManager_CreateRectangle_GivenOutOfBoundsParameter_ShouldThrowOutOfBoundsError(int location, int widthCellSize, int heightCellSize)
        {
            var firstRecResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);
            var secondRecResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);

            Assert.IsFalse(firstRecResponse.IsSuccessful);
            Assert.IsFalse(secondRecResponse.IsSuccessful);
            Assert.AreEqual("Rectangle will be out of bounds", secondRecResponse.Message);
        }

        [Test]
        public void RectangleManager_CreateRectangle_GivenNotOverlappingParameter_ShouldCreateTwoRectangle()
        {
            int location = 2;
            int widthCellSize = 2;
            int heightCellSize = 2;

            int location2 = 12;
            int widthCellSize2 = 2;
            int heightCellSize2 = 2;

            var firstRecResponse = recManager.CreateRectangle(location, widthCellSize, heightCellSize);
            var secondRecResponse = recManager.CreateRectangle(location2, widthCellSize2, heightCellSize2);

            Assert.IsTrue(firstRecResponse.IsSuccessful);
            Assert.IsTrue(secondRecResponse.IsSuccessful);
        }
    }
}