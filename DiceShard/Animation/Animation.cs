using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace DiceShard
{
    public class Animation
    {
        public Storyboard myStoryBoard = null;
        public bool IsRunning { get; set; } = false;
        MainWindow MyWindow = null;
        public LinearVector3DKeyFrame RandomKey2 { get; set; } = null;
        public LinearVector3DKeyFrame RandomKeyFrame { get; set; } = null;
        public LinearVector3DKeyFrame RandomKey3 { get; set; } = null;
        public DoubleAnimation RandomAngle { get; set; } = null;
        public DoubleAnimation RotateAnimation { get; set; } = null;


        public Animation()
        {
            myStoryBoard = new Storyboard();
        }
        public Animation(string StoryBoardName, MainWindow MyWindow)
        {
            this.MyWindow = MyWindow;
            myStoryBoard = (Storyboard)MyWindow.Res1.FindResource("MyStoryboard");
            myStoryBoard.Completed += Animation_Completed;

            RandomAngle = (DoubleAnimation)myStoryBoard.Children[0];
            RotateAnimation = (DoubleAnimation)myStoryBoard.Children[1];
            RandomKey2 = (LinearVector3DKeyFrame)((Vector3DAnimationUsingKeyFrames)myStoryBoard.Children[2]).KeyFrames[1];
            RandomKey3 = (LinearVector3DKeyFrame)((Vector3DAnimationUsingKeyFrames)myStoryBoard.Children[2]).KeyFrames[2];
            RandomKeyFrame = (LinearVector3DKeyFrame)((Vector3DAnimationUsingKeyFrames)myStoryBoard.Children[2]).KeyFrames[3];
        }

        public void AddRotationAnimation(double Angle, Vector3D Axis, double Seconds, double BeginTime, double Speed, string TargetName)
        {
            if (Axis != null )
            {
                    var Axisanimation = new Vector3DAnimation();
                    Axisanimation.To = Axis;
                    Axisanimation.BeginTime = TimeSpan.FromSeconds(BeginTime);
                    Axisanimation.SpeedRatio = Speed;
                    Axisanimation.Duration = TimeSpan.FromMilliseconds(0);
                    myStoryBoard.Children.Add(Axisanimation);
                    Storyboard.SetTargetName(Axisanimation, TargetName);
                    Storyboard.SetTargetProperty(Axisanimation, new PropertyPath(AxisAngleRotation3D.AxisProperty));
            }

            var animation = new DoubleAnimation();
            animation.By = Angle;
            animation.Duration = TimeSpan.FromSeconds(Seconds);
            animation.BeginTime = TimeSpan.FromSeconds(BeginTime);
            animation.SpeedRatio = Speed;
            myStoryBoard.Children.Add(animation); 
            Storyboard.SetTargetName(animation, TargetName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(AxisAngleRotation3D.AngleProperty));
        }
        public void ChangeAxisAngle(LinearVector3DKeyFrame KeyFrameObject, DoubleAnimation RotateObjectAnimation, Vector3D NewAxis, double NewAngle)
        {
            KeyFrameObject.Value = NewAxis;
            RotateObjectAnimation.To = NewAngle;
        }

        public void AddTransformAnimation(double[] offsetsXYZ, double seconds, double Speed, double BeginTimeSeconds, string TargetName)
        {
            var dependencys = new object[]
            {
                TranslateTransform3D.OffsetXProperty,
                TranslateTransform3D.OffsetYProperty,
                TranslateTransform3D.OffsetZProperty
            };

            for(short i = 0; i<3; i++)
            {
                var animation = new DoubleAnimation();
                animation.To = offsetsXYZ[i];
                animation.BeginTime = TimeSpan.FromSeconds(BeginTimeSeconds);
                animation.Duration = TimeSpan.FromSeconds(seconds);
                animation.SpeedRatio = Speed;
                myStoryBoard.Children.Add(animation);
                Storyboard.SetTargetName(animation, TargetName);
                Storyboard.SetTargetProperty(animation, new PropertyPath(dependencys[i]));
            }
        }
        private void Animation_Completed(object sender, EventArgs ea)
        {
            IsRunning = false;
        }
    }
}
