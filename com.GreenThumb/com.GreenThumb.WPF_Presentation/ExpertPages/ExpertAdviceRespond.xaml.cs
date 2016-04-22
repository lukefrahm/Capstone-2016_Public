﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using com.GreenThumb.BusinessLogic;
using com.GreenThumb.BusinessObjects;

namespace com.GreenThumb.WPF_Presentation.ExpertPages
{
    /// <summary>
    /// Interaction logic for ExpertAdviceRespond.xaml
    /// </summary>
    public partial class ExpertAdviceRespond : Page
    {
        QuestionManager questionManager = new QuestionManager();
        ResponseManager responseManager = new ResponseManager();
        RegionManager regionManager = new RegionManager();
        UserManager userManager = new UserManager();
        AccessToken _accessToken = new AccessToken();
        
        public ExpertAdviceRespond(AccessToken accessToken)
        {
            InitializeComponent();
            
            List<Region> regions = regionManager.GetRegions();
            _accessToken = accessToken;
            cmbRegions.Items.Add("All regions");
            cmbRegions.Items.Add("No region");
            foreach(Region region in regions)
            {
                cmbRegions.Items.Add(region);
            }

            cmbRegions.SelectedIndex = 0;
        }

        private void btnSearchRegion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Region region = (Region)cmbRegions.SelectedValue;
                gridQuestions.ItemsSource = questionManager.GetQuestionsByRegionID(region.RegionID);
            }
            catch(Exception)
            {
                if(cmbRegions.SelectedIndex == 1)
                {
                    gridQuestions.ItemsSource = questionManager.GetQuestionsWithNoRegion();
                }
                else if(cmbRegions.SelectedIndex == 0)
                {
                    gridQuestions.ItemsSource = questionManager.GetQuestions();
                }
            }

            ChangeGridVisibility();
            txtKeywords.Text = "";
        }

        private void gridQuestions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtResponse.IsEnabled = true;
            btnResponse.IsEnabled = true;
            Question question = new Question();
            try
            {
                question = (Question)gridQuestions.SelectedItem;
                lblContent.Content = question.Content;
                lblQuestion.Content = "Question asked by " + userManager.RetrieveUser(question.CreatedBy).UserName;

                Response response = responseManager.GetResponseByQuestionIDAndUser(question.QuestionID, _accessToken.UserID);
                if(response.QuestionID == question.QuestionID)
                {
                    btnResponse.Content = "Edit";
                    txtResponse.Text = response.UserResponse;
                }
                else
                {
                    btnResponse.Content = "Send";
                    txtResponse.Text = "";
                }
            }
            catch (Exception)
            {
                lblContent.Content = "";
                lblQuestion.Content = "Question:";
            }
        }

        private void ChangeGridVisibility()
        {
            if (gridQuestions.Items.Count > 0)
            {
                gridQuestions.Visibility = System.Windows.Visibility.Visible;
                lblNoMatch.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                gridQuestions.Visibility = System.Windows.Visibility.Collapsed;
                lblNoMatch.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void txtKeywords_KeyUp(object sender, KeyEventArgs e)
        {
            try // Selected region
            {
                Region region = (Region)cmbRegions.SelectedValue;
                gridQuestions.ItemsSource = questionManager.GetQuestionsWithKeywordAndRegion(txtKeywords.Text, region.RegionID);
            }
            catch (Exception)
            {
                if (cmbRegions.SelectedIndex == 0) // All regions
                {
                    gridQuestions.ItemsSource = questionManager.GetQuestionsWithKeyword(txtKeywords.Text);
                }
                else if (cmbRegions.SelectedIndex == 1) // No region
                {
                    gridQuestions.ItemsSource = questionManager.GetQuestionsWithKeywordAndRegion(txtKeywords.Text, null);
                }
            }

            ChangeGridVisibility();
        }

        private void btnResponse_Click(object sender, RoutedEventArgs e)
        {
            Response response = new Response();
            string responseText = txtResponse.Text;

            if(responseText.Length > 0)
            {
                try
                {
                    Question question = (Question)gridQuestions.SelectedItem;

                    response.Date = DateTime.Now;
                    response.QuestionID = question.QuestionID;
                    response.UserID = _accessToken.UserID;
                    response.UserResponse = responseText;

                    if (responseText.Length <= 250)
                    {
                        responseManager.AddResponse(response);
                        btnResponse.Content = "Edit";
                    }
                }
                catch (Exception)
                {
                    Response oldResponse = responseManager.GetResponseByQuestionIDAndUser(response.QuestionID, _accessToken.UserID);
                    responseManager.EditResponse(response, oldResponse);
                }
            }
            else
            {
                lblReply.Content = "Enter a response";
                lblReply.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void txtResponse_KeyUp(object sender, KeyEventArgs e)
        {
            string reply = txtResponse.Text;
            
            if(reply.Length > 250)
            {
                lblReply.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                lblReply.Foreground = System.Windows.Media.Brushes.Black;
            }

            lblReply.Content = "Your reply: " + (250 - reply.Length) + " characters remaining";
        }
    }
}
