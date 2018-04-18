Public Class Form1
    Dim xlApp, xlBook, xlSheet, xlBook_new, xlsheet_new, xlapp_new As Object
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim xlApp, xlBook, xlSheet, xlBook_new, xlsheet_new, xlapp_new As Object
        Dim A_(58) As Integer
        Dim B_(), C_(), D_(), CD_(), E_(), F_() As Double
        Dim i, sig As Integer
        Dim strFile As String = ""
        Dim intRow, intCol As Integer
        Dim lastRow, lastCol As Integer
        Dim retry As Boolean
        Dim minRnd, maxRnd, Rndi As Double
        Dim str_in As String
        Dim strDtm As Date
        Dim count As Integer = 0

        '可变
        'intRow = 59
        'intCol = 1
        With OpenFileDialog1
            .Filter = "表格文件(*.xls)|*.xls;*.xlsx"
            .Title = "数据导入"
            .FileName = ""
        End With
        If OpenFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Button1.Enabled = False
            'Try
            '导入表格
            xlApp = CreateObject("Excel.Application")
            xlBook = xlApp.Workbooks.Open(OpenFileDialog1.FileName)
            'xlApp.Visible = True '设置EXCEL对象可见（或不可见）  
            xlSheet = xlBook.Worksheets(1) '设置活动工作表''

            xlapp_new = CreateObject("Excel.Application")
            xlBook_new = xlapp_new.workbooks.add
            'xlapp_new.Visible = True
            lastCol = xlSheet.usedrange.columns.count

            lastRow = xlSheet.usedrange.rows.count
            'lastRow = xlSheet.cells(lastRow, 3).end(xlup).row
            ReDim B_(lastRow - 1), C_(lastRow - 1), D_(lastRow - 1), CD_(lastRow - 1), E_(lastRow - 1), F_(lastRow)
            F_(lastRow) = 0
            'intCol = 1
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = lastCol
            '添加sheet页
            retry = True

            While retry
                Try
                    'Do While Not (String.IsNullOrEmpty(xlSheet.cells(1, intCol).value.ToString()))
                    'Loop
                    For j = 1 To lastCol
                        xlsheet_new = xlBook_new.Worksheets.Add
                        intCol = j
                    Next
                    retry = False
                    count = 0
                Catch ex As Exception
                    RichTextBox1.Text = "0: addSheet col-" + intCol + " ,row-" + i + vbCrLf + RichTextBox1.Text
                    retry = True
                    count = count + 1
                    If count > 100 Then
                        xlapp_new.Visible = True
                        MessageBox.Show("输出错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End Try
            End While
            RichTextBox1.Text = "添加表格完成！" + vbCrLf + RichTextBox1.Text

            For j = 1 To lastCol
                Try
                    Rndi = Rndz(minRnd, maxRnd)
                Catch ex As Exception
                    MessageBox.Show("请输入正确的随机数范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
                xlsheet_new = xlBook_new.worksheets(j)       '表格个数

                If IsDate(xlSheet.cells(1, j).value) Then
                    strDtm = xlSheet.cells(1, j).value
                    str_in = Format(strDtm, "yyyy-mm-dd") '''''''''''''''''''''''''''''''''''弃掉
                    str_in = xlSheet.cells(1, j).value

                    str_in = "#" + str_in.Split("/")(1) + "月" + str_in.Split("/")(2) + "日" + Rnd_hour.ToString("00") + "时" + Rnd_Minute.ToString() + "分" + Rnd_Minute.ToString() + "秒"

                Else
                    '  [] *
                    str_in = "#" + xlSheet.cells(1, j).value + "___" + Rnd_hour.ToString("00") + "时" + Rnd_Minute.ToString() + "分" + Rnd_Minute.ToString() + "秒"
                    str_in = str_in.Replace(",", "_")
                    str_in = str_in.Replace("/", "|")
                    str_in = str_in.Replace("\", "|")
                    str_in = str_in.Replace("?", "a")
                    str_in = str_in.Replace("*", "&")
                    str_in = str_in.Replace("[", "(")
                    str_in = str_in.Replace("]", ")")
                End If

                str_in = TextBox3.Text + str_in
                If str_in.Length > 32 Then
                    str_in = str_in.Substring(0, 32)
                End If
                xlsheet_new.name = str_in     '表格名字  '！不能重名
                retry = True
                While retry
                    Try
                        ''''''''''''''''''''''''''''
                        '''''''''''''''''''''''''
                        For i = lastRow - 1 To 1 Step -1
                            F_(i) = xlSheet.cells(i + 1, j).value
                            E_(i) = Math.Round(F_(i) - F_(i + 1), 2)
                            sig = Math.Sign(E_(i))   '????????两位小数
                            C_(i) = Math.Round(E_(i) + Rndi, 2)
                            'MessageBox.Show(C_(i)
                            D_(i) = Math.Round(C_(i) - 2 * E_(i), 2)
                            F_(i) = Math.Round(F_(i), 2)
                        Next

                        xlsheet_new.cells(1, 1).value = "测点"
                        xlsheet_new.cells(1, 2).value = "深度(米)"
                        xlsheet_new.cells(1, 3).value = "A+(毫米)"
                        xlsheet_new.cells(1, 4).value = "A-(毫米)"
                        xlsheet_new.cells(1, 5).value = "测值(毫米)"
                        xlsheet_new.cells(1, 6).value = "累计位移(毫米)"

                        For i = 1 To lastRow - 1
                            xlsheet_new.cells(i + 1, 1).value = i
                            xlsheet_new.cells(i + 1, 2).value = i / 2
                            xlsheet_new.cells(i + 1, 3).value = C_(i).ToString()
                            xlsheet_new.cells(i + 1, 4).value = D_(i).ToString()
                            xlsheet_new.cells(i + 1, 5).value = E_(i).ToString()
                            xlsheet_new.cells(i + 1, 6).value = F_(i).ToString()
                        Next i
                        retry = False
                        ProgressBar1.Value = j
                        My.Application.DoEvents()
                        RichTextBox1.Text = j.ToString + vbCrLf + RichTextBox1.Text
                        count = 0
                    Catch ex As Exception
                        RichTextBox1.Text = "1: name col-" + j.ToString + " ,row-" + i.ToString + vbCrLf + RichTextBox1.Text
                        retry = True
                        count = count + 1
                        If count > 100 Then
                            xlapp_new.Visible = True
                            MessageBox.Show("输出错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End Try
                End While

                'intCol = intCol + 1
                'Loop
            Next j



            Button1.Enabled = True
            xlSheet = Nothing
            xlBook.Close()
            xlApp.Quit()
            xlBook = Nothing


            xlsheet_new = Nothing

            xlBook_new = Nothing
            xlapp_new.Visible = True
            xlApp = Nothing
            xlapp_new = Nothing
            ProgressBar1.Value = 0
            If CheckBox1.Checked = True Then
                Application.Restart()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            xlSheet = Nothing
            xlBook.Close()
            xlApp.Quit()
            xlBook = Nothing

            xlsheet_new = Nothing

            xlBook_new = Nothing
            xlapp_new.visible = True
            xlapp_new.Quit()
            xlApp = Nothing
            xlapp_new = Nothing
        Catch ex As Exception

        End Try
        
        Application.Exit()


    End Sub


    Private Function Rndz(ByVal a_min As Single, ByVal b_max As Single) As Double '
        '-50   50
        a_min = CInt(a_min * 100)
        b_max = CInt(b_max * 100)

        Return Int((a_min - b_max + 1) * Rnd() + b_max) / 100
        'M = (Int(Rnd() * 101) - 51(0 - 100) - 50~50

    End Function
    Private Function randomizer() As Double
        Dim A As Integer
        Dim M As Double
        'Randomiz
        A = Int(Rnd() * 1000) + 1    '0.01-0.50
        Select Case A
            Case 1 To 10
                M = (Int(Rnd() * 50) + 1) / 100  '0.01-0.50
            Case 11 To 950
                M = (Int(Rnd() * 40) + 10) / 100  '大概率 0.10-0.50
            Case 951 To 1000
                M = (Int(Rnd() * 50) + 1) / 100
        End Select
        Return M
    End Function
    Private Function Rnd_hour() As Double
        Dim A, M As Integer
        'Randomiz
        A = Int(Rnd() * 2) + 1    '0.01-0.50
        Select Case A
            Case 1
                M = (Int(Rnd() * 5) + 8)  '[8,12]
            Case 2
                M = (Int(Rnd() * 5) + 13)  '[14,18]
        End Select
        Return M
    End Function
    Private Function Rnd_Minute() As Double
        Dim A As Integer
        'Randomiz
        A = Int(Rnd() * 61)    '0.01-0.50
        Return A
    End Function

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged

    End Sub
    Private Function strToSheetName(ByVal str As String) As String  'Test  未使用
        Dim Dtm As Date
        If IsDate(str) Then
            Dtm = str
            str = "#" + Dtm.Month.ToString + "月" + Dtm.Day.ToString + "日" + Rnd_hour.ToString("00") + "时" + Rnd_Minute.ToString() + "分" + Rnd_Minute.ToString() + "秒"
        Else
            str = "#" + str + "___" + Rnd_hour.ToString("00") + "时" + Rnd_Minute.ToString() + "分" + Rnd_Minute.ToString() + "秒"
            str = str.Replace(",", "_")
            str = str.Replace("/", "|")
            str = str.Replace("\", "|")
            str = str.Replace("?", "a")
            str = str.Replace("*", "&")
            str = str.Replace("[", "(")
            str = str.Replace("]", ")")
        End If

        Return str
    End Function
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
