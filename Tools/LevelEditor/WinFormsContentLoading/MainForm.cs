#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;
#endregion

namespace WinFormsContentLoading
{
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to fill the entire form with a
    /// ModelViewerControl, except for the menu bar which provides the
    /// "File / Open..." option.
    /// </summary>
    public partial class MainForm : Form
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;


        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            contentBuilder = new ContentBuilder();

            contentManager = new ContentManager(modelViewerControl.Services,
                                                contentBuilder.OutputDirectory);

            /// Automatically bring up the "Load Model" dialog when we are first shown.
            this.Shown += OpenMenuClicked;
        }


        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Event handler for the Open menu option.
        /// </summary>
        void OpenMenuClicked(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.


            fileDialog.Title = "Load Model";

            fileDialog.Filter = "Model Files (*.fbx;*.x)|*.fbx;*.x|" +
                                "FBX Files (*.fbx)|*.fbx|" +
                                "X Files (*.x)|*.x|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadModel(fileDialog.FileName);
            }
        }


        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        void LoadModel(string fileName)
        {
            Cursor = Cursors.WaitCursor;

            // Unload any existing model.
            modelViewerControl.Model = null;
            contentManager.Unload();

            // Tell the ContentBuilder what to build.
            contentBuilder.Clear();
            contentBuilder.Add(fileName, "Model", null, "ModelProcessor");

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                modelViewerControl.Model = contentManager.Load<Model>("Model");
            }
            else
            {
                // If the build failed, display an error message.
                MessageBox.Show(buildError, "Error");
            }

            Cursor = Cursors.Arrow;
        }

        private void modelViewerControl_Click(object sender, EventArgs e)
        {

        }

        Vector2 mouseDownZero = new Vector2();
        Vector2 setRotation = new Vector2();
        private void modelViewerControl_MouseDown(object sender, MouseEventArgs e)
        {
            if(mouseDownZero == Vector2.Zero)
            {
                mouseDownZero.X = e.X;
                mouseDownZero.Y = e.Y;
            }
        }

        private void modelViewerControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownZero = Vector2.Zero;

            // Save current rotation of the model 
            setRotation.X = modelViewerControl.RotX;
            setRotation.Y = modelViewerControl.RotY;
        }

        private void modelViewerControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (modelViewerControl != null && mouseDownZero != Vector2.Zero)
            {
                float unitX = e.X - mouseDownZero.X;
                float unitY = e.Y - mouseDownZero.Y;

                Vector2 changeVector = new Vector2(unitY, unitX);
                if (changeVector != Vector2.Zero)
                {
                    changeVector *= 0.01f;
                    changeVector += setRotation;
                    modelViewerControl.RotX = changeVector.X;
                    modelViewerControl.RotY = changeVector.Y;
                }
            }
        }

        Vector3 posChange = new Vector3();
        private void modelViewerControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.S)
            {
                posChange.Z = -1;
            }
            if (e.KeyData == Keys.W)
            {
                posChange.Z = 1;
            }
            if (e.KeyData == Keys.A)
            {
                posChange.X = -1;
            }
            if (e.KeyData == Keys.D)
            {
                posChange.X = 1;
            }
            if (e.KeyData == Keys.Q)
            {
                posChange.Y = -1;
            }
            if (e.KeyData == Keys.E)
            {
                posChange.Y = 1;
            }
        }

        private void modelViewerControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            posChange *= 5;
            char key = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToUpper(e.KeyChar);
            if (key == Keys.S.ToChar() || key == Keys.W.ToChar())
            {
                this.modelViewerControl.PosZ += (int)posChange.Z;
            }
            if (key == Keys.A.ToChar() || key == Keys.D.ToChar())
            {
                this.modelViewerControl.PosX += (int)posChange.X;
            }
            if (key == Keys.Q.ToChar() || key == Keys.E.ToChar())
            {
                this.modelViewerControl.PosY += (int)posChange.Y;
            }
        }

        private void modelViewerControl_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
