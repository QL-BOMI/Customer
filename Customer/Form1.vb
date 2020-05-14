' Import Data and SqlClient namespaces..
Imports System.Data
Imports System.Data.SqlClient

Public Class Form1

    ' Declare objects..
    Dim objConnection As New SqlConnection("server=VB1\SQLEXPRESS;database=testClipper;integrated security=true")

    Dim objDataAdapter As New SqlDataAdapter(
        "SELECT hname, hdob, hhome, hsex, snational FROM Holder ORDER BY hhold", objConnection)
    Dim objDataSet As DataSet
    Dim objDataView As DataView
    Dim objCurrencyManager As CurrencyManager

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Add items to the combo box..
        cboField.Items.Add("Full Name")
        cboField.Items.Add("DOB")
        cboField.Items.Add("Home Phone")
        cboField.Items.Add("Gender")
        cboField.Items.Add("Nationality")

        ' Make the first item selected..
        cboField.SelectedIndex = 0

        ' Fill the DataSet and bind the fields..
        FillDataSetAndView()
        BindFields()

        ' Show the current record position..
        ShowPosition()
    End Sub

    Private Sub FillDataSetAndView()
        ' Initialize a new instance of the DataSet object.
        objDataSet = New DataSet()

        ' Fill the DataSet object with data.
        objDataAdapter.Fill(objDataSet, "Holder")

        ' Set the DataView object to the DataSet object.
        objDataView = New DataView(objDataSet.Tables("Holder"))

        ' Set our CurrencyManager object to the DataView object.
        objCurrencyManager = CType(Me.BindingContext(objDataView), CurrencyManager)

    End Sub

    Private Sub BindFields()
        ' Clear any previous bindings..
        txtName.DataBindings.Clear()
        txtDOB.DataBindings.Clear()
        txtHomePhone.DataBindings.Clear()
        txtSex.DataBindings.Clear()
        txtNationality.DataBindings.Clear()

        ' Add new bindings to the DataView object..
        txtName.DataBindings.Add("Text", objDataView, "hname")
        txtDOB.DataBindings.Add("Text", objDataView, "hdob")
        txtHomePhone.DataBindings.Add("Text", objDataView, "hhome")
        txtSex.DataBindings.Add("Text", objDataView, "hsex")
        txtNationality.DataBindings.Add("Text", objDataView, "snational")

        ' Display a ready status..
        ToolStripLabel1.Text = "Ready"
    End Sub

    Private Sub ShowPosition()
        'Format number in the txtGender field to include cents
        'Try
        '    txtGender.Text = Format(CType(txtGender.Text, Decimal), "##0.00")
        'Catch e As System.Exception
        '    txtGender.Text = "0"
        '    txtGender.Text = Format(CType(txtGender.Text, Decimal), "##0.00")
        'End Try

        ' Display the current position and the number of records
        txtRecordPosition.Text = objCurrencyManager.Position + 1 & " of " & objCurrencyManager.Count()

    End Sub

    Private Sub btnMoveFirst_Click(Sender As Object, E As EventArgs) Handles btnMoveFirst.Click
        ' Set the record position to the first record..
        objCurrencyManager.Position = 0

        ' Show the current record position..
        ShowPosition()
    End Sub

    Private Sub btnMovePrevious_Click(Sender As Object, E As EventArgs) Handles btnMovePrevious.Click
        ' Move to the previous record..
        objCurrencyManager.Position -= 1

        ' Show the current record position..
        ShowPosition()
    End Sub

    Private Sub btnMoveNext_Click(Sender As Object, E As EventArgs) Handles btnMoveNext.Click
        ' Move to the next record..
        objCurrencyManager.Position += 1

        ' Show the current record position..
        ShowPosition()
    End Sub

    Private Sub btnMoveLast_Click(Sender As Object, E As EventArgs) Handles btnMoveLast.Click
        ' Set the record position to the last record..
        objCurrencyManager.Position = objCurrencyManager.Count - 1

        ' Show the current record position..
        ShowPosition()
    End Sub

    Private Sub btnPerformSort_Click(Sender As Object, E As EventArgs) Handles btnPerformSort.Click
        ' Determine the appropriate item selected and set the
        ' Sort property of the DataView object..
        Select Case cboField.SelectedIndex
            Case 0 'Full Name
                objDataView.Sort = "hname"
            Case 1 'Birth Date
                objDataView.Sort = "hdob"
            Case 2 'Home Phone
                objDataView.Sort = "hhome"
            Case 3 'Gender
                objDataView.Sort = "hsex"
            Case 4 'Nationality
                objDataView.Sort = "snational"
        End Select

        ' Call the click event for the MoveFirst button..
        btnMoveFirst_Click(Nothing, Nothing)

        ' Display a message that the records have been sorted..
        ToolStripLabel1.Text = "Records Sorted"
    End Sub

    Private Sub btnPerformSearch_Click(Sender As Object, E As EventArgs) Handles btnPerformSearch.Click
        ' Declare local variables..
        Dim intPosition As Integer

        ' Determine the appropriate item selected and set the
        ' Sort property of the DataView object..
        Select Case cboField.SelectedIndex
            Case 0 'Full Name
                objDataView.Sort = "hname"
            Case 1 'Birth Date
                objDataView.Sort = "hdob"
            Case 2 'Home Phone
                objDataView.Sort = "hhome"
            Case 3 'Gender
                objDataView.Sort = "hsex"
            Case 4 'Nationality
                objDataView.Sort = "snational"
        End Select

        'search field

        If cboField.SelectedIndex < 5 Then
            ' Find the last name, first name, addr1, gender or sort name..
            intPosition = objDataView.Find(txtSearchCriteria.Text)
        End If

        If intPosition = -1 Then
            ' Display a message that the record was not found..
            ToolStripLabel1.Text = "Record Not Found"
            'txtFirstName.Text = ""
            'txtLastName.Text = ""
            'txtEmail.Text = ""
            'txtGender.Text = ""
        Else
            ' Otherwise display a message that the record was
            ' found and reposition the CurrencyManager to that
            ' record..
            ToolStripLabel1.Text = "Record Found"
            objCurrencyManager.Position = intPosition
        End If

        ' Show the current record position..
        ShowPosition()
    End Sub


    Private Sub btnNew_Click(Sender As Object, E As EventArgs) Handles btnNew.Click
        ' Clear the Email and gender fields..
        txtSex.Text = ""
        txtNationality.Text = ""
    End Sub

    'Private Sub btnAdd_Click(Sender As Object,
    '        E As EventArgs) Handles btnAdd.Click
    '    ' Declare local variables and objects..
    '    Dim intPosition As Integer, intMaxID As Integer
    '    Dim strID As String
    '    Dim objCommand As SqlCommand = New SqlCommand()

    '    ' Save the current record position..
    '    intPosition = objCurrencyManager.Position

    '    ' Create a new SqlCommand object..
    '    Dim maxIdCommand As SqlCommand = New SqlCommand _
    '   ("SELECT MAX(title_id) AS MaxID " &
    '    "FROM titles WHERE title_id LIKE 'DM%'", objConnection)

    '    ' Open the connection, execute the command
    '    objConnection.Open()
    '    Dim maxId As Object = maxIdCommand.ExecuteScalar()
    '    ' If the MaxID column is null..
    '    If maxId Is DBNull.Value Then
    '        ' Set a default value of 1000..
    '        intMaxID = 1000
    '    Else
    '        ' otherwise set the strID variable to the value in MaxID..
    '        strID = CType(maxId, String)

    '        ' Get the integer part of the string..
    '        intMaxID = CType(strID.Remove(0, 2), Integer)

    '        ' Increment the value..
    '        intMaxID += 1
    '    End If

    '    ' Finally, set the new ID..
    '    strID = "DM" & intMaxID.ToString

    '    ' Set the SqlCommand object properties..
    '    objCommand.Connection = objConnection
    '    objCommand.CommandText = "INSERT INTO titles " &
    '    "(title_id, title, type, price, pubdate) " &
    '    "VALUES(@title_id,@title,@type,@price,@pubdate);" &
    '    "INSERT INTO titleauthor (au_id, title_id) VALUES(@au_id,@title_id)"

    '    ' Add parameters for the placeholders in the SQL in the
    '    ' CommandText property..

    '    ' Parameter for the title_id column..
    '    objCommand.Parameters.AddWithValue("@title_id", strID)

    '    ' Parameter for the title column..
    '    objCommand.Parameters.AddWithValue("@title", txtEmail.Text)

    '    ' Parameter for the type column
    '    objCommand.Parameters.AddWithValue("@type", "Demo")

    '    ' Parameter for the price column..
    '    objCommand.Parameters.AddWithValue("@price", txtGender.Text).DbType _
    '                              = DbType.Currency

    '    ' Parameter for the pubdate column
    '    objCommand.Parameters.AddWithValue("@pubdate", Date.Now)

    '    ' Parameter for the au_id column..
    '    objCommand.Parameters.AddWithValue _
    '              ("@au_id", BindingContext(objDataView).Current("au_id"))

    '    ' Execute the SqlCommand object to insert the new data..
    '    Try
    '        objCommand.ExecuteNonQuery()
    '    Catch SqlExceptionErr As SqlException
    '        MessageBox.Show(SqlExceptionErr.Message)
    '    End Try

    '    ' Close the connection..
    '    objConnection.Close()

    '    ' Fill the dataset and bind the fields..
    '    FillDataSetAndView()
    '    BindFields()

    '    ' Set the record position to the one that you saved..
    '    objCurrencyManager.Position = intPosition

    '    ' Show the current record position..
    '    ShowPosition()

    '    ' Display a message that the record was added..
    '    ToolStripLabel1.Text = "Record Added"
    'End Sub

    'Private Sub btnUpdate_Click(Sender As Object,
    '         E As EventArgs) Handles btnUpdate.Click
    '    ' Declare local variables and objects..
    '    Dim intPosition As Integer
    '    Dim objCommand As SqlCommand = New SqlCommand()

    '    ' Save the current record position..
    '    intPosition = objCurrencyManager.Position

    '    ' Set the SqlCommand object properties..
    '    objCommand.Connection = objConnection
    '    objCommand.CommandText = "UPDATE titles " &
    '        "SET title = @title, price = @price WHERE title_id = @title_id"
    '    objCommand.CommandType = CommandType.Text

    '    ' Add parameters for the placeholders in the SQL in the
    '    ' CommandText property..

    '    ' Parameter for the title field..
    '    objCommand.Parameters.AddWithValue("@title", txtEmail.Text)

    '    ' Parameter for the price field..
    '    objCommand.Parameters.AddWithValue("@price", txtGender.Text).DbType _
    '        = DbType.Currency

    '    ' Parameter for the title_id field..
    '    objCommand.Parameters.AddWithValue _
    '        ("@title_id", BindingContext(objDataView).Current("title_id"))

    '    ' Open the connection..
    '    objConnection.Open()

    '    ' Execute the SqlCommand object to update the data..
    '    objCommand.ExecuteNonQuery()

    '    ' Close the connection..
    '    objConnection.Close()

    '    ' Fill the DataSet and bind the fields..
    '    FillDataSetAndView()
    '    BindFields()

    '    ' Set the record position to the one that you saved..
    '    objCurrencyManager.Position = intPosition

    '    ' Show the current record position..
    '    ShowPosition()

    '    ' Display a message that the record was updated..
    '    ToolStripLabel1.Text = "Record Updated"
    'End Sub

    'Private Sub btnDelete_Click(Sender As Object,
    '        E As EventArgs) Handles btnDelete.Click
    '    ' Declare local variables and objects..
    '    Dim intPosition As Integer
    '    Dim objCommand As SqlCommand = New SqlCommand()

    '    ' Save the current record position—1 for the one to be
    '    ' deleted..
    '    intPosition = Me.BindingContext(objDataView).Position - 1

    '    ' If the position is less than 0 set it to 0..
    '    If intPosition < 0 Then
    '        intPosition = 0
    '    End If

    '    ' Set the Command object properties..
    '    objCommand.Connection = objConnection
    '    objCommand.CommandText = "DELETE FROM titleauthor " &
    '        "WHERE title_id = @title_id;" &
    '        "DELETE FROM titles WHERE title_id = @title_id"

    '    ' Parameter for the title_id field..
    '    objCommand.Parameters.AddWithValue _
    '    ("@title_id", BindingContext(objDataView).Current("title_id"))

    '    ' Open the database connection..
    '    objConnection.Open()

    '    ' Execute the SqlCommand object to update the data..
    '    objCommand.ExecuteNonQuery()

    '    ' Close the connection..
    '    objConnection.Close()

    '    ' Fill the DataSet and bind the fields..
    '    FillDataSetAndView()
    '    BindFields()

    '    ' Set the record position to the one that you saved..
    '    Me.BindingContext(objDataView).Position = intPosition

    '    ' Show the current record position..
    '    ShowPosition()

    '    ' Display a message that the record was deleted..
    '    ToolStripLabel1.Text = "Record Deleted"
    'End Sub


End Class


