using System;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace DiceShard
{
    class Object3D : ModelVisual3D
    {
        public RotateTransform3D rotateModel = null;
        public TranslateTransform3D translateModel;
        MainWindow window = null;
        public string Name { get; set; } = string.Empty;

        public Object3D(string pathTo3DModel, string modelName, MainWindow window)
        {
            try
            {
                var myModel = window.Model1;
                Name = nameof(window.Model1);
                myModel.Content = new ModelImporter().Load(pathTo3DModel);
                this.window = window;
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show($"Can't create 3DModel or other model error: {e.Message}", "Program");
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show($"Invalid path or corrupted file {e.Message}", "Program");
            }
        }
        public void AddToTranform3DGroup(object[] transformsArray)
        {
            var transformGroup = new Transform3DGroup();
            foreach (var transform in transformsArray)
            {
                transformGroup.Children.Add((Transform3D)transform);
            }
            Transform = transformGroup;
        }
        public void AddRotationModel(string objectName, Vector3D axis, double startAngle)
        {
            var rotateObject = new AxisAngleRotation3D()
            {
                Axis = axis,
                Angle = startAngle
            };
            window.RegisterName(objectName, rotateObject);
            rotateModel = new RotateTransform3D()
            {
                Rotation = rotateObject
            };
            window.RegisterName("Rotate", rotateModel);
        }
        public void AddTranslateModel(string objectName, Vector3D offsetsXYZ)
        {
            translateModel = new TranslateTransform3D(offsetsXYZ);
            window.RegisterName(objectName, translateModel);
        }
    }
}
