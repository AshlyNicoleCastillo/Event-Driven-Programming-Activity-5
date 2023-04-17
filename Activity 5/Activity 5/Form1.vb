Imports System.IO
Imports System.Text

Public Class Form1
    Private Sub BtnUpload_Click(sender As Object, e As EventArgs) Handles BtnUpload.Click
        Dim openFileDialog1 As New OpenFileDialog()
        openFileDialog1.Filter = "CSV Files (*.csv)|*.csv"
        openFileDialog1.Title = "Select a CSV File"
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim csvData As String = File.ReadAllText(openFileDialog1.FileName)
            Dim lines() As String = csvData.Split(ControlChars.Lf)
            Dim data As New DataTable()
            Dim headers() As String = lines(0).Split(",")
            For Each header As String In headers
                data.Columns.Add(header, GetType(String))
            Next
            For i As Integer = 1 To lines.Length - 1
                Dim fields() As String = lines(i).Split(",")
                data.Rows.Add(fields)
            Next
            DataGridView1.DataSource = data
        End If
    End Sub


    Private Sub BtnBackup_Click(sender As Object, e As EventArgs) Handles BtnBackup.Click
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.Filter = "CSV Files (*.csv)|*.csv"
        saveFileDialog1.Title = "Save CSV File Backup"
        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim writer As New StreamWriter(saveFileDialog1.FileName)
            For Each row As DataGridViewRow In DataGridView1.Rows
                Dim cells As List(Of String) = (From cell As DataGridViewCell In row.Cells Select Convert.ToString(cell.Value)).ToList()
                writer.WriteLine(String.Join(",", cells))
            Next
            writer.Flush()
            writer.Close()
            MessageBox.Show("Backup successfully created!", "Backup Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub BtnView_Click(sender As Object, e As EventArgs) Handles BtnView.Click
        Dim openFileDialog1 As New OpenFileDialog()
        openFileDialog1.Filter = "CSV Files (*.csv)|*.csv"
        openFileDialog1.Title = "Select a CSV File to View"
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim reader As New StreamReader(openFileDialog1.FileName)
            Dim csvData As New DataTable()
            Dim isFirstRow As Boolean = True
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                If isFirstRow Then
                    Dim headers As String() = line.Split(",")
                    For Each header As String In headers
                        csvData.Columns.Add(header.Trim())
                    Next
                    isFirstRow = False
                Else
                    Dim values As String() = line.Split(",")
                    csvData.Rows.Add(values)
                End If
            End While
            reader.Close()

            Dim form As New Form()
            form.Text = openFileDialog1.FileName
            form.Width = 800
            form.Height = 600

            Dim dataGridView As New DataGridView()
            dataGridView.DataSource = csvData
            dataGridView.Dock = DockStyle.Fill

            form.Controls.Add(dataGridView)
            form.ShowDialog()
        End If
    End Sub


    Private Sub BtnNewFile_Click(sender As Object, e As EventArgs) Handles BtnNewFile.Click
        Dim data As New DataTable()
        data.Columns.Add("Column1", GetType(String))
        data.Columns.Add("Column2", GetType(String))
        data.Columns.Add("Column3", GetType(String))
        data.Columns.Add("Column4", GetType(String))
        data.Columns.Add("Column5", GetType(String))
        data.Rows.Add("Row1", GetType(String))
        data.Rows.Add("Row2", GetType(String))
        data.Rows.Add("Row3", GetType(String))
        data.Rows.Add("Row4", GetType(String))
        data.Rows.Add("Row5", GetType(String))
        DataGridView1.DataSource = data

        'Prompt the user to save the new file
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.Filter = "CSV Files (*.csv)|*.csv"
        saveFileDialog1.Title = "Save the new CSV file"
        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            'Save the data to the specified file
            Dim sb As New StringBuilder()
            Dim headers() As String = data.Columns.Cast(Of DataColumn)().Select(Function(c) c.ColumnName).ToArray()
            sb.AppendLine(String.Join(",", headers))
            For Each row As DataRow In data.Rows
                Dim fields() As String = row.ItemArray.Select(Function(f) f.ToString()).ToArray()
                sb.AppendLine(String.Join(",", fields))
            Next
            File.WriteAllText(saveFileDialog1.FileName, sb.ToString())
        End If
    End Sub


    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this file?", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            DataGridView1.DataSource = Nothing
            MessageBox.Show("Deleted successfully!", "Backup Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class