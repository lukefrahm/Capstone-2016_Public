﻿using com.GreenThumb.BusinessLogic;
using com.GreenThumb.BusinessObjects;
using com.GreenThumb.BussinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace com.GreenThumb.WPF_Presentation
{
    /// <summary>
    /// Interaction logic for UserRole.xaml
    /// </summary>
    public partial class UserRole : UserControl
    {
        /// <summary>
        /// Author: Ibrahim Abuzaid
        /// Data Transfer Object to represent a User from the
        /// Database
        /// 
        /// Added 3/26 By Ibarahim
        /// </summary>

        private AccessToken _accessToken;
        String insertUpdate = "";
        bool active;
            
        UserManager myUserManager = new UserManager();
        RoleManager myRoleManager = new RoleManager();
        UserRoleManager myUserRoleManager = new UserRoleManager();

        public UserRole(AccessToken accessToken)
        {
            _accessToken = accessToken;
            InitializeComponent();
            PopulateUserGrid(); // Displays User Table data
            PopulateRoleGrid(); // Displays Roles Table Data
            PopulateUserRoleGrid(); // Displays UserRole Table data
            frmUserRoleEdit.Visibility = Visibility.Hidden;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PopulateUserGrid(); // Displays User Table data
            PopulateRoleGrid(); // Displays Roles Table Data
            PopulateUserRoleGrid(); // Displays UserRole Table data
            frmUserRoleEdit.Visibility = Visibility.Hidden;
        }
        private void PopulateUserGrid()
        {
            
            try
            {
                var users = myUserManager.GetUserList(Active.active);
                grdUserList.ItemsSource = users;

                var count = myUserManager.GetUserCount(Active.active);
                lblUserCount.Content = "Count: " + count.ToString();
            }
            catch (Exception)
            {
                grdUserList.ItemsSource = null;
                lblUserCount.Content = "Count: 0";
            }
        }

        private void PopulateRoleGrid()
        {
            try
            {
                var roles = myRoleManager.GetRoleList();
                grdRoleList.ItemsSource = roles;

                var count = myRoleManager.GetRoleCount();
                lblRoleCount.Content = "Count: " + count.ToString();
            }
            catch (Exception)
            {
                grdRoleList.ItemsSource = null;
                lblRoleCount.Content = "Count: 0";
            }
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
   
        }

        private void PopulateUserRoleGrid()
        {
            try
            {
                var userRoles = myUserRoleManager.GetUserRoleList();
                grdUserRoleList.ItemsSource = userRoles;

                var count = myUserRoleManager.GetUserRoleCount();
                lblUserRoleCount.Content = "Count: " + count.ToString();
            }
            catch (Exception)
            {
                grdUserRoleList.ItemsSource = null;

                lblUserRoleCount.Content = "Count: 0";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lblEditWindow1.Content = "Insert New Record";
            lblActive.Visibility = Visibility.Hidden;
            chkBoxActive.Visibility = Visibility.Hidden;
            frmUserRoleEdit.Visibility = Visibility.Visible;
            insertUpdate = "i";
           
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
           lblEditWindow1.Content = "Updating Active Status";
           
           lblActive.Visibility = Visibility.Visible;
           chkBoxActive.Visibility = Visibility.Visible;
           frmUserRoleEdit.Visibility = Visibility.Visible;
           insertUpdate = "u";
           lblMessage.Content = "Active= " + active;
           var bar = Console.ReadLine();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
           
            frmUserRoleEdit.Visibility = Visibility.Hidden;
            var UserID = int.Parse(txtUserID.Text);
            var RoleID = txtRoleID.Text.ToString();
           
            try 
            {
                 if (insertUpdate.Equals("i"))
                 {
                       myUserRoleManager.AddNewUserRole(UserID, RoleID, _accessToken.UserID, DateTime.Now);
                       lblCrudRes.Content = "Record Inserted successfully.";
                 }
                  else
                 if (insertUpdate.Equals("u"))
                 {
                      var res =  myUserRoleManager.ChangeUserRoleStatus(UserID, RoleID, active);
                      if (res == true)
                      {
                          lblCrudRes.Content = "Record Updated successfully.";
                          lblMessage.Content = "Operation Succeeded, Active= " + active;
                      }
                      else
                          lblMessage.Content = "Operation failed. returned false " + active;
                 }
 
            }
             catch (Exception)
            {
                 lblCrudRes.Content = "Operation Failed, check out!";
            }
            finally
            {
                insertUpdate = "";
                txtUserID.Text = "";
                txtRoleID.Text = "";
            }
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmUserRoleEdit.Visibility = Visibility.Hidden;
            insertUpdate = "";
            txtUserID.Text = "";
            txtRoleID.Text = "";
        }

        private void chkBoxActive_Checked(object sender, RoutedEventArgs e)
        {
            active = true;
        }
        private void chkBoxActive_UnChecked(object sender, RoutedEventArgs e)
        {
            active = false;
        }
    }
}