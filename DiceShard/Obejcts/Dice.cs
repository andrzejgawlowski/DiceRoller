using System;
using System.Windows.Media.Media3D;

namespace DiceShard
{
    class Dice
    {
        public short Number { get; set; } = 0;
        public ushort RollCount { get; set; }
        public MainWindow MyWindow { get; set; } = null;
        public Animation Animation { get; set; } = null;
        public short OldNumber { get; set; } = 0;

        public void Draw(object sender, EventArgs ea)
        {
            Animation = MyWindow.RollAnimation;

            if (Animation.IsRunning)
                Animation.myStoryBoard.SkipToFill();
            else
            {
                RollCount++;
                Animation.IsRunning = true;

                Random random = new Random();

                MyWindow.NumberLabelNew.Content = "You've drawn ";
                MyWindow.NumberLabelOld.Content = "You've drawn ";
                MyWindow.NumOfRols.Content = "Roll Counter: ";

                
                Number = (short)random.Next(1, 7); // Gets random number from (1-6)


                if (random.NextDouble() < 0.5) // 50% chance for first if and 50% for second if
                {
                    Animation.RandomKey2.Value = new Vector3D(0, 1, 1);
                    Animation.RandomKey3.Value = new Vector3D(1, 0, 1);
                }
                else
                {
                    Animation.RandomKey2.Value = new Vector3D(1, 0, 1);
                    Animation.RandomKey3.Value = new Vector3D(0, 1, 1);
                }

                Animation.RandomAngle.To = random.NextDouble() < 0.5 ? 1500 - (1500 / random.Next(2, 4)) : (1500 - (1500 / random.Next(2, 4))) * -1;

                
                MyWindow.NumOfRols.Content += RollCount.ToString();
                MyWindow.NumberLabelNew.Content += Number.ToString();
                MyWindow.NumberLabelOld.Content += OldNumber.ToString();

                
                OldNumber = Number; //Save old number to show it later in label

                switch (Number)
                {
                    case 1: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(1, 0, 0), 90); break;
                    case 2: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(0, 1, 0), 90); break;
                    case 3: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(0, 1, 0), 0); break;
                    case 4: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(0, 1, 0), 180); break;
                    case 5: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(0, 1, 0), -90); break;
                    case 6: Animation.ChangeAxisAngle(Animation.RandomKeyFrame, Animation.RotateAnimation, new Vector3D(1, 0, 0), -90); break;
                }
                Animation.myStoryBoard.Begin();
            }   
        }
        
    }
}
