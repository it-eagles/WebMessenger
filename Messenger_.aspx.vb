﻿Option Strict On
Option Explicit On

Imports System.Linq
Imports System.Data
Imports System.Globalization
Imports System.DateTime

Public Class Messenger_
    Inherits System.Web.UI.Page
    'Dim db As New DB_EaglesInternalE01
    'Dim db As New DB_EaglesIntemalEntities_test
    Dim db As New DB_EaglesInternalEntities
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim menu As String = "Messenger Booking"
            Dim id As String = Session("UserID").ToString

            Dim ds = (From c In db.tblUserMenu Where c.UserID = id And c.Menu = menu And c.Save_ = 1).FirstOrDefault
            If IsNothing(ds) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect", "alert('คุณไม่มีสิทธิ์เข้าเมนูนี้ครับ'); window.location='" + Request.ApplicationPath + "Default.aspx'; ", True)
            Else
                lblNameAdd.Text = Session("Prefix_thai").ToString + " " + Session("Name_thai").ToString + " " + Session("Surname_thai").ToString
                BindData()
            End If



        End If
    End Sub
    Private Sub BindData()
        Dim Name As String = Session("Prefix_thai").ToString & " " & Session("Name_thai").ToString + " " + Session("Surname_thai").ToString
        'Where c.Name.Contains(Name)
        Dim ds = (From c In db.tblMessenger Where c.Name.Contains(Name) Order By c.MessDate Descending
                     Select New With {
                         c.MessDate, c.MileIn, c.MileOut, c.Name, _
                         c.TimeIn, c.TimeOut}).ToList()

        If ds.Count > 0 Then
            Me.showMessnger.DataSource = ds
            Me.showMessnger.DataBind()
        Else
            Me.showMessnger.DataSource = Nothing
            Me.showMessnger.DataBind()
        End If

    End Sub

    Protected Sub lnkEdit_Click(ByVal sender As Object, ByVal e As EventArgs)

        'Dim btnsubmit As LinkButton = TryCast(sender, LinkButton)
        'Dim gRow As GridViewRow = DirectCast(btnsubmit.NamingContainer, GridViewRow)
        'Dim dateMess As Date = DateTime.ParseExact(CStr(GridView1.DataKeys(gRow.RowIndex).Values(0)), "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"))
        'Dim name As String = CStr(GridView1.DataKeys(gRow.RowIndex).Values(1))
        Dim item As RepeaterItem = TryCast(TryCast(sender, LinkButton).Parent, RepeaterItem)
        Dim dateMess As String = TryCast(item.FindControl("lblMessDate"), Label).Text.Trim
        Dim dateM As Date = DateTime.ParseExact(dateMess, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"))
        Dim name As String = TryCast(item.FindControl("lblName"), Label).Text.Trim


        'Dim ds = (From c In db.tblMessengers Where c.MessDate = dateM And c.Name = name).SingleOrDefault
        'lblMessDate.Text = CStr(ds.MessDate)
        'lblName.Text = ds.Name
        'txtTimeOut.Text = ds.TimeOut
        'txtMileOut.Text = CStr(ds.MileOut)
        'txtTimeIn.Text = ds.TimeIn
        'txtMileIn.Text = CStr(ds.MileIn)


        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EditModalScript", "openModalEdit();", True)
        '"EditDistance.aspx?Date=" 
        Dim url As String = "EditDistance.aspx?Date=" + CStr(dateM)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "onclick", "javascript:window.open( '" + url + "','_self','height=600px,width=1000px,scrollbars=1');", True)

    End Sub

    Protected Sub btnSaveChange_Click(sender As Object, e As EventArgs) Handles btnSaveChange.Click

        Dim messdate As Date = DateTime.ParseExact(lblMessDate.Text, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"))

        Dim Messenger As tblMessenger = (From c In db.tblMessenger _
                        Where c.Name = lblName.Text And c.MessDate = messdate _
                        Select c).First()
        Messenger.TimeOut = txtTimeOut.Text.Trim
        Messenger.MileOut = CDec(txtMileOut.Text.Trim)
        Messenger.TimeIn = txtTimeOut.Text.Trim
        Messenger.MileIn = CDec(txtMileIn.Text.Trim)
        Messenger.UpdateBy = Session("Name_eng").ToString.ToUpper
        Messenger.UpdateDate = Now

        db.SaveChanges()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect", "alert('แก้ไขข้อมูล เรียบร้อยแล้วครับ'); window.location='" + Request.ApplicationPath + "/Default.aspx';", True)

    End Sub

    'Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim lblMessDate As Label = DirectCast(e.Row.FindControl("lblMessDate"), Label)
    '        If Not IsNothing(lblMessDate) Then
    '            lblMessDate.Text = CStr(DataBinder.Eval(e.Row.DataItem, "MessDate", "{0:dd/MM/yyyy}"))
    '        End If
    '        Dim lblName As Label = DirectCast(e.Row.FindControl("lblName"), Label)
    '        If Not IsNothing(lblName) Then
    '            lblName.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString()
    '        End If
    '        Dim lblTimeOut As Label = DirectCast(e.Row.FindControl("lblTimeOut"), Label)
    '        If Not IsNothing(lblTimeOut) Then
    '            lblTimeOut.Text = DataBinder.Eval(e.Row.DataItem, "TimeOut").ToString()
    '        End If
    '        Dim lblMileOut As Label = DirectCast(e.Row.FindControl("lblMileOut"), Label)
    '        If Not IsNothing(lblMileOut) Then
    '            lblMileOut.Text = DataBinder.Eval(e.Row.DataItem, "MileOut").ToString()
    '        End If
    '        Dim lblTimeIn As Label = DirectCast(e.Row.FindControl("lblTimeIn"), Label)
    '        If Not IsNothing(lblTimeIn) Then
    '            lblTimeIn.Text = DataBinder.Eval(e.Row.DataItem, "TimeIn").ToString()
    '        End If
    '        Dim lblMileIn As Label = DirectCast(e.Row.FindControl("lblMileIn"), Label)
    '        If Not IsNothing(lblMileIn) Then
    '            lblMileIn.Text = DataBinder.Eval(e.Row.DataItem, "MileIn").ToString()
    '        End If

    '    End If
    'End Sub

    Protected Sub ShowPageCommand(s As Object, e As GridViewPageEventArgs)
        'GridView1.PageIndex = e.NewPageIndex
        'BindData()

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If String.IsNullOrEmpty(txtMessDate.Text) Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('กรุณาระบุวันที่');", True)
        ElseIf String.IsNullOrEmpty(txtTimeOutAdd.Text) Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('กรุณาระบุเวลาออก');", True)
        ElseIf String.IsNullOrEmpty(txtMileOutAdd.Text) Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('กรุณาระบุเลขไมล์ออก');", True)
        ElseIf String.IsNullOrEmpty(txtTimeInAdd.Text) Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('กรุณาระบุเวลาเข้า');", True)
        ElseIf String.IsNullOrEmpty(txtMileInAdd.Text) Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('กรุณาระบุเลขไมล์เข้า');", True)
        Else
            Try
                db.tblMessenger.Add(New tblMessenger() With { _
                  .MessDate = DateTime.ParseExact(txtMessDate.Text, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US")), _
                  .Name = lblNameAdd.Text.Trim, _
                  .TimeOut = txtTimeOutAdd.Text.Trim, _
                  .MileOut = CDec(txtMileOutAdd.Text.Trim), _
                  .TimeIn = txtTimeInAdd.Text.Trim, _
                  .MileIn = CDec(txtMileInAdd.Text.Trim), _
                  .CreateBy = Session("Name_eng").ToString.ToUpper, _
                  .CreateDate = Now _
             })
                db.SaveChanges()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect", "alert('บันทึกข้อมูล เรียบร้อยแล้วครับ'); window.location='" + Request.ApplicationPath + "/Messenger.aspx';", True)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('เกิดข้อผิดพลาดในการบันทึก ');", True)
            End Try

        End If

    End Sub

    'Protected Sub btnAddMile_Click(sender As Object, e As EventArgs) Handles btnAddMile.Click
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AddModalScript", "openModalAdd();", True)
    'End Sub

    Protected Sub showMessnger_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles showMessnger.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lblMessDate As Label = DirectCast(e.Item.FindControl("lblMessDate"), Label)
            If Not IsNothing(lblMessDate) Then
                lblMessDate.Text = CStr(DataBinder.Eval(e.Item.DataItem, "MessDate", "{0:dd/MM/yyyy}"))
            End If
            Dim lblName As Label = DirectCast(e.Item.FindControl("lblName"), Label)
            If Not IsNothing(lblName) Then
                lblName.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString()
            End If
            Dim lblTimeOut As Label = DirectCast(e.Item.FindControl("lblTimeOut"), Label)
            If Not IsNothing(lblTimeOut) Then
                lblTimeOut.Text = DataBinder.Eval(e.Item.DataItem, "TimeOut").ToString()
            End If
            Dim lblMileOut As Label = DirectCast(e.Item.FindControl("lblMileOut"), Label)
            If Not IsNothing(lblMileOut) Then
                lblMileOut.Text = DataBinder.Eval(e.Item.DataItem, "MileOut").ToString()
            End If
            Dim lblTimeIn As Label = DirectCast(e.Item.FindControl("lblTimeIn"), Label)
            If Not IsNothing(lblTimeIn) Then
                lblTimeIn.Text = DataBinder.Eval(e.Item.DataItem, "TimeIn").ToString()
            End If
            Dim lblMileIn As Label = DirectCast(e.Item.FindControl("lblMileIn"), Label)
            If Not IsNothing(lblMileIn) Then
                lblMileIn.Text = DataBinder.Eval(e.Item.DataItem, "MileIn").ToString()
            End If
        End If

    End Sub
End Class