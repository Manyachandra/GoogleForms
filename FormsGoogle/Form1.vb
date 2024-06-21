Public Class Form1
    Inherits Form

    ' Define the BlueButton class for consistent button appearance
    Public Class BlueButton
        Inherits Button

        Public Sub New()
            Me.BackColor = Color.LightBlue
            Me.ForeColor = Color.White
        End Sub
    End Class

    ' Define the Submission class
    Public Class Submission
        Public Property Name As String
        Public Property Email As String
        Public Property PhoneNumber As String
        Public Property GitHubRepoLink As String
        Public Property StopWatchTime As String
    End Class

    Private Sub btnViewSubmissions_Click(sender As Object, e As EventArgs) Handles btnViewSubmissions.Click
        Dim viewSubmissionsForm As New ViewSubmissionsForm()
        viewSubmissionsForm.Show()
    End Sub

    Private Sub btnCreateSubmission_Click(sender As Object, e As EventArgs) Handles btnCreateSubmission.Click
        Dim createSubmissionForm As New CreateSubmissionForm()
        createSubmissionForm.Show()
    End Sub

    ' Define the ViewSubmissionsForm class
    Public Class ViewSubmissionsForm
        Inherits Form

        Private submissions As List(Of Submission)
        Private currentIndex As Integer = 0

        Private WithEvents btnPrevious As New BlueButton()
        Private WithEvents btnNext As New BlueButton()
        Private WithEvents btnDelete As New BlueButton()
        Private WithEvents btnEdit As New BlueButton()
        Private lblSubmissionDetails As New Label()

        Public Sub New()
            Me.Text = "View Submissions"
            Me.Size = New Size(400, 300)

            btnPrevious.Text = "&Previous"
            btnPrevious.Location = New Point(10, 230)
            Me.Controls.Add(btnPrevious)

            btnNext.Text = "&Next"
            btnNext.Location = New Point(100, 230)
            Me.Controls.Add(btnNext)

            btnDelete.Text = "&Delete"
            btnDelete.Location = New Point(190, 230)
            Me.Controls.Add(btnDelete)

            btnEdit.Text = "&Edit"
            btnEdit.Location = New Point(280, 230)
            Me.Controls.Add(btnEdit)

            lblSubmissionDetails.AutoSize = True
            lblSubmissionDetails.Location = New Point(10, 10)
            lblSubmissionDetails.Width = 380
            lblSubmissionDetails.Height = 200
            lblSubmissionDetails.BorderStyle = BorderStyle.FixedSingle
            Me.Controls.Add(lblSubmissionDetails)

            LoadSubmissions()
            ShowSubmission(currentIndex)
        End Sub

        Private Sub LoadSubmissions()
            ' Load submissions from backend or database
            submissions = New List(Of Submission)()
            submissions.Add(New Submission With {
                .Name = "John Doe",
                .Email = "john@example.com",
                .PhoneNumber = "1234567890",
                .GitHubRepoLink = "https://github.com/johndoe",
                .StopWatchTime = "00:01:00"
            })
        End Sub

        Private Sub ShowSubmission(index As Integer)
            If submissions.Count > 0 Then
                Dim submission As Submission = submissions(index)
                lblSubmissionDetails.Text = $"Name: {submission.Name}" & vbCrLf &
                                            $"Email: {submission.Email}" & vbCrLf &
                                            $"Phone: {submission.PhoneNumber}" & vbCrLf &
                                            $"GitHub: {submission.GitHubRepoLink}" & vbCrLf &
                                            $"StopWatchTime: {submission.StopWatchTime}"
            Else
                lblSubmissionDetails.Text = "No submissions available."
            End If
        End Sub

        Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
            If currentIndex > 0 Then
                currentIndex -= 1
                ShowSubmission(currentIndex)
            End If
        End Sub

        Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
            If currentIndex < submissions.Count - 1 Then
                currentIndex += 1
                ShowSubmission(currentIndex)
            End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
                submissions.RemoveAt(currentIndex)
                If currentIndex >= submissions.Count Then
                    currentIndex = submissions.Count - 1
                End If
                ShowSubmission(currentIndex)
                MessageBox.Show("Submission Deleted!")
            End If
        End Sub

        Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            If currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
                Dim editForm As New CreateSubmissionForm(submissions(currentIndex))
                editForm.ShowDialog()

                ' Update the submission after editing
                submissions(currentIndex) = editForm.GetSubmission()
                ShowSubmission(currentIndex)
            End If
        End Sub
    End Class

    ' Define the CreateSubmissionForm class
    Public Class CreateSubmissionForm
        Inherits Form

        Private txtName As New TextBox()
        Private txtEmail As New TextBox()
        Private txtPhoneNumber As New TextBox()
        Private txtGitHubRepoLink As New TextBox()
        Private txtStopWatchTime As New TextBox()
        Private WithEvents btnSubmit As New BlueButton()

        Private WithEvents btnStartPause As New BlueButton()
        Private lblElapsedTime As New Label()

        Private stopwatch As New System.Diagnostics.Stopwatch()
        Private timer As New Timer()

        Private submission As Submission

        Public Sub New()
            Me.Text = "Create New Submission"
            Me.Size = New Size(400, 350)

            InitializeForm()
            InitializeStopwatch()
        End Sub

        Public Sub New(existingSubmission As Submission)
            Me.Text = "Edit Submission"
            Me.Size = New Size(400, 350)

            submission = existingSubmission

            InitializeForm()
            InitializeStopwatch()

            ' Populate fields with existing data for editing
            txtName.Text = submission.Name
            txtEmail.Text = submission.Email
            txtPhoneNumber.Text = submission.PhoneNumber
            txtGitHubRepoLink.Text = submission.GitHubRepoLink
            txtStopWatchTime.Text = submission.StopWatchTime
            lblElapsedTime.Text = submission.StopWatchTime
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            If keyData = (Keys.Control Or Keys.S) Then
                btnSubmit.PerformClick()
                Return True
            ElseIf keyData = (Keys.Control Or Keys.T) Then
                btnStartPause.PerformClick()
                Return True
            End If

            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function

        Private Sub InitializeForm()
            Dim lblName As New Label With {.Text = "Name:", .Location = New Point(10, 10)}
            txtName.Location = New Point(150, 10)
            Me.Controls.Add(lblName)
            Me.Controls.Add(txtName)

            Dim lblEmail As New Label With {.Text = "Email:", .Location = New Point(10, 50)}
            txtEmail.Location = New Point(150, 50)
            Me.Controls.Add(lblEmail)
            Me.Controls.Add(txtEmail)

            Dim lblPhoneNumber As New Label With {.Text = "Phone Number:", .Location = New Point(10, 90)}
            txtPhoneNumber.Location = New Point(150, 90)
            Me.Controls.Add(lblPhoneNumber)
            Me.Controls.Add(txtPhoneNumber)

            Dim lblGitHubRepoLink As New Label With {.Text = "GitHub Repo Link:", .Location = New Point(10, 130)}
            txtGitHubRepoLink.Location = New Point(150, 130)
            Me.Controls.Add(lblGitHubRepoLink)
            Me.Controls.Add(txtGitHubRepoLink)

            Dim lblStopWatchTime As New Label With {.Text = "StopWatchTime:", .Location = New Point(10, 170)}
            txtStopWatchTime.Location = New Point(150, 170)
            Me.Controls.Add(lblStopWatchTime)
            Me.Controls.Add(txtStopWatchTime)

            btnSubmit.Text = "&Submit"
            btnSubmit.Location = New Point(150, 210)
            Me.Controls.Add(btnSubmit)

            AddHandler btnSubmit.Click, AddressOf btnSubmit_Click
        End Sub

        Private Sub InitializeStopwatch()
            lblElapsedTime.Location = New Point(10, 250)
            lblElapsedTime.Size = New Size(150, 30)
            lblElapsedTime.BorderStyle = BorderStyle.FixedSingle
            lblElapsedTime.Text = "00:00:00" ' Initialize with 00:00:00
            Me.Controls.Add(lblElapsedTime)

            btnStartPause.Text = "&Start/Pause"
            btnStartPause.Location = New Point(170, 250)
            Me.Controls.Add(btnStartPause)

            timer.Interval = 1000
            AddHandler timer.Tick, AddressOf Timer_Tick
        End Sub

        Private Sub Timer_Tick(sender As Object, e As EventArgs)
            lblElapsedTime.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
        End Sub

        Private Sub btnStartPause_Click(sender As Object, e As EventArgs) Handles btnStartPause.Click
            If Not stopwatch.IsRunning Then
                stopwatch.Reset() ' Reset the stopwatch
                lblElapsedTime.Text = "00:00:00" ' Reset the label to 00:00:00
            End If

            If stopwatch.IsRunning Then
                stopwatch.Stop()
                btnStartPause.Text = "Resume"
            Else
                stopwatch.Start()
                btnStartPause.Text = "Pause"
            End If
        End Sub

        Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
            If submission Is Nothing Then
                submission = New Submission()
            End If

            ' Update submission data
            submission.Name = txtName.Text
            submission.Email = txtEmail.Text
            submission.PhoneNumber = txtPhoneNumber.Text
            submission.GitHubRepoLink = txtGitHubRepoLink.Text
            submission.StopWatchTime = lblElapsedTime.Text

            ' Simulate submission to backend or database
            ' Ideally, you would save/update data here

            MessageBox.Show("Submission Successful!")

            ' Close the form after submission
            Me.Close()
        End Sub

        Public Function GetSubmission() As Submission
            Return submission
        End Function
    End Class

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnCreateSubmission.Click

    End Sub
End Class
