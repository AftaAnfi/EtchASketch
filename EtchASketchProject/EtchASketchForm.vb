Option Explicit On
Option Strict On

'Aftanom Anfilofieff
'RCET0265
'Spring 2021
'Etch A Sketch
'https://github.com/AftaAnfi/EtchASketch.git

Public Class EtchASketchForm
    Dim penColor As Color = Color.Black
    Dim penWidth As Integer = 0

    Sub DrawLine(oldX As Integer, oldY As Integer, newX As Integer, newY As Integer, _penColor As Color, Optional _penWidth As Integer = 1)
        'initialize the graphics and pen
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim pen As New Pen(_penColor, _penWidth)

        'draw a line
        g.DrawLine(pen, oldX, oldY, newX, newY)

        'close / dispose of both graphics and pen
        pen.Dispose()
        g.Dispose()
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click, ExitToolStripMenuItem.Click, ExitToolStripMenuItem1.Click
        'Close form
        Me.Close()
    End Sub

    Private Sub ColorButton_Click(sender As Object, e As EventArgs) Handles SelectColorButton.Click, SelectColorToolStripMenuItem.Click, SelectColorToolStripMenuItem1.Click
        'when colorbutton or menu is clicked, change the color
        OpenColorDialog()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        'change the pen width when the numeric value is changed
        penWidth = CInt(NumericUpDown1.Value)
    End Sub

    Function LastPoint(Optional x As Integer = 0, Optional y As Integer = 0, Optional update As Boolean = True) As Point
        'set last point to point that is entered
        Static _lastPoint As Point

        If update = True Then
            _lastPoint.X = x
            _lastPoint.Y = y
        End If

        Return _lastPoint

    End Function

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'default pen width to 1 and set minimum numeric value to 1
        penWidth = 1
        NumericUpDown1.Minimum = 1
    End Sub

    Sub OpenColorDialog()
        'Change color when clicking the button
        ColorDialog.ShowDialog()
        penColor = ColorDialog.Color

    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click, ClearToolStripMenuItem.Click, ClearToolStripMenuItem1.Click
        'reset the picturebox
        PictureBox1.BackColor = Color.White
        PictureBox1.Refresh()
        'make the program 'Shake'
        Shake()
    End Sub

    Sub Shake()
        'set an amount for the screen to shake
        Dim moveAmount As Integer = 30

        'for this many times, screen will shake
        For i = 1 To 6
            Me.Top += moveAmount
            Me.Left += moveAmount
            System.Threading.Thread.Sleep(50)
            'reverse the amount to move to have a back and forth motion
            moveAmount *= -1
        Next


    End Sub


    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown, PictureBox1.MouseMove
        'do related function to which button is pressed
        Select Case e.Button.ToString
            Case "Left"
                DrawLine(LastPoint(,, False).X, LastPoint(,, False).Y, e.X, e.Y, penColor, penWidth)
            Case "Right"

            Case "Middle"
                'open the color dialog and set the color of the pen
                OpenColorDialog()
            Case "None"
                'don't do anything
            Case "Left, Right"
                'don't do anything
            Case Else
                'something went wrong
                MsgBox("Well this is awkward, you broke it ERROR CODE: 0001")
        End Select

        'set the last point to last point cursor was at
        LastPoint(e.X, e.Y)

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click, AboutToolStripMenuItem1.Click
        'show about form and hide this form
        AboutForm.Show()
        Me.Hide()
    End Sub

    Private Sub DrawWaveformsButton_Click(sender As Object, e As EventArgs) Handles DrawWaveformsButton.Click, DrawWaveformsToolStripMenuItem.Click, DrawWaveformsToolStripMenuItem1.Click

        'reset the picturebox
        PictureBox1.BackColor = Color.White
        PictureBox1.Refresh()

        'make the program 'Shake'
        Shake()

        'draw graticuels
        Draw10x10Graticules()

        'draw waveform sin
        DrawSine()

        'draw waveform cosine
        DrawCoSine()

        'draw the tangent waveform
        DrawTangent()


    End Sub



    Sub Draw10x10Graticules()
        'get the divisions from the width
        Dim xDrawStep As Integer
        xDrawStep = CInt(PictureBox1.Width / 10)

        'draw the vertical graticules
        For i = 0 To PictureBox1.Width Step xDrawStep
            DrawLine(i, 0, i, PictureBox1.Height, Color.Green)
        Next
        DrawLine(PictureBox1.Width - 1, 0, PictureBox1.Width - 1, PictureBox1.Height, Color.Green)

        'find the division pixels from height
        Dim yDrawStep As Integer
        yDrawStep = CInt(PictureBox1.Height / 10)

        'draw the horizontal graticules
        For i = 0 To PictureBox1.Height Step yDrawStep
            DrawLine(0, i, PictureBox1.Width, i, Color.Green)
        Next

    End Sub

    Sub DrawSine()
        'get the division of points for 360 points
        Dim xDiv As Double = PictureBox1.Width / 360
        Dim xCurrent As Double = 0
        Dim xOld As Double = 0

        'get the max value for the sine wave
        Dim yMax As Double = PictureBox1.Height
        Dim yCurrent As Double = 0
        Dim yOld As Double = PictureBox1.Height / 2


        'vi = vp * sin(w*t+theta)+DC
        'instantatneous = yMax * Sin( Degrees * pi / 180) + DC
        'step through every degree and plot point
        For i = 0 To 360 Step 1

            yCurrent = ((-yMax / 2) * Math.Sin((i) * Math.PI / 180)) + (PictureBox1.Height / 2)
            xCurrent += xDiv
            DrawLine(CInt(xOld), CInt(yOld), CInt(xCurrent), CInt(yCurrent), Color.Red)

            xOld = xCurrent
            yOld = yCurrent
        Next

    End Sub

    Sub DrawCoSine()
        'get the division of points for 360 points
        Dim xDiv As Double = PictureBox1.Width / 360
        Dim xCurrent As Double = 0
        Dim xOld As Double = 0

        'get the max value for the waveform
        Dim yMax As Double = PictureBox1.Height
        Dim yCurrent As Double = 0
        Dim yOld As Double = PictureBox1.Height / 2


        'vi = vp * cos(w*t+theta)+DC
        'instantatneous = yMax * cos( Degrees * pi / 180) + DC
        'step through every degree and plot point
        For i = 0 To 360 Step 1

            yCurrent = ((-yMax / 2) * Math.Cos((i) * Math.PI / 180)) + (PictureBox1.Height / 2)
            xCurrent += xDiv
            DrawLine(CInt(xOld), CInt(yOld), CInt(xCurrent), CInt(yCurrent), Color.Orange)

            xOld = xCurrent
            yOld = yCurrent
        Next

    End Sub


    Sub DrawTangent()
        'get the division of points for 360 points
        Dim xDiv As Double = PictureBox1.Width / 360
        Dim xCurrent As Double = 0
        Dim xOld As Double = 0

        'get the max value for the waveform
        Dim yMax As Double = PictureBox1.Height
        Dim yCurrent As Double = 0
        Dim yOld As Double = PictureBox1.Height / 2


        'vi = vp * tan(w*t+theta)+DC
        'instantatneous = yMax * tan( Degrees * pi / 180) + DC
        'step through every degree and plot point
        For i = 0 To 360 Step 1

            yCurrent = ((-yMax / 2) * Math.Tan((i) * Math.PI / 180)) + (PictureBox1.Height / 2)
            xCurrent += xDiv

            If Math.Abs(yCurrent) < PictureBox1.Height Then
                DrawLine(CInt(xOld), CInt(yOld), CInt(xCurrent), CInt(yCurrent), Color.Black)

            End If

            xOld = xCurrent
            yOld = yCurrent
        Next

    End Sub

End Class
